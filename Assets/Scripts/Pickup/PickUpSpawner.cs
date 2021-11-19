using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSpawner : MonoBehaviour
{

    [SerializeField]
    List<GameObject> PickUpsToSpawn;
    // Start is called before the first frame update
    void Start()
    {
        if (ObjectPool_Projectiles.Instance == null)
        {
            ObjectPool_Projectiles.CreateObjectPoolInstance();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ReleasePickUps()
    {

    }
}
