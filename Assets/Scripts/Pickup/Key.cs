using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    enum IKeyType {RoomKey, BossKey}

    [SerializeField]
    IKeyType keyType;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            PlayerStats ps = other.GetComponent<PlayerStats>();

            ps.keys++;

            /*
             * technical debt. this should use the object pool system but this will work for now 
             */
            Destroy(this.gameObject);
        }
    }
}
