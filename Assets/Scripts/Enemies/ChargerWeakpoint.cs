using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargerWeakpoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.transform.tag == "PlayerBullet")
            this.gameObject.GetComponentInParent<Charger>().DamageTaken(1);
    }
}
