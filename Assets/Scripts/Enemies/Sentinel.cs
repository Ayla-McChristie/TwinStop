using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sentinel : Enemy
{
    [SerializeField]
    public float speed = 3;
    [SerializeField]
    public float health = 10;
    [SerializeField]
    public float damage = 2;
    [SerializeField]
    public float attackRange = 2;
    [SerializeField]
    public float attackRate = 1;
    public LayerMask mask;
    State state;
    protected float moveRange = 5f;

    GameObject projectileStart;
    GameObject projectile;

    Vector3 moveDir;
    Vector3 destination; //the distance between destinated location to this gameobject
    Vector3 dist2Target; //distance between this gameobject and the target
    float fireTimer = 0;
    float waitTimer = 0;
    float fovDist = 100.0f;
    public float maxDist = 10f;
    public float minDist = 5f;
    string projectileType = "EnemyProjectile";

    bool coolDown;
    bool waitToMove;
    // Start is called before the first frame update
    public override void Start()
    {
        moveDir = new Vector3(Random.Range(this.transform.position.x, this.transform.position.x), this.transform.position.y, Random.Range(this.transform.position.z, this.transform.position.z));
        projectileStart = this.gameObject.transform.GetChild(0).gameObject;
        state = State.Attacking;
        this.Health = health;
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * fovDist, Color.red, 1, true); Debug.Log(state);

        if (!CanSeeTarget())
            state = State.MoveToTarget;
        else if (CanSeeTarget())
            state = State.Attacking;

        LookAtTarget();
        switch (state)
        {
            case State.Attacking:
                AttackTarget();
                AttackCoolDown();
                MoveAround();
                break;
            case State.MoveToTarget:
                break;
        }
    }

    void AttackTarget()
    {
        if(!coolDown)
        {
            Vector3 projectileDir = target.transform.position - this.transform.position; //the direction of projectile is heading 
            projectileDir = projectileDir.normalized; 
            projectile = oP.GetProjectile(projectileType); //Getting the projectile gameobject
            projectile.GetComponent<Projectile>().SetUp(projectileDir, this.transform.position, this.gameObject.tag); //Activating projectile with it's direction, starting position, and the tag of the user
            coolDown = true; //Space out when the enemy can shoot again
        }
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
            if(destination.magnitude <= 1f)
            {
                moveDir = MoveBasedOnLocation();
                waitToMove = true;
            }
            else
                this.transform.position = Vector3.MoveTowards(this.transform.position, moveDir, speed * Time.deltaTime);
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

    void GetNewDestination()
    {
        moveDir = new Vector3(Random.Range(target.transform.position.x - moveRange, target.transform.position.x + moveRange), this.transform.position.y, Random.Range(target.transform.position.z - moveRange, target.transform.position.z + moveRange));
        waitToMove = true;
    }

    void LookAtTarget() //Always facing the player
    {
        transform.LookAt(target.transform);
    }

    bool CanSeeTarget()
    {
        Vector3 direction = target.transform.position - this.transform.position;
        float angle = Vector3.Angle(direction, this.transform.position);

        RaycastHit hit;
        if (Physics.Raycast(this.transform.position, direction, out hit, fovDist, mask) 
                            && hit.collider.gameObject.tag == "Player")
            return true;
        return false;
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Obstacle")
            GetNewDestination();

        base.OnCollisionEnter(collision);   
    }
}
