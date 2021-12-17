using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Sentinel : Enemy
{
    protected enum State { Attacking, MoveToTarget}
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
    bool projectiles4 = false;
    [SerializeField]
    bool projectiles5 = false;
    [SerializeField]
    float projectileSpeed = 6;
    State state;
    protected float moveRange = 5f;

    List<Transform> projectileStartPos;
    GameObject projectile;


    Vector3 moveDir;
    Vector3 destination; //the distance between destinated location to this gameobject
    Vector3 projectileDir;
    float fireTimer = 0;
    float waitTimer = 0;
    public float maxDist = 10f;
    public float minDist = 5f;

    string projectileType = "EnemyProjectile";

    bool coolDown;
    bool waitToMove;
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
    }

    void CheckProjectileSet()
    {
        int count;
        if (projectiles3)
        {
            this.transform.Find("projectiles3").gameObject.SetActive(true);
            count = this.transform.Find("projectiles3").childCount;
            SetShootAspect(count, this.transform.Find("projectiles3"));
        }
        else if (projectiles4)
        {
            this.transform.Find("projectiles4").gameObject.SetActive(true);
            count = this.transform.Find("projectiles4").childCount;
            SetShootAspect(count, this.transform.Find("projectiles4"));
        }
        else if (projectiles5)
        {
            this.transform.Find("projectiles5").gameObject.SetActive(true);
            count = this.transform.Find("projectiles5").childCount;
            SetShootAspect(count, this.transform.Find("projectiles5"));
        }
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
            if (!CanSeeTarget())
                state = State.MoveToTarget;
            else if (CanSeeTarget())
                state = State.Attacking;
            Physics.IgnoreCollision(GameObject.FindWithTag("Enemy").GetComponent<Collider>(), this.gameObject.GetComponent<Collider>(), true);
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
            this.transform.LookAt(target.transform.position);
        }
        base.FixedUpdate();
    }

    void AttackTarget()
    {
        if(!coolDown && !target.GetComponent<PlayerStats>().isDead)
        {
            for(int i = 0; i < projectileStartPos.Count; i++)
            {
                projectile = ObjectPool_Projectiles.Instance.GetProjectile(projectileType); //Getting the projectile gameobject
                projectile.GetComponent<Projectile>().b_Speed = projectileSpeed;
                projectile.GetComponent<Projectile>().SetUp(projectileStartPos[i].forward, projectileStartPos[i].position, "Enemy");
                //projectile.transform.position = projectileStart.transform.position;
                //if (GetPredictedDir(target.transform.position, this.transform.position, target.GetComponent<Rigidbody>().velocity, projectile.GetComponent<Projectile>().b_Speed, out var direction))
                //{
                //    //projectile.GetComponent<Rigidbody>().velocity = direction * projectile.GetComponent<Projectile>().b_Speed;
                //    projectile.GetComponent<Projectile>().SetUp(direction, projectileStartPos[i].position, "Enemy");
                //}
                //else
                //{
                //    //projectile.GetComponent<Rigidbody>().velocity = (target.transform.position- this.transform.position) * projectile.GetComponent<Projectile>().b_Speed;
                //    projectile.GetComponent<Projectile>().SetUp((target.transform.position - this.transform.position), projectileStartPos[i].position, "Enemy");
                //}
                //Space out when the enemy can shoot again
                //Debug.Log(projectile.transform.position);
                //projectileDir.y = 0;
                //projectileDir = projectileDir.normalized;
                //projectile.GetComponent<Projectile>().SetUp(projectileDir, this.transform.position, this.gameObject.tag); //Activating projectile with it's direction, starting position, and the tag of the user          

            }
            coolDown = true;
        }
    }

    bool GetPredictedDir(Vector3 targetPos, Vector3 shooterPos, Vector3 targetV, float b_Speed, out Vector3 result)
    {
        var targetToShooter = targetPos - shooterPos;
        var distance = targetToShooter.magnitude;
        var angle = Vector3.Angle(targetPos, targetV) * Mathf.Deg2Rad;

        var speedTar = targetV.magnitude;
        var r = speedTar / b_Speed;
        if (SolveQuadratic(1 - r * r, 2 * r * distance * Mathf.Cos(angle), -(distance * distance), out var root1, out var root2) == 0)
        {
            result = Vector3.zero;
            return false;
        }
        var projectedDist = Mathf.Max(root1, root2);
        var t = projectedDist / b_Speed;
        var c = targetPos + targetV * t;
        result = (c - shooterPos).normalized;
        return true;
    }

    int SolveQuadratic(float a, float b, float c, out float root1, out float root2)
    {
        var discriminant = b * b - 4 * a * c;
        if (discriminant < 0)
        {
            root1 = Mathf.Infinity;
            root2 = -root1;
            return 0;
        }

        root1 = (-b + Mathf.Sqrt(discriminant)) / (2 * a);
        root2 = (-b - Mathf.Sqrt(discriminant)) / (2 * a);

        return discriminant > 0 ? 2 : 1;
    }


    void AttackCoolDown() //timer for when the enemy can shoot again
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
            return true;
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
