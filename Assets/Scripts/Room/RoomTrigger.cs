using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    [SerializeField]
    List<GameObject> spawnPoints;
    [SerializeField]
    GameObject PickUpSpawner;

    int waveNum;
    int totalWaves;
    bool hasStarted = false;
    float longestDelay = 2.2f;

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
        //adds un listed spawns to list
        foreach (var childObject in GetComponentsInChildren<RoomSpawnPoint>())
        {
            if (!spawnPoints.Contains(childObject.gameObject))
            {
                spawnPoints.Add(childObject.gameObject);
            }
        }
        //Read our spawnpoints
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

        //checks if we should spawn pick ups
        if (PickUpSpawner != null)
        {
            if (NoMoreWaves == true && EnemyManager.Instance.isInCombat == false && hasStarted)
            {
                PickUpSpawner temp = PickUpSpawner.GetComponent<PickUpSpawner>();
                temp.StartSpawn();
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player" && !hasStarted)
        {
            PlayerStats.ResetKillCount();
            if (audio != null)
            {
                audio.Play();
            }
            RoomManager.Instance.SetCurrentRoom(this);

            //PlayParticles();
            /*
            * Start wave spawn 
            */
            hasStarted = true;
            NoMoreWaves = false;
        }
    }

    void PlayParticles()
    {
        foreach (var roomSpawn in spawnPoints)
        {
            if (roomSpawn != null)
            {
                roomSpawn.GetComponent<RoomSpawnPoint>().PlaySpawnParticles();
            }
        }
    }

    //IEnumerator DelaySpawnForParticles(float longestDelay)
    //{
    //    //foreach (var roomSpawn in spawnPoints)
    //    //{
    //    //    if (roomSpawn != null)
    //    //    {
    //    //        roomSpawn.GetComponent<RoomSpawnPoint>().PlaySpawnParticles(); 
    //    //    }
    //    //}
    //    yield return new WaitForSeconds(longestDelay);
    //    //hasStarted = true;
    //    //NoMoreWaves = false;

    //}

    void SpawnNextWave()
    {
        foreach (var item in spawnPoints)
        {
            if (item != null)
            {
                var temp = item.GetComponent<RoomSpawnPoint>();
                if(temp.listOfWaves.Count-1 >= waveNum)
                {
                    if (!(temp.listOfWaves[waveNum].WaveList.Count == 0))
                    {
                        EnemyManager.Instance.SpawnEnemies(temp.listOfWaves[waveNum].WaveList, item.transform, longestDelay);
                        temp.PlaySpawnParticles();
                    }
                }
            }
        }
        waveNum++;
    }

    int GetTotalWaves()
    {
        int highestWaveCount = 0;
        foreach (var spawnPoint in spawnPoints)
        {
            if(spawnPoint != null)
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
        }
        return highestWaveCount;
    }
}
