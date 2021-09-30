using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool_Projectiles : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int maxSize;
    }
    [SerializeField]
    List<Pool> pool;

    Dictionary<string, Queue<GameObject>> poolDictionary;

    void Start()
    {
        GameObject temp;
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach(Pool p in pool)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for(int i = 0; i < p.maxSize; i++)
            {
                temp = Instantiate(p.prefab);
                temp.SetActive(false);
                objectPool.Enqueue(temp);
            }

            poolDictionary.Add(p.tag, objectPool);
        }
    }

    public GameObject GetProjectile(string objectTag)
    {
        GameObject objectSpawn = poolDictionary[objectTag].Dequeue();
        objectSpawn.SetActive(true);
        poolDictionary[objectTag].Enqueue(objectSpawn);
        return objectSpawn;
    }

    public void DeactivateProjectile(GameObject ObjectType)
    {
        ObjectType.SetActive(false);
    }
}
