using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniBossBlue : Sentinel
{
    enum MiniBossState { Attack, SpecialAttack, Dead, Spawn, offline }
    MiniBossState mState;
    [SerializeField] GameObject roomTrigger;
    [SerializeField] float spawnRate;

    [SerializeField] MiniBossEnemySpawn[] enemySpawn;
    [SerializeField] int[] spawnAtHealthPercent;


    float burstTimer;
    float waveTimer;
    int fireIntervals;
    int intervalCounter;
    int maxHealth;
    bool modify;
    bool isActive;

    // Update is called once per frame
    public override void Start()
    {
        base.Start();
        mState = MiniBossState.offline;
        fireIntervals = 1;
        maxHealth = (int)Health;
        isActive = this.transform.Find("ActivationTrigger").GetComponent<SentryTrigger>().isTriggered;
        if (enemySpawn != null)
            TurnOffSpawners();
    }

    void TurnOffSpawners()
    {
        foreach (MiniBossEnemySpawn e in enemySpawn)
            e.gameObject.SetActive(false);
    }

    public override void FixedUpdate()
    {
        if (PauseScript.Instance.isPaused)
            return;
        SwitchState();
        RotateToPlayer();
        ModTest();
        DamageFlash();
        GetActivation();
        CheckHealth(); Debug.Log(Health);
        EnemySpawnerSystem();
    }

    void GetActivation()
    {
        Debug.Log(isActive);
        if (!this.transform.Find("ActivationTrigger").GetComponent<SentryTrigger>().isTriggered)
            return;
        mState = MiniBossState.Attack;
    }

    void EnemySpawnerSystem()
    {
        SpawnOnHealthPercent();
        if (waveTimer >= spawnRate)
        {
            SpawnersActivation("Group");
            waveTimer = 0;
            return;
        }
        if (waveTimer >= 2.2f)
            TurnOffSpawners();
        waveTimer += Time.deltaTime;
    }

    void SpawnOnHealthPercent()
    {
        for (int i = 0; i < spawnAtHealthPercent.Length; i++)
        {
            if ((int)PercentCalc() == spawnAtHealthPercent[i])
            {
                SpawnersActivation("Wave");
            }

        }
    }

    void SpawnersActivation(string type)
    {
        for (int i = 0; i < enemySpawn.Length; i++)
        {
            enemySpawn[i].gameObject.SetActive(true);
            if (type == "Group")
                SpawnGroup(enemySpawn[i]);
            else
                SpawnWave(enemySpawn[i]);
        }
    }

    void SpawnGroup(MiniBossEnemySpawn m)
    {
        m.SpawnEnemy();
    }

    void SpawnWave(MiniBossEnemySpawn m)
    {
        m.GetEnemyWave();
    }

    void ModTest()
    {
        if ((int)PercentCalc() == 60)
            fireCount = 5;
        if ((int)PercentCalc() == 30)
            fireCount = maxCount;
        if ((int)PercentCalc() == 50)
            fireIntervals = 2;
        if ((int)PercentCalc() == 40)
            fireIntervals = 3;
    }

    float PercentCalc()
    {
        return (this.Health / maxHealth) * 100;
    }

    void RotateToPlayer()
    {
        Vector3 targetDir = target.transform.position - this.transform.position;
        Vector3 direction = Vector3.RotateTowards(this.transform.forward, targetDir, Time.deltaTime, 0.0f);
        this.transform.rotation = Quaternion.LookRotation(direction);
    }

    protected override void AttackTarget()
    {
        if (!coolDown && !target.GetComponent<PlayerStats>().isDead)
        {
            FireIntervals();
        }
    }

    void Shoot()
    {
        for (int i = 0; i < fireCount; i++)
        {
            projectile = ObjectPool_Projectiles.Instance.GetProjectile(projectileType); //Getting the projectile gameobject
            projectile.GetComponent<Projectile>().b_Speed = projectileSpeed;
            projectile.GetComponent<Projectile>().SetUp(projectileStartPos[i].forward, projectileStartPos[i].position, "Enemy");
        }
    }

    void FireIntervals()
    {
        if (intervalCounter >= fireIntervals)
        {
            coolDown = true;
            intervalCounter = 0;
            return;
        }
        if (burstTimer >= .2f)
        {
            Shoot();
            burstTimer = 0;
            intervalCounter++;
            return;
        }
        burstTimer += Time.deltaTime;

    }

    void SwitchState()
    {
        switch (mState)
        {
            case MiniBossState.Attack:
                AttackTarget();
                AttackCoolDown();
                break;
            case MiniBossState.Dead:
                break;
        }
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
    }
}
