using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : MonoBehaviour
{
    Animator Animator;
    public bool amIUp;

    [SerializeField]
    bool IsPlayerOnBridge;

    [SerializeField]
    IsPlayerOnBridgeCheck bridgeCheckScript;

    // Start is called before the first frame update
    void Start()
    {
        this.Animator = this.GetComponent<Animator>();
        amIUp = false;
        IsPlayerOnBridge = false;
    }

    private void Update()
    {
        IsPlayerOnBridge = bridgeCheckScript.IsPlayerOnBridge;
    }

    public void PutBridgeDown()
    {
        if (!IsPlayerOnBridge)
        {
            amIUp = false;
            Animator.SetBool("IsUp", false);
        }
    }

    public void PutBridgeUp()
    {
        if (!IsPlayerOnBridge)
        {
            amIUp = true;
            Animator.SetBool("IsUp", true);
        }
    }

    
}
