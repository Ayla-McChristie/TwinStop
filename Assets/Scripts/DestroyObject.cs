using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{

    public ParticleSystem ps; // Was trying to be cool and have the enemy spawn particle effect to play but it wont work
    public GameObject GameObject;

    void Start()
    {
        //ps = GetComponentInChildren<ParticleSystem>();
        //ps.Stop();
    }

   void OnCollisionEnter(Collision other) 
   {
       if (other.transform.tag == "Player")
        {
            Debug.Log("I detect a player");
            ps.Play();
            Destroy(GameObject);
            ps.Stop();
        }   

        Destroy(this.gameObject);
   }
   
}
