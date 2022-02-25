using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : MonoBehaviour
{
    Animator Animator;
    public bool amIUp;

    // Start is called before the first frame update
    void Start()
    {
        this.Animator = this.GetComponent<Animator>();
        amIUp = false;
    }

    public void PutBridgeDown()
    {
        amIUp = false;
        Animator.SetBool("IsUp", false);
    }

    public void PutBridgeUp()
    {
        amIUp = true;
        Animator.SetBool("IsUp", true);
    }
}
