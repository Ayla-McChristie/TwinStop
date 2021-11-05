using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeBehavior : MonoBehaviour
{
    // How many seconds the traps go up - Steven
    [SerializeField] private float SpikeTrapTime = 2000;

    void Start() {
        Debug.Log("Im working");
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(CheckTimeTrap());
    }

    IEnumerator CheckTimeTrap()
    {
        while(true)
        {
            yield return new WaitForSeconds(SpikeTrapTime); // Fires next line after SpikeTrapTime - Steven
            MoveSpike(); // Runs function that plays animation
            Debug.Log("Spike has moved");
        }  
    }


    void MoveSpike()
    {
        transform.GetComponent<Animator>().Play("MoveSpike");
    }
}
