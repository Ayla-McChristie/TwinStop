using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instadeath : MonoBehaviour
{
    GameObject player;
    PlayerStats PlayerStatsScript;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        PlayerStatsScript = player.GetComponent<PlayerStats>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerStatsScript.Instakill();
        }
    }
}
