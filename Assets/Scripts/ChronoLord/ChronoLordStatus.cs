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

    public void AmVulnerable()
    {
        Vulnerable = true;
        MyAnimator.SetBool("Vulnerable", true);
    }

    public void NotVulnerable()
    {
        Vulnerable = false;
        MyAnimator.SetBool("Vulnerable", false);
    }

    public void AmFiring()
    {
        Firing = true;
        MyAnimator.SetBool("Firing", true);
    }

    public void NotFiring()
    {
        Firing = false;
        MyAnimator.SetBool("Firing", false);
    }

    public void Die()
    {
        MyAnimator.SetTrigger("IDied");
    }
}
