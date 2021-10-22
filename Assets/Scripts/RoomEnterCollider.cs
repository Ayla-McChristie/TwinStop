using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomEnterCollider : MonoBehaviour
{
    EnemyManager em;

    private void Start()
    {
        em = GameObject.Find("Enemy Manager").GetComponent<EnemyManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //em.GetComponent<EnemyManager>().SpawnEnemies(0);
        }
    }
}
