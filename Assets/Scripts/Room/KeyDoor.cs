using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyDoor : Door
{
    // Start is called before the first frame update
    void Start()
    {
        this.IsLocked = true;
        this.IsOpen = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        //checks to see if the player has a key
        if (other.transform.tag == "Player")
        {
            this.IsLocked = false;
        }
    }
}
