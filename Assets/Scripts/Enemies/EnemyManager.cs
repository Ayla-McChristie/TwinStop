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
    }

    void TestSpawn()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Spawn();
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
                for (int i = 0; i < r.listOfEnemies.Count; i++)
                {
                    GameObject enemy = o.GetProjectile(r.listOfEnemies[i].gameObject.name);
                    enemy.transform.position = r.SpawnLoc[i].position;
                }
            }
        }


    }
}
