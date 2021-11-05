using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayDuringCombat : MonoBehaviour
{
    ParticleSystem ps;
    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponentInChildren<ParticleSystem>();
        ps.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        if (EnemyManager.Instance.isInCombat && !ps.isPlaying)
        {
            ps.Play();
        }
        else if(!EnemyManager.Instance.isInCombat)
        {
            ps.Stop();
        }
    }
}
