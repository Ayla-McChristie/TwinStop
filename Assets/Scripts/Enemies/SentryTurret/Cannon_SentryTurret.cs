using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon_SentryTurret : Enemy
{
    protected enum State {Offline, Attack, Online }
    [SerializeField]
    public int health = 10;
    [SerializeField]
    public int damage = 2;
    [SerializeField]
    public float attackRate = 1;

    Vector3 playerDir;

    GameObject projectile;
    public GameObject projectileStartPos;
    public GameObject RoomTrigger;
    public bool isNotSentry;

    bool coolDown;

    float fireTimer = 0;
    float projectileSpeed = 12;
    float bootUpTimer = 0;

    string projectileType;

    State state;
    // Start is called before the first frame update
    public override void Start()
    {
        if (isNotSentry)
            state = State.Attack;
        else
            state = State.Offline;
        Debug.Log(state);
        projectileType = "EnemyProjectile";
        base.Start();
        target = GameObject.FindGameObjectWithTag("Player");
        this.Health = health;
        this.Damage = damage;
        defaultMat = FlashRenderer.material;
        rigidbody = GetComponent<Rigidbody>();
        deathSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    public override void FixedUpdate()
    {
        Debug.Log(state);
        CheckIfObjectOnScreen();
        //DeadState();
        SwitchState();
        base.FixedUpdate();
    }

    void CheckIfObjectOnScreen()
    {
        if (isNotSentry)
            return;
        if (hasChildrenRender)
            if (this.transform.GetComponentInChildren<Renderer>().isVisible)
            {
                TimeBeforeStart();
                return;
            }
        else
            if (this.transform.GetComponentInChildren<Renderer>().isVisible)
            {
                {
                    TimeBeforeStart();
                    return;
                }
            }
        state = State.Offline;
        bootUpTimer = 0;
        return;
    }

    void TimeBeforeStart()
    {
        if (bootUpTimer <= .2f)
            bootUpTimer += Time.deltaTime;
        else
            state = State.Attack;
    }

    void DeadState()
    {
        if (isDead)
        {
            DeathSoundClipTime();
            return;
        }
    }

    void DetectPlayerEnterRoom()
    {
        if(RoomTrigger == null)
        {
            state = State.Online;
            return;
        }
        if (RoomTrigger.GetComponent<SentryTrigger>().isTriggered)
            state = State.Online;
    }

    void SeeTargetState()
    {
        if (isNotSentry)
        {
            this.transform.LookAt(new Vector3(target.transform.position.x, this.transform.position.y, target.transform.position.z));
            return;
        }
        if (CanSeeTarget())
        {
            state = State.Attack;
            return;
        }
        state = State.Online;
    }

    void SwitchState()
    {
        switch (state)
        {
            case State.Online:
                SeeTargetState();
                break;
            case State.Offline:
                //DetectPlayerEnterRoom();
                break;
            case State.Attack:
                SeeTargetState();
                AttackTarget();
                AttackCoolDown();     
                break;  
        }
    }
    
    bool CanSeeTarget()
    {
        this.transform.LookAt(new Vector3(target.transform.position.x, this.transform.position.y, target.transform.position.z));
        RaycastHit hit;
        if (Physics.Raycast(this.transform.position, transform.forward, out hit, fovDist, mask)
                            && hit.collider.gameObject.tag == "Player")
            return true;
        return false;
    }

    protected override void DeathSoundClipTime()
    {
        if (Health <= 0)
        {
            clipTimer += Time.deltaTime;
            if (clipTimer >= deathSound.clip.length)
            {
                if(this.transform.parent != null)
                    this.gameObject.transform.parent.GetComponent<Collider>().enabled = false;
                else
                    this.transform.GetComponent<Collider>().enabled = false;
                this.gameObject.SetActive(false);

            }
        }
    }

    void AttackTarget()
    {
        if (!coolDown && !target.GetComponent<PlayerStats>().isDead)
        {
            projectile = ObjectPool_Projectiles.Instance.GetProjectile(projectileType); //Getting the projectile gameobject
            projectile.GetComponent<Projectile>().b_Speed = projectileSpeed;
            if (GetPredictedDir(new Vector3(target.transform.position.x, this.transform.position.y, target.transform.position.z), this.transform.position, target.GetComponent<Rigidbody>().velocity, projectile.GetComponent<Projectile>().b_Speed, out var direction))
                projectile.GetComponent<Projectile>().SetUp(direction, projectileStartPos.transform.position, "Enemy");
            else
                projectile.GetComponent<Projectile>().SetUp((target.transform.position - this.transform.position), projectileStartPos.transform.position, "Enemy");
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
        if (PauseScript.Instance.isPaused)
            return;
        if (!coolDown)
            return;
        fireTimer += Time.deltaTime;
        if (fireTimer >= attackRate)
        {
            fireTimer = 0;
            coolDown = false;
        }
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
    }
}
