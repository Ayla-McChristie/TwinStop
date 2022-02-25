using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootableButton : MonoBehaviour
{
    [SerializeField]
    GameObject MyBridge;
    Bridge MyBridgeScript;

    //bool that sets whether the button can be shot multiple times
    [SerializeField]
    bool OnlyPressableOnce;
    // Start is called before the first frame update
    void Start()
    {
        MyBridgeScript = MyBridge.GetComponent<Bridge>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("PlayerBullet"))
        {
            
        }
    }
}
