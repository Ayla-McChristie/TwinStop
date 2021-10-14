using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class KeyDoor : Door
{
    // Start is called before the first frame update
    void Start()
    {
        this.IsLocked = true;
        this.IsOpen = false;
    }

    private void OnCollisionEnter(Collision other)
    {
        /*
         * checks to see if the player has a key and unlocks the door if they do.
         */
        if (other.transform.tag == "Player")
        {
            throw new NotImplementedException();

            //if (other.playerStats.keys > 1)
            //{
            //    other.playerStats.keys--;
            //    this.IsLocked = false;
            //}
        }
    }
}
