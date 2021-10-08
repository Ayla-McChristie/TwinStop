using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomEnterCollider : MonoBehaviour
{
    public GameObject go;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            go.GetComponent<EnemyManager>().SpawnEnemies(0);
        }
    }
}
