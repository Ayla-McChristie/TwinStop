using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChronoLordAttackPattern : MonoBehaviour
{
    Animator MyAnimator;

    [SerializeField]
    GameObject[] Chargers, TestEnemies, Turtles;

    GameObject[] waveToCheck;

    [SerializeField]
    public float attackRate = 1;

    bool IsVulnerable, HasIncremented;

    [SerializeField]
    bool coolDown;

    int waveIndex;

    float fireTimer = 0;

    string projectileType;

    float projectileSpeed = 12;

    string BoolToSet;

    [SerializeField]
    float Health;

    [SerializeField]
    Slider CLHealthBar;

    [SerializeField]
    GameObject BubbleShield, projectileStartPos;

    GameObject Player, projectile;

    public Renderer FlashRenderer { get; set; }
    public Material hurtMat;
    public Material HurtMat { get => hurtMat; }
    protected Material defaultMat;
    public float flashDuration;

    [SerializeField]
    ChronoLordStatus MyChronoLordStatus;
    public float FlashDuration
    {
        get => flashDuration;
        set => flashDuration = value;
    }
    public float FlashTimer { get; set; }
    Renderer[] FlashRenderers;

    public bool hasChildrenRender;
    public LayerMask mask;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        HasIncremented = false;
        MyAnimator = GetComponent<Animator>();
        IsVulnerable = false;
        BubbleShield.SetActive(true);
        //projectileStartPos.SetActive(false);
        waveIndex = 1;
        waveToCheck = Chargers;

        FlashTimer = 0;

        FlashRenderer = GetComponent<Renderer>();
        if (hasChildrenRender)
        {
            FlashRenderers = this.transform.GetComponentsInChildren<Renderer>();
        }
        if (FlashRenderer == null || FlashRenderer.enabled == false)
        {
            FlashRenderer = this.GetComponentInChildren<Renderer>();
        }
        defaultMat = FlashRenderer.material;

        MyChronoLordStatus.AmFiring();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsWaveFinished(waveToCheck) && !IsVulnerable && !HasIncremented)
        {
            MakeVulnerable();
            MyAnimator.SetBool(BoolToSet, true);
        }
        //Debug.Log("baba"+FlashTimer);
    }

    private void FixedUpdate()
    {
        FlashCoolDown();
    }

    void FlashCoolDown()
    {
        FlashTimer -= Time.deltaTime;
        float lerp = Mathf.Clamp01(FlashTimer / FlashDuration);

        if (hasChildrenRender)
        {
            if (FlashTimer >= 0 && FlashRenderer.material != hurtMat)
            {
                foreach (Renderer r in FlashRenderers)
                    r.material = HurtMat;
            }
            else if (FlashTimer <= 0 && FlashRenderer.material != defaultMat)
            {
                foreach (Renderer r in FlashRenderers)
                    r.material = defaultMat;
            }
            return;
        }

        if (FlashTimer >= 0 && FlashRenderer.material != hurtMat)
        {
            FlashRenderer.material = HurtMat;
        }
        else if (FlashTimer <= 0 && FlashRenderer.material != defaultMat)
        {
            FlashRenderer.material = defaultMat;
        }
    }

    public bool IsWaveFinished(GameObject[] enemiesInWave)
    {
        foreach (GameObject enemy in enemiesInWave)
        {
            if (enemy.activeInHierarchy)
            {
                return false;
            }
        }
        return true;
    }

    void MakeInvulnerable()
    {
        HasIncremented = false;
        MyAnimator.SetBool("Vulnerable", false);
        IsVulnerable = false;
        BubbleShield.SetActive(true);
        projectileStartPos.SetActive(false);
        MyChronoLordStatus.NotVulnerable();
        MyChronoLordStatus.AmFiring();
    }

    void MakeVulnerable()
    {
        IncrementWave();
        MyAnimator.SetBool("Vulnerable", true);
        IsVulnerable = true;
        BubbleShield.SetActive(false);
        projectileStartPos.SetActive(true);
        MyChronoLordStatus.AmVulnerable();
        MyChronoLordStatus.NotFiring();
    }

    void IncrementWave()
    {
        if (waveIndex == 1)
        {
            BoolToSet = "ChargerWaveEnd";
            waveToCheck = TestEnemies;
        }
        else if (waveIndex > 1)
        {
            waveToCheck = Turtles;
            BoolToSet = "TestEnemyWaveEnd";
        }
        waveIndex++;
        HasIncremented = true;
        Debug.Log("wave index" + waveIndex);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "PlayerBullet" && IsVulnerable)
        {
            TakeDamage();
        }
    }

    void TakeDamage()
    {
        Health--;
        CLHealthBar.value--;
        FlashTimer = FlashDuration;

        if (CheckAmIDead())
        {
            Die();
        }
    }

    bool CheckAmIDead()
    {
        if (Health <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void Die()
    {
        MyAnimator.SetBool("ImDead", true);
    }

    void Despawn()
    {
        this.gameObject.SetActive(false);
    }

    // --Taken from Sentry gun script--
    //void AttackTarget()
    //{
    //    if (!coolDown && !Player.GetComponent<PlayerStats>().isDead)
    //    {
    //        Debug.Log("Fired Fired Fired");
    //        projectile = ObjectPool_Projectiles.Instance.GetProjectile(projectileType); //Getting the projectile gameobject
    //        projectile.GetComponent<Projectile>().b_Speed = projectileSpeed;
    //        if (GetPredictedDir(new Vector3(Player.transform.position.x, this.transform.position.y, Player.transform.position.z), this.transform.position, Player.GetComponent<Rigidbody>().velocity, projectile.GetComponent<Projectile>().b_Speed, out var direction))
    //            projectile.GetComponent<Projectile>().SetUp(direction, projectileStartPos.transform.position, "Enemy");
    //        else
    //            projectile.GetComponent<Projectile>().SetUp((Player.transform.position - this.transform.position), projectileStartPos.transform.position, "Enemy");
    //        coolDown = true;
    //    }
    //}

    //bool GetPredictedDir(Vector3 targetPos, Vector3 shooterPos, Vector3 targetV, float b_Speed, out Vector3 result)
    //{
    //    var targetToShooter = targetPos - shooterPos;
    //    var distance = targetToShooter.magnitude;
    //    var angle = Vector3.Angle(targetPos, targetV) * Mathf.Deg2Rad;

    //    var speedTar = targetV.magnitude;
    //    var r = speedTar / b_Speed;
    //    if (SolveQuadratic(1 - r * r, 2 * r * distance * Mathf.Cos(angle), -(distance * distance), out var root1, out var root2) == 0)
    //    {
    //        result = Vector3.zero;
    //        return false;
    //    }
    //    var projectedDist = Mathf.Max(root1, root2);
    //    var t = projectedDist / b_Speed;
    //    var c = targetPos + targetV * t;
    //    result = (c - shooterPos).normalized;
    //    return true;
    //}

    //int SolveQuadratic(float a, float b, float c, out float root1, out float root2)
    //{
    //    var discriminant = b * b - 4 * a * c;
    //    if (discriminant < 0)
    //    {
    //        root1 = Mathf.Infinity;
    //        root2 = -root1;
    //        return 0;
    //    }

    //    root1 = (-b + Mathf.Sqrt(discriminant)) / (2 * a);
    //    root2 = (-b - Mathf.Sqrt(discriminant)) / (2 * a);

    //    return discriminant > 0 ? 2 : 1;
    //}

    //void AttackCoolDown() //timer for when the enemy can shoot again
    //{
    //    if (PauseScript.Instance.isPaused)
    //        return;
    //    if (!coolDown)
    //        return;
    //    fireTimer += Time.deltaTime;
    //    if (fireTimer >= attackRate)
    //    {
    //        fireTimer = 0;
    //        coolDown = false;
    //    }
    //}
    //----

}
