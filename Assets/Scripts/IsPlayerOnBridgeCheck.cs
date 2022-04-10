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
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            IsPlayerOnBridge = true;
            Physics.IgnoreCollision(collision.gameObject.GetComponent<Collider>(), GetComponent<Collider>());
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            IsPlayerOnBridge = false;
        }
    }
}
