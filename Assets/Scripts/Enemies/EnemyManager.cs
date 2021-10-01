using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    private GameObject basicEnemy;
    // Start is called before the first frame update
    void Start()
    {
        /*
         * TODO Get enemy object pools
         */
    }

    // Update is called once per frame
    void Update()
    {
        TestSpawn();
    }

    void TestSpawn()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Spawn();
        }
    }

    void Spawn()
    {
        /*
         * TODO Pull from object pool instead of making new entities
         */
        GameObject e = Instantiate(basicEnemy, this.transform);
    }
}
