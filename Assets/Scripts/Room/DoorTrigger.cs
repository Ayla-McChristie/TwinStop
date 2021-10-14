using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            /*
            * TODO Go to next room
            */
        }
    }
}
