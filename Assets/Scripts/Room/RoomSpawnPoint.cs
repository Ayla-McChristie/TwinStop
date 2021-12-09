using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawnPoint : MonoBehaviour
{

    [SerializeField]
    public List<WaveListWrapper> listOfWaves;

    //this is because unity doesnt serialized nested lists without it
    [System.Serializable]

    public class WaveListWrapper
    {
        public List<GameObject> WaveList;
    }

    GameObject spawnParticles;
    private void Start()
    {
        //List<WaveListWrapper> listOfWaves = new List<WaveListWrapper>();
        foreach (ParticleSystem item in GetComponentsInChildren<ParticleSystem>())
        {
            item.Stop();
        }
    }

    public void PlaySpawnParticles()
    {
        foreach (ParticleSystem item in GetComponentsInChildren<ParticleSystem>())
        {
            ParticleSystem.EmissionModule em = item.emission;
            if (!item.isPlaying)
            {
                item.Play();
            }
        }
    }

    private void Update()
    {
        foreach (ParticleSystem item in GetComponentsInChildren<ParticleSystem>())
        {
            if(item.time > item.main.duration)
            {
                item.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            }
        }
    }
}
