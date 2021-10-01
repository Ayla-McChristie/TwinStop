using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    bool stationary;

    //combat
    //this will be used to get the players transform for first playable
    protected GameObject target;
    public float Health { get; set; }
    protected float Damage;



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
    }

    // Update is called once per frame
    public virtual void Update()
    {
        velocity += acceleration;
        velocity.Normalize();
        velocity *= this.Speed * Time.deltaTime;

        if (!stationary)
        {
            rigidbody.velocity = velocity;
        }

        acceleration *= 0;

        this.transform.LookAt(target.transform);
    }

    void TakeDamage(float damageAmmount)
    {
        this.Health -= damageAmmount;
    }

    public void AddForce(Vector3 force)
    {
        this.acceleration += force;
    }
    public void Seek(Vector3 target)
    {
        Vector3 desired = target - this.gameObject.transform.position;
        desired.Normalize();
        desired *= Speed * Time.deltaTime;

        Vector3 steer = desired - velocity;

        AddForce(desired);
    }
    void Die()
    {
        this.enabled = false;
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player")
        {
            this.Die();
        }
    }
}
