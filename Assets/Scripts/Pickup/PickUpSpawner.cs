using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSpawner : MonoBehaviour
{
    [SerializeField]
    float sideForce = .1f;
    [SerializeField]
    float upwardForce = 1f;
    [SerializeField]
    float forceDeadzone = 1f;
    [SerializeField]
    float spawnRateInSeconds = .5f;
    [SerializeField]
    List<GameObject> pickupsToSpawn;
    [SerializeField]
    List<Pool> pickupPools;
    int currentPickUp;
    bool onCooldown = false;
    bool hasFired = false;
    float timeStamp;
    // Start is called before the first frame update
    void Start()
    {
        if (ObjectPool_Projectiles.Instance == null)
        {
            ObjectPool_Projectiles.CreateObjectPoolInstance();
        }
        foreach (var pool in pickupPools)
        {
            if (pool.prefab != null)
            {
                if (!ObjectPool_Projectiles.Instance.CheckIfDictionaryHasValue(pool))
                {
                    ObjectPool_Projectiles.Instance.InstantiatePool(pool);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (hasFired)
        {
            ReleasePickUps();
        }
    }

    void ReleasePickUps()
    {
        if (Time.time >= timeStamp && currentPickUp <= pickupsToSpawn.Count - 1)
        {
            GameObject temp = ObjectPool_Projectiles.Instance.GetProjectile(pickupsToSpawn[currentPickUp].name);
            temp.transform.position = this.transform.position;
            currentPickUp++;
            if (temp.GetComponent<Rigidbody>() != null)
            {
                float xForce = Random.Range(forceDeadzone, sideForce);
                float yForce = Random.Range(upwardForce / 2, upwardForce);
                float zForce = Random.Range(forceDeadzone, sideForce);
                

                Vector3 force = new Vector3(xForce * GetRandomSign(), yForce, zForce * GetRandomSign());

                temp.GetComponent<Rigidbody>().velocity = force;
            }
            timeStamp = Time.time + spawnRateInSeconds;
        }
    }

    int GetRandomSign()
    {
        int temp = 1;
        int num = (Random.Range(1, 10) % 2);

        if (num == 1)
        {
            return temp;
        }
        else
        {
            return -temp;
        }
    }

    public void StartSpawn()
    {
        this.hasFired = true;
    }
}
