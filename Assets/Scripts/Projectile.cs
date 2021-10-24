using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    #region Serialized Variables
    [SerializeField]
    [Range(5f, 30f)]
    float b_Speed = 10; //Bullet's Speed
    #endregion
    Vector3 direction;
    string projectileUser;
    Rigidbody rb;

    ObjectPool_Projectiles opP;

    void Start()
    {
        opP = new ObjectPool_Projectiles();
        rb = GetComponent<Rigidbody>();
    }

    public void SetUp(Vector3 direction, Vector3 position, string projectileUser)
    {
        this.direction = direction;
        this.transform.position = position;
        this.projectileUser = projectileUser;
        transform.forward = direction;
        IgnoreCollision(projectileUser);
    }

    void IgnoreCollision(string user)
    {
        if (user == "Player")
            Physics.IgnoreCollision(GameObject.FindWithTag("Player").GetComponent<Collider>(), this.gameObject.GetComponent<Collider>(), true);
        else if (user == "Enemy")
            Physics.IgnoreCollision(GameObject.FindWithTag("Enemy").GetComponent<Collider>(), this.gameObject.GetComponent<Collider>(), true);
    }

    // Update is called once per frame
    void Update()
    {
        if (projectileUser == "Player")
        {
            //use unscaled delta time if unaffected by time stop
            //use delta time if affected by time stop
            rb.velocity = direction * b_Speed;
        }
        else
        {
            transform.position += direction * b_Speed * Time.deltaTime;
        }

        IgnoreProjectiles();
    }

    void IgnoreProjectiles()
    {
        if (GameObject.FindWithTag("PlayerBullet"))
            Physics.IgnoreCollision(GameObject.FindWithTag("PlayerBullet").GetComponent<Collider>(), this.gameObject.GetComponent<Collider>(), true);
        if (GameObject.FindWithTag("EnemyBullet"))
            Physics.IgnoreCollision(GameObject.FindWithTag("EnemyBullet").GetComponent<Collider>(), this.gameObject.GetComponent<Collider>(), true);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //player bullet vs enemy
        if (projectileUser == "Player" && collision.transform.tag == "Enemy")
        {
            Destroy(collision.gameObject);
            PlayerStats.AddToKillCount();
        }

        //enemy bullet vs player
        if (projectileUser == "Enemy" && collision.transform.tag == "Player")
        {
            //deal damage to player
        }
        opP.DeactivateProjectile(this.gameObject);
    }
}
