using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SentryTrigger : MonoBehaviour
{
    public bool isTriggered;
    Vector3 pos;
    Quaternion rot;
    private void Start()
    {
        pos = this.transform.position;
        rot = this.transform.rotation;
    }
    private void Update()
    {
        this.transform.position = pos;
        this.transform.rotation = rot;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform.tag == "Player")
        {
            isTriggered = true;
        }

    }
}
