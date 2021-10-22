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

    int waveNum;



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            Debug.Log("player has entered a room");
            /*
            * Start wave spawn 
            */
            foreach (var item in spawnPoints)
            {
                Debug.Log("Spawning enemies");
                var temp = item.GetComponent<RoomSpawnPoint>();
                em.SpawnEnemies(temp.listOfWaves[waveNum].WaveList, item.transform);
            }
        }
    }
}
