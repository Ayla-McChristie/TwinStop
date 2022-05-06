using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetObjectActive : MonoBehaviour
{
 // This code is to be used for the tutorial level to active the time mechanic - Steven

    public GameObject[] ActivateGameObjects;
    public GameObject[] DeactivateGameObjects;
    public GameObject[] TurnOnGravity;

    TimeManager timeManagerScript;

    private void Start()
    {
        foreach (GameObject item in ActivateGameObjects)
        {
                item.SetActive(false);
        }
    }
    // Set the number of game obects you want activated here
    // then drag their game object into the field - Steven

    private void OnCollisionEnter(Collision other) 
    {
        if (other.transform.tag == "Player")
        {
            foreach (GameObject gb in DeactivateGameObjects)
            {
                gb.SetActive(false);
            }
            //Scans through every object in ActivateGameObjects and makes them active
            foreach(GameObject gb in ActivateGameObjects) 
            {
                gb.SetActive(true);
            }

            foreach (GameObject item in TurnOnGravity)
            {
                item.GetComponent<Rigidbody>().useGravity = true;
            }

            //easiest way to find the time manager script ig lol -Ryan
            timeManagerScript = GameObject.FindGameObjectWithTag("TimeManager").GetComponent<TimeManager>();
            timeManagerScript.hasTimeCrystal = true;

            Destroy(this.gameObject); // destory current object
        }    
    }

    void SetAObjectActive(GameObject gameObject)
    {
        gameObject.SetActive(true);
    }
}
