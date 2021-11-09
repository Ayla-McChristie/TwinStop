using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetObjectActive : MonoBehaviour
{
 // This code is to be used for the tutorial level to active the time mechanic - Steven

    public GameObject[] ActivateGameObjects;
    // Set the number of game obects you want activated here
    // then drag their game object into the field - Steven

    private void OnCollisionEnter(Collision other) 
    {
        if (other.transform.tag == "Player")
        {
            //Scans through every object in ActivateGameObjects and makes them active
            foreach(GameObject gb in ActivateGameObjects) 
            {
                gb.SetActive(true);
            }
            Destroy(this.gameObject); // destory current object
        }    
    }

    void SetAObjectActive(GameObject gameObject)
    {
        gameObject.SetActive(true);
    }
}
