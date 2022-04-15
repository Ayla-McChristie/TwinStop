using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChronoLordStatus : MonoBehaviour
{
    bool Vulnerable, Firing, Idle;
    Animator MyAnimator;
    // Start is called before the first frame update
    void Start()
    {
        Vulnerable = Firing = false;
        Idle = true;
        MyAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void AmVulnerable()
    {
        Vulnerable = true;
        MyAnimator.SetBool("Vulnerable", true);
    }

    void NotVulnerable()
    {
        Vulnerable = false;
        MyAnimator.SetBool("Vulnerable", false);
    }

    void AmIdle()
    {
        Vulnerable = true;
        MyAnimator.SetBool("Idle", true);
    }

    void NotIdle()
    {
        Idle = false;
        MyAnimator.SetBool("Idle", false);
    }

    void AmFiring()
    {
        Vulnerable = true;
        MyAnimator.SetBool("Firing", true);
    }

    void NotFiring()
    {
        Firing = false;
        MyAnimator.SetBool("Firing", false);
    }
}
