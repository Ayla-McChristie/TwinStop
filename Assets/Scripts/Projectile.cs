using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    #region Serialized Variables
    [SerializeField]
    [Range(1f, 3f)]
    float b_Speed; //Bullet's Speed

    [SerializeField]
    float bd_Speed; //Rate The Bullet Drops
                    // Start is called before the first frame update
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
        transform.position += direction * b_Speed * Time.deltaTime + Vector3.down * bd_Speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (projectileUser == "Player")
        {
            if (other.gameObject.tag == "Enemy")
            {
                opP.DeactivateProjectile(this.gameObject);
            }
        }
        else if (projectileUser == "Enemy")
        {
            if (other.gameObject.tag == "Player")
            {
                opP.DeactivateProjectile(this.gameObject);
            }
        }
        if (other.gameObject.tag == "Obstacle")
        {
            opP.DeactivateProjectile(this.gameObject);
        }
    }
}
