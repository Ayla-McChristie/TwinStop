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

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BridgeUpOrDown(bool upOrDown)
    {
        Animator.SetBool("IsUp", upOrDown);
    }
}
