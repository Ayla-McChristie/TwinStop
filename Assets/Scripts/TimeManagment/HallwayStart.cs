using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HallwayStart : MonoBehaviour
{
    public TimeManager timeManager;

    void Update()
    {
        timeManager.outtaTime = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Player")
        {
            timeManager.outtaTime = false;
            timeManager.hasTimeCrystal = true;
            timeManager.isTimeStopped = true;
            Debug.Log("I am working");
        }
    }
}
