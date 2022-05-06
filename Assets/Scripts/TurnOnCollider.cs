using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOnCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "FallingObjects")
        {
            Collider boxCollider = other.gameObject.GetComponent<Collider>();
            boxCollider.enabled = true;
        }
    }
}
