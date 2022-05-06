using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class MiniBossScript : Sentinel
{
    enum MiniBossState {Attack, SpecialAttack, Dead }
    MiniBossState mState;
    [SerializeField] GameObject roomTrigger;
    [SerializeField] GameObject MeteorMarker;
    [SerializeField] float specialAttackRate;
    [SerializeField]int meteorCount;

    float specialAttackTimer;
    float specialAttackInterval;
    Vector3[] randomLoc;
    List<GameObject> meteors;
    // Update is called once per frame
    public override void Start()
    {
        base.Start();
        mState = MiniBossState.Attack;
        specialAttackRate = 20f;
        meteors = new List<GameObject>();
        CreateMeteorMarkers();
        MeteorMarker.SetActive(false);
        randomLoc = new Vector3[meteorCount];
    }

    void CreateMeteorMarkers()
    {
        for(int i = 0; i < meteorCount; i++)
        {
            meteors.Add(Instantiate(MeteorMarker));
            meteors[i].SetActive(false);
        }
    }
    public override void FixedUpdate()
    {
        if (PauseScript.Instance.isPaused)
            return;
        SwitchState();
        SpecialAttackTimer();
        RotateToPlayer();
        Debug.Log(target.transform.position);
    }

    void SpecialAttackTimer()
    {
        if (mState == MiniBossState.SpecialAttack)
            return;
        if(specialAttackTimer >= specialAttackRate)
        {
            mState = MiniBossState.SpecialAttack;
            specialAttackTimer = 0;
            return;
        }
        specialAttackTimer += Time.deltaTime;
    }

    void GetMeteors()
    {
        for(int i = 0; i < meteorCount; i++)
        {
            Debug.Log(i + " " + meteorCount);
            if(i <= 0)
            {
                randomLoc[i] = target.transform.position;
                meteors[i].transform.position = target.transform.position + new Vector3(0, .1f, 0);
            }
            else
            {
                NavMeshHit hit;
                if(NavMesh.SamplePosition(target.transform.position + Random.insideUnitSphere * 10f,out hit, 10f, NavMesh.AllAreas))
                {
                    randomLoc[i] = hit.position;
                    meteors[i].transform.position = hit.position;
                }
            }
        }
    }

    void RotateToPlayer()
    {
        Vector3 targetDir = target.transform.position - this.transform.position;
        Vector3 direction = Vector3.RotateTowards(this.transform.forward, targetDir, Time.deltaTime, 0.0f);
        this.transform.rotation = Quaternion.LookRotation(direction);
    }

    void MeteorShower()
    {
        if (specialAttackInterval < 3f)
        {
            specialAttackInterval += Time.deltaTime;
            return;
        }
        foreach(Vector3 v in randomLoc)
        {
            projectile = ObjectPool_Projectiles.Instance.GetProjectile(projectileType);
            projectile.GetComponent<Projectile>().b_Speed = 2f;
            projectile.GetComponent<Projectile>().SetUp(Vector3.down, new Vector3(v.x, v.y + 10f, v.z), "Enemy");
        }
        mState = MiniBossState.Attack;
    }

    void DisplayMeteorDestination()
    {
        foreach (GameObject g in meteors)
            g.SetActive(true);

    }

    void TurnOffMarkers()
    {
        if (!meteors[0].activeSelf)
            return;
        if (specialAttackTimer <= 5f)
            return;
        foreach (GameObject g in meteors)
            g.SetActive(false);

    }

    void SwitchState()
    {
        switch (mState)
        {
            case MiniBossState.Attack:
                AttackTarget();
                AttackCoolDown();
                TurnOffMarkers();
                break;
            case MiniBossState.Dead:
                break;
            case MiniBossState.SpecialAttack:  
                GetMeteors();
                DisplayMeteorDestination();
                MeteorShower();
                break;
        }
    }
}
