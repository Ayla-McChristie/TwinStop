using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Teleporter : Enemy
{
    protected enum State { Attack, Teleport, Setting }

    State state;

    [SerializeField]
    public int health = 15;
    [SerializeField]
    public int projectileSpeed = 8;
    [SerializeField]
    public float minSpeed = 8;
    [SerializeField]
    public float maxSpeed = 10;
    [SerializeField]
    float rangeOfTeleport = 3f;
    [SerializeField]
    float attackRate = 1f;

    List<Transform> projectileStartPos;
    GameObject projectile;
    NavMeshPath path;
    Collider collider;

    Vector3 newPos;

    string projectileType = "EnemyProjectile";
    int index = 0;
    int count;
    float attackCoolDown;
    bool shotFired;
    bool setNewPos;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        SetProjectile();
        this.Health = health;
        this.Speed = maxSpeed;
        state = State.Attack;
        path = new NavMeshPath();
        setNewPos = false;
        deathSound = GetComponent<AudioSource>();
        collider = this.GetComponent<Collider>();
    }

    // Update is called once per frame
    public override void FixedUpdate()
    {
        if (IsDead())
            return;
        SwitchState();
        SwitchMethod();
        ChangeSpeed();
        CollisionModifier();
        base.FixedUpdate();
    }

    void SetProjectile()
    {
        projectileStartPos = new List<Transform>();
        this.transform.Find("projectiles4").gameObject.SetActive(true);
        count = this.transform.Find("projectiles4").childCount;
        SetShootAspect(count, this.transform.Find("projectiles4"));
    }

    void SetShootAspect(int count, Transform projectileSet)
    {
        for (int i = 0; i < count; i++)
        {
            if (projectileSet.GetChild(i).name == "projectileStart")
            {
                projectileStartPos.Add(projectileSet.GetChild(i));
            }
        }
    }

    bool IsDead()
    {
        if (isDead)
            return true;
        return false;
    }

    void SwitchState()
    {
        if (!shotFired)
        {
            state = State.Attack;
            return;
        }
        if (setNewPos)
            return;
        state = State.Setting;
    }

    void SwitchMethod()
    {
        switch (state)
        {
            case State.Attack:
                LookAtEnemy();
                Fire();
                break;
            case State.Teleport:
                MoveToNewPos();
                AtNewPosition();
                break;
            case State.Setting:
                Teleport();
                break;
        }
    }

    void CollisionModifier()
    {
        if (!TimeManager.Instance.isTimeStopped && state == State.Teleport)
        {
            collider.enabled = false;
            return;
        }
        collider.enabled = true;
    }

    void ChangeSpeed()
    {
        if (!TimeManager.Instance.isTimeStopped)
        {
            this.Speed = maxSpeed;
            return;
        }
        this.Speed = minSpeed;
    }

    void Fire()
    {
        if (PauseScript.Instance.isPaused)
            return;
        if (!CoolDown())
            return;
        for(int i = 0; i < projectileStartPos.Count; i++)
        {
            projectile = ObjectPool_Projectiles.Instance.GetProjectile(projectileType); //Getting the projectile gameobject
            projectile.GetComponent<Projectile>().b_Speed = projectileSpeed;
            projectile.GetComponent<Projectile>().SetUp(projectileStartPos[i].transform.forward, projectileStartPos[i].transform.position, "Enemy");
        }
        shotFired = true;
    }

    void LookAtEnemy()
    {
        this.transform.LookAt(new Vector3(target.transform.position.x, this.transform.position.y, target.transform.position.z));
    }

    void Teleport()
    {
        if (PauseScript.Instance.isPaused)
            return;
        Vector3 randomLoc = target.transform.position *2f  + Random.insideUnitSphere * rangeOfTeleport;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomLoc, out hit, 10f, NavMesh.AllAreas))
        {
            NavMesh.CalculatePath(this.transform.position, hit.position, NavMesh.AllAreas, path);
            newPos = hit.position;
            setNewPos = true;
            state = State.Teleport;
        }
    }

    void AtNewPosition()
    {
        Vector3 distance = newPos - this.transform.position;
        if (distance.magnitude < 1f)
        {
            index = 0;
            attackCoolDown = 0;
            shotFired = false;
            setNewPos = false;
        }
    }

    void MoveToNewPos()
    {
        if (index >= path.corners.Length)
            return;
        float distance = Vector3.Distance(this.transform.position, path.corners[index]);
        if (distance < 1f)
        {
            index++;
            return;
        }
        this.transform.position = Vector3.MoveTowards(this.transform.position, path.corners[index], this.Speed * Time.deltaTime);
    }

    bool CoolDown()
    {
        
        if (attackCoolDown > attackRate)
            return true;
        attackCoolDown += Time.deltaTime;
        return false;
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        Debug.Log(this.Health);
        base.OnCollisionEnter(collision);
    }
}
