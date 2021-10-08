using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    #region Serialized Variables
    [SerializeField]
    [Range(1f, 3f)]
    float b_Speed; //Bullet's Speed
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
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += direction * b_Speed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (projectileUser == "Player")
        {
            if (collision.transform.tag == "Enemy")
            {
                opP.DeactivateProjectile(this.gameObject);
            }
        }
        else if (projectileUser == "Enemy")
        {
            if (collision.transform.tag == "Player")
            {
                opP.DeactivateProjectile(this.gameObject);
            }
        }
        if (collision.transform.tag == "Obstacle")
        {
            opP.DeactivateProjectile(this.gameObject);
        }
    }
}
