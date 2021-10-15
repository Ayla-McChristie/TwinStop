using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    #region Serialized Variables
    [SerializeField]
    [Range(5f, 10f)]
    float b_Speed = 10; //Bullet's Speed
    #endregion
    Vector3 direction;
    string projectileUser;
    ObjectPool_Projectiles opP;

    void Start()
    {
        opP = new ObjectPool_Projectiles();
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
        transform.position += direction * b_Speed * Time.deltaTime;
        IgnoreProjectiles();
    }

    void IgnoreProjectiles()
    {
        if (GameObject.FindWithTag("PlayerBullet"))
            Physics.IgnoreCollision(GameObject.FindWithTag("PlayerBullet").GetComponent<Collider>(), this.gameObject.GetComponent<Collider>(), true);
        if (GameObject.FindWithTag("EnemyBullet"))
            Physics.IgnoreCollision(GameObject.FindWithTag("EnemyBullet").GetComponent<Collider>(), this.gameObject.GetComponent<Collider>(), true);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (projectileUser == "Player")
        {
            if (other.transform.tag == "Enemy")
            {
                /*
                 * technical debt. make enemies use object pool
                 */
                Destroy(other.gameObject);
            }
        }
        else if (projectileUser == "Enemy")
        {
            if (other.transform.tag == "Player")
            {
                //Deal damage script here
            }
        }
        opP.DeactivateProjectile(this.gameObject);
    }
}
