using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SentryTrigger : MonoBehaviour
{
    public bool isTriggered;
    Vector3 pos;
    private void Start()
    {
        pos = this.transform.position;
    }
    private void Update()
    {
        this.transform.position = pos;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform.tag == "Player")
        {
            isTriggered = true;
        }

    }
}
