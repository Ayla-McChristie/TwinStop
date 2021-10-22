using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private static EnemyManager _instance;
    public static EnemyManager Instance { get { return _instance; } }

    [System.Serializable]
    public class RoomDetail
    {
        public int roomNum;
        public List<GameObject> listOfEnemies;
        public Transform[] SpawnLoc;
    }
    //[SerializeField]
    //private GameObject basicEnemy;
    [SerializeField]
    List<RoomDetail> roomDetailTemp;
    List<RoomDetail> roomDetail;
    public GameObject basicEnemy;
    public ObjectPool_Projectiles o;

    //lets us know if were in combat or not. Should probably be move to the room manager
    public bool isInCombat;
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
        roomDetail = new List<RoomDetail>();
        foreach (RoomDetail r in roomDetailTemp)
        {
            roomDetail.Add(r);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //TestSpawn();
        //TestInCombat();
        CheckForCombat();
    }

    void TestSpawn()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Spawn();
        }
    }

    public void Spawn()
    {
        /*
         * TODO Pull from object pool instead of making new entities
         */
        GameObject e = Instantiate(basicEnemy, this.transform);
    }

    public void SpawnEnemies(int room)
    {
        foreach (RoomDetail r in roomDetail)
        {
            if (r.roomNum == room)
            {
                numOfEnemiesInCombat = r.listOfEnemies.Count;
                for (int i = 0; i < r.listOfEnemies.Count; i++)
                {
                    GameObject enemy = o.GetProjectile(r.listOfEnemies[i].gameObject.name);
                    enemy.transform.position = r.SpawnLoc[i].position;
                }
            }
        }

        //lets us know combat has started
        isInCombat = true;
    }
    public void SpawnEnemies(List<GameObject> listOfEnemies, Transform spawnPoint)
    {
        numOfEnemiesInCombat += listOfEnemies.Count;
        Debug.Log($"spawning {listOfEnemies.Count} enemies");
        foreach (var go in listOfEnemies)
        {
            /*
             * rn we create new enemies but i need to make it use object pool
             */
            GameObject e = Instantiate(go, spawnPoint);

        }
        isInCombat = true;
    }
    /*
     * Runs in update and will tell us when combat is over by turning isInCombat to false
     */
    void CheckForCombat()
    {
        Debug.Log($"there are {numOfEnemiesInCombat} enemies in combat");
        if (numOfEnemiesInCombat <= 0)
        {
            isInCombat = false;
        }
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
