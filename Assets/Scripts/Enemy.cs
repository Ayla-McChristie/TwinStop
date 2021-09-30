using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    /* 
     * Attributes 
     */

    //movement
    Vector3 speed;
    Vector3 acceleration;
    Vector3 velocity;
    Vector3 position;

    bool stationary;

    //combat
    //this will be used to get the players transform for first playable
    GameObject targetPlayer;
    public float Health { get; set; }
    public float damage;


    /*
     * Methods
     */

    public Enemy()
    {
        this.position = this.gameObject.transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        velocity += acceleration;
        velocity.Normalize();
        velocity.Scale(this.speed * Time.deltaTime);

        if (!stationary)
        {
            this.position += velocity;
        }

        acceleration *= 0;

        /*
         * TODO Combine with player system to make it so enemies look at the player
         */
        //this.gameObject.transform.LookAt(targetPlayer.transform);


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
        Vector3 desired = this.gameObject.transform.position - target;
        desired.Normalize();
        desired.Scale(speed * Time.deltaTime);

        Vector3 steer = desired - velocity;

        AddForce(desired);
    }
}
