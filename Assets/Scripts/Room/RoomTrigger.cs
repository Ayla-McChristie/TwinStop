using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    public class RoomDetail
    {
        public int roomNum;
        public List<GameObject> listOfEnemies;
        public Transform[] SpawnLoc;
    }

    [SerializeField]
    List<GameObject> spawnPoints;
    //enemy manager should make itself

    int waveNum;
    int totalWaves;
    bool hasStarted = false;

    private bool noMoreWaves;
    public static bool NoMoreWaves { get; private set; }

    private void Start()
    {
        totalWaves = GetTotalWaves();
        Debug.Log($"total waves is {totalWaves}");
    }
    private void Update()
    {
        if (waveNum >= totalWaves)
        {
            noMoreWaves = true;
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
            Debug.Log("player has entered a room");
            PlayerStats.ResetKillCount();

            hasStarted = true;
            NoMoreWaves = false;
            /*
            * Start wave spawn 
            */
        }
    }

    void SpawnNextWave()
    {
        foreach (var item in spawnPoints)
        {
            Debug.Log("Spawning enemies");
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
