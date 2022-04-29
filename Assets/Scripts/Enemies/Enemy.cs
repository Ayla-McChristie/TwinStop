using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;
[RequireComponent(typeof(Rigidbody))]
public class Enemy : MonoBehaviour, IDamageFlash
{

    /* 
     * Attributes 
     */
     //Player's State Script To Get TakeDamage()
    PlayerStats pStats;
    //movement
    protected Rigidbody rigidbody;
    protected float Speed;
    Vector3 acceleration;
    Vector3 velocity;
    protected AudioSource deathSound;
    AudioClip deathClip;
    protected float clipTimer;
    bool audioPlayed;
    //stationary is used for enemy types who like to keep their distance
    bool stationary;

    //combat
    //this will be used to get the players transform for first playable
    protected GameObject target;
    public float Health { get; set; }
    protected float Damage;
    bool attackCooldown = false;
    protected NavMeshAgent agent;
    public bool isDead = false;
    public LayerMask mask;
    public bool hasChildrenRender;
    protected float fovDist = 200.0f;

    protected Animator MyAnimator;
    CharacterController con;
    /*
     * HitFlash Variables
     */
    Renderer[] FlashRenderers;
    [SerializeField]
    public Renderer FlashRenderer { get; set; }
    public Material hurtMat;
    public Material HurtMat { get => hurtMat; }
    protected Material defaultMat;
    public float flashDuration;
    public float FlashDuration
    {
        get => flashDuration;
        set => flashDuration = value;
    }
    public float FlashTimer { get; set; }

    [SerializeField]
    GameObject deathExplosion;

    /*
     * Methods
     */
    // Start is called before the first frame update
    public virtual void Start()
    {
        pStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player");
        }
        rigidbody = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
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

        MyAnimator = GetComponent<Animator>();
        //deathExplosion = Resources.Load(@"Assets\PrefabsVFX\vfx_Explosion_Big.prefab") as GameObject;
    }
    public virtual void FixedUpdate()
    {
        if (PauseScript.Instance.isPaused)
        {
            if (agent == null)
                return;
            agent.velocity = Vector3.zero;
            agent.isStopped = true;
            return;
        }
        if (this.Health <= 0 && this.isDead == false)
        {
            this.Die();
            this.isDead = true;
            if(MyAnimator == null)
                this.gameObject.SetActive(false);
            this.gameObject.GetComponent<Collider>().enabled = false;
        }
        FlashCoolDown();
        DeathSoundClipTime();
    }

    public void AddForce(Vector3 force)
    {
        this.acceleration += force;
    }
    /*
     * Adds force so the enemy walks towards their target
     */
    public void Seek(Vector3 target)
    {
        Vector3 desired = target - this.gameObject.transform.position;

        Vector3 steer = desired - velocity;

        AddForce(steer);
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
                {
                    if (r.transform.name == "Vfx_BubbleShield")
                        return;
                    r.material = HurtMat;
                }
            }
            else if (FlashTimer <= 0 && FlashRenderer.material != defaultMat)
            {
                foreach (Renderer r in FlashRenderers)
                {
                    if (r.transform.name == "Vfx_BubbleShield")
                        return;
                    r.material = defaultMat;
                }
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
    public void TakeDamage()
    {
        TakeDamage(1);
    }
    public void TakeDamage(float damageAmount)
    {
        Health -= damageAmount;
        FlashTimer = FlashDuration;
    }

    protected virtual void DeathSoundClipTime()
    {
        if (Health <= 0)
        {
            if (deathSound == null)
                return;
            clipTimer += Time.deltaTime;
            //if (clipTimer >= deathSound.clip.length)
                //this.gameObject.SetActive(false);
        }
    }

    void PlayDeathSound()
    {
        if (!audioPlayed)
        {
            if (deathSound == null)
                return;
            AudioManager.Instance.PlaySound(deathSound.name, this.transform.position, true);
            audioPlayed = true;
        }
    }
    /*
     * When ever death conditions are met we run this to disable the enemy
     */
    void Die()
    {
        //put code for enemy to do damage to player here

        //Plays death anim
        if (MyAnimator != null)
        { 
            if (!MyAnimator.isActiveAndEnabled)
            {
                MyAnimator.enabled = true;
            }
        
            MyAnimator.SetTrigger("ImDead");
        }

        PlayerStats.AddToKillCount();
        PlayDeathSound();
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player" && !isDead)
        {
            pStats.TakeDamage();
        }

        if (collision.transform.tag == "PlayerBullet")
        {
            this.TakeDamage();
        }
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

    protected void DamagePlayer()
    {
        pStats.TakeDamage();
    }

    //Sets this object to inactive
    public void Despawn()
    {
        this.gameObject.SetActive(false);
    }

    //Spawns a death explosion
    public void Explode()
    {
        Instantiate(deathExplosion, this.transform.position + Vector3.up, this.transform.rotation);
    }
}
