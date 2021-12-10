using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(Rigidbody))]
public class Enemy : MonoBehaviour, IDamageFlash
{

    /* 
     * Attributes 
     */

    //movement
    protected Rigidbody rigidbody;
    protected float Speed;
    Vector3 acceleration;
    Vector3 velocity;
    protected AudioSource deathSound;
    AudioClip deathClip;
    float clipTimer;
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
    protected bool isDead = false;
    public LayerMask mask;
    protected float fovDist = 100.0f;
    /*
     * HitFlash Variables
     */
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

    /*
     * Methods
     */
    // Start is called before the first frame update
    public virtual void Start()
    {
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player");
        }
        rigidbody = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        FlashRenderer = GetComponent<Renderer>();
        defaultMat = FlashRenderer.material;
        deathSound = GetComponent<AudioSource>();
        deathClip = deathSound.clip;
        clipTimer = 0;
    }
    public virtual void FixedUpdate()
    {
        if (this.Health <= 0)
        {
            this.Die();
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
        if (this.Health <= 0)
        {
            Die();
            isDead = true;
        }
    }

    protected void DeathSoundClipTime()
    {
        if (Health <= 0)
        {
            clipTimer += Time.deltaTime;
            if (clipTimer >= deathClip.length)
                this.gameObject.SetActive(false);
        }
    }

    void PlayDeathSound()
    {
        if (!audioPlayed)
        {
            deathSound.Play();
            audioPlayed = true;
        }
    }
    /*
     * When ever death conditions are met we run this to disable the enemy
     */
    void Die()
    {
        //put code for enemy to do damage to player here
        PlayerStats.AddToKillCount();
        PlayDeathSound();

    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player")
        {
            /*
             * TODO add enemy attack 
             */

            //this.TakeDamage();
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
}
