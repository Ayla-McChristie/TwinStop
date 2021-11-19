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
    //stationary is used for enemy types who like to keep their distance
    bool stationary;

    //combat
    //this will be used to get the players transform for first playable
    protected GameObject target;
    public float Health { get; set; }
    protected float Damage;
    bool attackCooldown = false;
    protected NavMeshAgent agent;

    /*
     * HitFlash Variables
     */
    public SkinnedMeshRenderer FlashRenderer { get; set; }
    public MeshRenderer Mesh { get; set; }
    public float flashIntensity;
    public float FlashIntensity
    {
        get => flashIntensity;
        set => flashIntensity = value;
    }
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
        Mesh = GetComponent<MeshRenderer>();
    }
    public virtual void FixedUpdate()
    {
        if (this.Health <= 0)
        {
            this.Die();
        }
        FlashCoolDown();
        this.transform.LookAt(target.transform);
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
        float intesity = (lerp * FlashIntensity) + 1.0f;
        Mesh.material.color = Color.white * intesity;
    }
    public void TakeDamage()
    {
        TakeDamage(1);
    }
    public void TakeDamage(float damageAmount)
    {
        Health -= damageAmount;
        FlashTimer = FlashDuration;
        if (Health <= 0)
        {
            Die();
        }
    }

    /*
     * When ever death conditions are met we run this to disable the enemy
     */
    void Die()
    {
        //put code for enemy to do damage to player here
        PlayerStats.AddToKillCount();
        this.gameObject.SetActive(false);
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
}
