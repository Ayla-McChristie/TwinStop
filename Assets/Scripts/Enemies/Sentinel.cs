using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Sentinel : Enemy
{
    protected enum State {Spawn, Attacking, MoveToTarget}
    [SerializeField]
    public float speed = 3;
    [SerializeField]
    public int health = 10;
    [SerializeField]
    public int damage = 2;
    [SerializeField]
    public float attackRange = 2;
    [SerializeField]
    public float attackRate = 1;
    [SerializeField]
    bool projectiles3 = true;
    [SerializeField]
    bool projectiles5 = false;
    [SerializeField]
    protected float projectileSpeed = 6;
    State state;
    protected float moveRange = 5f;

    protected List<Transform> projectileStartPos;
    protected GameObject projectile;

    [SerializeField]
    Renderer flashrender;

    Vector3 moveDir;
    Vector3 destination; //the distance between destinated location to this gameobject
    Vector3 projectileDir;
    float fireTimer = 0;
    float waitTimer = 0;
    float spawnTimer = 0;
    public float maxDist = 10f;
    public float minDist = 5f;

    protected string projectileType = "EnemyProjectile";
    protected int maxCount;
    protected bool coolDown;
    bool waitToMove;
    protected int fireCount;
    // Start is called before the first frame update
    public override void Start()
    {
        moveDir = new Vector3(Random.Range(this.transform.position.x, this.transform.position.x), this.transform.position.y, Random.Range(this.transform.position.z, this.transform.position.z));
        state = State.MoveToTarget;
        this.Health = health;
        this.Speed = speed;
        this.Damage = damage;
        projectileStartPos = new List<Transform>();
        CheckProjectileSet();
        deathSound = GetComponent<AudioSource>();
        base.Start();
        FlashRenderer = flashrender;
        
    }

    void CheckProjectileSet()
    {

        this.transform.Find("Projectiles").gameObject.SetActive(true);
        maxCount = this.transform.Find("Projectiles").childCount;
        SetShootAspect(maxCount, this.transform.Find("Projectiles"));
        if (projectiles5)
        {
            fireCount = 5;
            return;
        }
        fireCount = 3;
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

    // Update is called once per frame
    public override void FixedUpdate()
    {
        Debug.DrawRay(transform.position, transform.forward * fovDist, Color.red, 1, true); //Debug.Log(state);
        DeathSoundClipTime();
        if (!isDead)
        {
            Spawning();
            switch (state)
            {
                case State.Attacking:
                    AttackTarget();
                    AttackCoolDown();
                    MoveAround();
                    break;
                case State.MoveToTarget:
                    MoveToTarget();
                    break;
            }
            if (state == State.Spawn)
                return;
            if (!CanSeeTarget())
                state = State.MoveToTarget;
            if (CanSeeTarget() && spawnTimer >= .25f)
                state = State.Attacking;
            Physics.IgnoreCollision(GameObject.FindWithTag("Enemy").GetComponent<Collider>(), this.gameObject.GetComponent<Collider>(), true);
            this.transform.LookAt(target.transform.position);
        }
        base.FixedUpdate();
    }

    void Spawning()
    {
        if (spawnTimer <= .25f)
            spawnTimer += Time.deltaTime;
    }

    protected virtual void AttackTarget()
    {
        if(!coolDown && !target.GetComponent<PlayerStats>().isDead)
        {
            for(int i = 0; i < fireCount; i++)
            {
                projectile = ObjectPool_Projectiles.Instance.GetProjectile(projectileType); //Getting the projectile gameobject
                projectile.GetComponent<Projectile>().b_Speed = projectileSpeed;
                projectile.GetComponent<Projectile>().SetUp(projectileStartPos[i].forward, projectileStartPos[i].position, "Enemy");
            }
            coolDown = true;
        }
    }

    protected void AttackCoolDown() //timer for when the enemy can shoot again
    {
        if (coolDown)
        {
            fireTimer += Time.deltaTime;
            if (fireTimer >= attackRate)
            {
                fireTimer = 0;
                coolDown = false;
            }
        }
    }

    void MoveAround() //Move in a random location based on Player's location
    {
        if (!waitToMove)
        {
            destination = moveDir - this.transform.position;
            if (destination.magnitude <= 1f)
            {
                moveDir = MoveBasedOnLocation();
            }
            else
            {
                NavMeshHit hit;
                if (NavMesh.SamplePosition(moveDir, out hit, 1.0f, NavMesh.AllAreas))
                {
                    agent.SetDestination(hit.position);
                    waitToMove = true;
                }
                else
                    destination = this.transform.position;
            }
        }
        else
        {
            waitTimer += Time.deltaTime;
            if (waitTimer >= 1)
            {
                waitTimer = 0;
                waitToMove = false;
            }
        }
    }

    Vector3 MoveBasedOnLocation() //This is to check the distance between the player and this gameobject, then compare x and z values. If one is greater the other, then the gameobject will stay in that axis
    {
        Vector3 distance = target.transform.position - this.transform.position;
        if (distance.x > distance.z)
            return MoveBasedX();
        else if (distance.z > distance.x)
            return MoveBasedZ();
        return Vector3.zero;
    }

    Vector3 MoveBasedX()
    {
        if (this.transform.position.x < target.transform.position.x)
            return new Vector3(Random.Range(target.transform.position.x - minDist, target.transform.position.x - maxDist),
                                  this.transform.position.y,
                                  Random.Range(target.transform.position.z - moveRange, target.transform.position.z + moveRange));

        else if (this.transform.position.x > target.transform.position.x)
            return new Vector3(Random.Range(target.transform.position.x + minDist, target.transform.position.x + maxDist),
                                  this.transform.position.y,
                                  Random.Range(target.transform.position.z - moveRange, target.transform.position.z + moveRange));

        return Vector3.zero;    
    }

    Vector3 MoveBasedZ()
    {
        if (this.transform.position.z < target.transform.position.z)
            return new Vector3(Random.Range(target.transform.position.x - moveRange, target.transform.position.x + moveRange),
                                  this.transform.position.y,
                                  Random.Range(target.transform.position.z - minDist, target.transform.position.z - maxDist));

        else if (this.transform.position.z > target.transform.position.z)
            return new Vector3(Random.Range(target.transform.position.x - moveRange, target.transform.position.x + moveRange),
                                  this.transform.position.y,
                                  Random.Range(target.transform.position.z + minDist, target.transform.position.z + maxDist));

        return Vector3.zero;
    }

    bool CanSeeTarget()
    {
        Vector3 direction = target.transform.position - this.transform.position;
        float angle = Vector3.Angle(direction, this.transform.position);

        RaycastHit hit;
        if (Physics.Raycast(this.transform.position, this.transform.forward, out hit, fovDist, mask) 
                            && hit.collider.gameObject.tag == "Player")
        {
            Debug.Log(hit.collider.name);
            return true;
        }
        return false;
    }

    void MoveToTarget()
    {
        agent.SetDestination(target.transform.position);
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);   
    }
}
