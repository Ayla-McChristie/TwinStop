using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour
{
    private static EnemyManager _instance;
    public static EnemyManager Instance { get { return _instance; } }

    //[System.Serializable]
    //public class RoomDetail
    //{
    //    public int roomNum;
    //    public List<GameObject> listOfEnemies;
    //    public Transform[] SpawnLoc;
    //}
    //[SerializeField]
    //private GameObject basicEnemy;
    //[SerializeField]
    //List<RoomDetail> roomDetailTemp;
    //List<RoomDetail> roomDetail;


    [SerializeField]
    List<Pool> enemyPools;

    PlayerStats playerStats;

    //lets us know if were in combat or not. Should probably be move to the room manager
    public bool isInCombat;
    //this is technical debt for sure but we only have 2 weeks and im tired -Aidan
    bool combatOverride;
    private int numOfEnemiesInCombat;



    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        if (ObjectPool_Projectiles.Instance != null)
        {
            foreach (var p in enemyPools)
            {
                ObjectPool_Projectiles.Instance.InstantiatePool(p);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //TestInCombat();
        CheckForCombat();
    }

    protected void Spawn(GameObject go)
    {
        go = ObjectPool_Projectiles.Instance.GetProjectile(go.name);
    }
    public void SpawnEnemies(List<GameObject> listOfEnemies, Transform spawnPoint)
    {
        numOfEnemiesInCombat += listOfEnemies.Count;
        //Debug.Log($"spawning {listOfEnemies.Count} enemies");
        foreach (var go in listOfEnemies)
        {
            /*
             * rn we create new enemies but i need to make it use object pool
             */
            GameObject e = ObjectPool_Projectiles.Instance.GetProjectile(go.name);
            if (e.GetComponent<NavMeshAgent>() != null)
            {
                e.GetComponent<NavMeshAgent>().Warp(spawnPoint.position);
            }
            else
            {
                e.transform.position = spawnPoint.position;
            }
        }
        isInCombat = true;
        combatOverride = false;
    }
    public void SpawnEnemies(List<GameObject> listOfEnemies, Transform spawnPoint, float WaitTime)
    {           
        isInCombat = true;
        combatOverride = true;
        StartCoroutine(DelaySpawnForTime(listOfEnemies, spawnPoint, WaitTime));
    }
    IEnumerator DelaySpawnForTime(List<GameObject> listOfEnemies, Transform spawnPoint, float WaitTime)
    {
        yield return new WaitForSeconds(WaitTime);
        SpawnEnemies(listOfEnemies, spawnPoint);
    }
    /*
     * Runs in update and will tell us when combat is over by turning isInCombat to false
     */
    void CheckForCombat()
    {
        //Debug.Log($"there are {numOfEnemiesInCombat} enemies in combat");
        if (numOfEnemiesInCombat <= PlayerStats.NumOfKilledEnemies && combatOverride == false)
        {
            numOfEnemiesInCombat = 0;
            isInCombat = false;
        }
    }

    public void StartCombatOverride()
    {
        isInCombat = true;

    }
    /*
     * this should probably just call a method from the door manager that lets us lock and unlock all the doors
     */
    void TestInCombat()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            isInCombat = true;
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            isInCombat = false;
        }
    }

}
