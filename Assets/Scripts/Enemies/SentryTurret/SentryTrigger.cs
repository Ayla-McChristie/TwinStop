using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SentryTrigger : MonoBehaviour
{
    public bool isTriggered;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform.tag == "Player")
            isTriggered = true;
    }
}
