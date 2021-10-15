using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
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
    [HideInInspector]
    public bool isInCombat;
    private int numOfEnemiesInCombat;

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
        TestInCombat();
    }

    void TestSpawn()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Spawn();
        }
    }
    /*
     * this should probably just call a method from the door manager that lets us lock and unlock all the doors
     */
    void TestInCombat()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            isInCombat = true;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            isInCombat = false;
        }
    }

    protected void Spawn()
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
    /*
     * Runs in update and will tell us when combat is over by turning isInCombat to false
     */
    void CheckForCombat()
    {
        if (numOfEnemiesInCombat <= 0)
        {
            isInCombat = false;
        }
    }
}
