using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Rotate))]
public class SpinningWallTrap : MonoBehaviour
{
    CapsuleCollider capsule;
    // Start is called before the first frame update
    void Start()
    {
        capsule = this.GetComponent<CapsuleCollider>();
        //AudioManager.Instance.PlaySound(GetComponent<AudioSource>().name, this.transform.position, false);
    }

    // Update is called once per frame
    void Update()
    {
        if (TimeManager.Instance != null)
        {
            Debug.Log(TimeManager.Instance.isTimeStopped.ToString());
            if (TimeManager.Instance.isTimeStopped)
            {
                capsule.enabled = false;
            }
            else
            {
                capsule.enabled = true;
            }
        }
    }
}
