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

    float CooldownInt, TimerIncrement;
    // Start is called before the first frame update
    void Start()
    {
        MyBridgeScript = MyBridge.GetComponent<Bridge>();
        TimerIncrement = .1f;
        CooldownInt = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (CooldownInt > 0)
        {
            Timer();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("PlayerBullet"))
        {
            if (CooldownInt <= 0)
            {
                if (MyBridgeScript.amIUp)
                {
                    Debug.Log("put down");
                    MyBridgeScript.PutBridgeDown();
                }
                else
                {
                    Debug.Log("put up");
                    MyBridgeScript.PutBridgeUp();
                }
                CooldownInt = 10;
            }
        }
    }

    /// <summary>
    /// Activated after shooting the button. Increments the CooldownInt var
    /// </summary>
    void Timer()
    {
        CooldownInt -= TimerIncrement;
    }
}
