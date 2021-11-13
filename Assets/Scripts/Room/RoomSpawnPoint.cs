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
            item.Play();
        }
    }

    private void Update()
    {
        if (((RoomTrigger)GetComponentInParent<RoomTrigger>()).NoMoreWaves == true)
        {
            foreach (ParticleSystem item in GetComponentsInChildren<ParticleSystem>())
            {
                item.Stop();
            }
        }
    }
}
