using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FourthCargerChaseTrigger : MonoBehaviour
{
    [SerializeField]
    Animator Animator;

    [SerializeField]
    GameObject Bridge;

    GameObject Player;
    PlayerStats PlayerStatsScript;

    Bridge BridgeBridgeScript;
    // Start is called before the first frame update
    void Start()
    {
        BridgeBridgeScript = Bridge.GetComponent<Bridge>();

        Player = GameObject.FindGameObjectWithTag("Player");
        PlayerStatsScript = Player.GetComponent<PlayerStats>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("FourthCharger"))
        {
            Animator.SetTrigger("ReachedPit");
            if (BridgeBridgeScript.amIUp)
            {
                Animator.SetBool("IsBridgeUp", false);
                PlayerStatsScript.TakeDamage(6);
            }
            else
            {
                Animator.SetBool("IsBridgeUp", true);
            }
        }
    }
}
