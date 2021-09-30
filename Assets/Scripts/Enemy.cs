using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    /* 
     * Attributes 
     */

    //movement
    protected float Speed;
    Vector3 acceleration;
    Vector3 velocity;

    bool stationary;

    //combat
    //this will be used to get the players transform for first playable
    protected GameObject targetPlayer;
    public float Health { get; set; }
    protected float Damage;


    /*
     * Methods
     */

    public Enemy()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public virtual void Update()
    {
        velocity += acceleration;
        velocity.Normalize();
        velocity *= this.Speed * Time.deltaTime;

        if (!stationary)
        {
            this.transform.position += velocity;
        }

        acceleration *= 0;

        /*
        * TODO Combine with player system to make it so enemies look at the player
        */
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
}
