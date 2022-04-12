using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsPlayerOnBridgeCheck : MonoBehaviour
{
    public bool IsPlayerOnBridge;

    // Start is called before the first frame update
    void Start()
    {
        IsPlayerOnBridge = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            IsPlayerOnBridge = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            IsPlayerOnBridge = false;
        }
    }

}
