using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayDuringCombat : MonoBehaviour
{
    ParticleSystem[] ps;
    [SerializeField]
    bool playDuringCombat;
    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponentsInChildren<ParticleSystem>();
        foreach (var item in ps)
        {
            item.Stop();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playDuringCombat)
        {
            DuringCombat();
        }
        else
        {
            OutsideCombat();
        }
    }

    void DuringCombat()
    {
        foreach (var item in ps)
        {
            if (EnemyManager.Instance.isInCombat && !item.isPlaying)
            {
                item.Play();
            }
            if (RoomManager.Instance.CurrentRoom != null)
            {
                if (!EnemyManager.Instance.isInCombat && RoomManager.Instance.CurrentRoom.NoMoreWaves)
                {
                    item.Stop();
                }
            }
        }
    }

    void OutsideCombat()
    {
        foreach (var item in ps)
        {
            if (!EnemyManager.Instance.isInCombat && !item.isPlaying && !(this.transform.parent.gameObject.GetComponent<Door>().IsLocked))
            {
                item.Play();
            }
            if (RoomManager.Instance.CurrentRoom != null)
            {
                if (EnemyManager.Instance.isInCombat && !RoomManager.Instance.CurrentRoom.NoMoreWaves)
                {
                    item.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                }
            }
        }
    }
}
