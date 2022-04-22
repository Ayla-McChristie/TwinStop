using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniBossScript : MonoBehaviour
{
    Sentinel sentinel = new Sentinel();

    [SerializeField] GameObject roomTrigger;
    // Update is called once per frame
    void Update()
    {
        if(this.sentinel.health <= 100) 
        {

        }
    }
}
