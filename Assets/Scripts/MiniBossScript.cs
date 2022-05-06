using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniBossScript : Sentinel
{
    new enum State {Attack, SpecialAttack, Dead }
    [SerializeField] GameObject roomTrigger;
    // Update is called once per frame
    public override void Start()
    {
        base.Start();
    }
    void Update()
    {
    }
}
