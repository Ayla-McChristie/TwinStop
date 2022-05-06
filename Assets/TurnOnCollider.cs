using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOnCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "FallingObjects")
        {
            other.gameObject.GetComponent<BoxCollider>().enabled = true;
            Debug.Log("I worked");
        }
    }
}
