using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(Rigidbody))]
public class Enemy : MonoBehaviour
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
    protected NavMeshAgent agent;

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
    }

    public virtual void FixedUpdate()
    {
        velocity += acceleration;
        velocity.Normalize();
        velocity *= this.Speed;

        if (!stationary)
        {
            rigidbody.velocity = velocity;
        }

        acceleration *= 0;

        this.transform.LookAt(target.transform);
    }

    protected void TakeDamage(float damageAmmount)
    {
        this.Health -= damageAmmount;
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
    /*
     * When ever death conditions are met we run this to disable the enemy
     */
    void Die()
    {
        /*
         * Should replace this with object pooling instead of destruction
         */
        this.gameObject.SetActive(false);
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player")
        {
            /*
             * TODO add enemy attack 
             */

            //put code for enemy to do damage to player here
            collision.gameObject.GetComponent<PlayerStats>().health--;
            this.Die();
        }

        if (collision.transform.tag == "PlayerBullet")
        {
            this.Die();
        }
    }
}
