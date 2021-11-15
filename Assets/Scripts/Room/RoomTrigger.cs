using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    [SerializeField]
    List<GameObject> spawnPoints;
    //enemy manager should make itself

    int waveNum;
    int totalWaves;
    bool hasStarted = false;
    float longestDelay = 0;

    AudioSource audio;

    public bool NoMoreWaves { get; private set; }

    private void Awake()
    {
        if (RoomManager.Instance == null)
        {
            RoomManager.CreateRoomManager();
        }
    }

    private void Start()
    {
        NoMoreWaves = false;
        totalWaves = GetTotalWaves();
        audio = GetComponent<AudioSource>();
        foreach (var roomSpawn in spawnPoints)
        {
            //roomSpawn.GetComponent<RoomSpawnPoint>().PlaySpawnParticles();
            foreach (var item in roomSpawn.GetComponentsInChildren<ParticleSystem>())
            {
                if (item.startDelay > longestDelay)
                {
                    longestDelay = item.startDelay;
                }
            }
        }
    }
    private void Update()
    {
        if (waveNum >= totalWaves)
        {
            NoMoreWaves = true;
        }
        if (waveNum < totalWaves && EnemyManager.Instance.isInCombat == false && hasStarted) 
        {
            PlayerStats.ResetKillCount();
            SpawnNextWave();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player" && !hasStarted)
        {
            PlayerStats.ResetKillCount();
            audio.Play();
            RoomManager.Instance.SetCurrentRoom(this);

            StartCoroutine("DelaySpawnForParticles", longestDelay);
            
            /*
            * Start wave spawn 
            */
        }
    }

    IEnumerator DelaySpawnForParticles(float longestDelay)
    {
        foreach (var roomSpawn in spawnPoints)
        {
            roomSpawn.GetComponent<RoomSpawnPoint>().PlaySpawnParticles(); 
        }
        yield return new WaitForSeconds(longestDelay);
        hasStarted = true;
        NoMoreWaves = false;

    }

    void SpawnNextWave()
    {
        foreach (var item in spawnPoints)
        {
            var temp = item.GetComponent<RoomSpawnPoint>();
            EnemyManager.Instance.SpawnEnemies(temp.listOfWaves[waveNum].WaveList, item.transform);
        }
        waveNum++;
    }

    int GetTotalWaves()
    {
        int highestWaveCount = 0;
        foreach (var spawnPoint in spawnPoints)
        {
            int waveCount = 0;
            foreach (var wave in spawnPoint.GetComponent<RoomSpawnPoint>().listOfWaves)
            {
                waveCount++;
            }

            if (waveCount > highestWaveCount)
            {
                highestWaveCount = waveCount;
            }
        }
        return highestWaveCount;
    }
}
