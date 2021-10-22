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
    [SerializeField]
    EnemyManager em;

    PlayerStats player;

    int waveNum;
    bool hasStarted = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player" && !hasStarted)
        {
            Debug.Log("player has entered a room");
            player.numOfKilledEnemies = 0;
            hasStarted = true;
            /*
            * Start wave spawn 
            */
            foreach (var item in spawnPoints)
            {
                Debug.Log("Spawning enemies");
                var temp = item.GetComponent<RoomSpawnPoint>();
                em.SpawnEnemies(temp.listOfWaves[waveNum].WaveList, item.transform);
            }
            waveNum++;
        }
    }
}
