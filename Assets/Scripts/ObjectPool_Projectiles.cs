using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool_Projectiles : MonoBehaviour
{
    [SerializeField]
    int maxBulletCount;
    [SerializeField]
    GameObject pooledBullet;
    [SerializeField]
    GameObject enemyProjectile;

    List<GameObject> playerProjectiles;
    List<GameObject> enemyProjectiles;

    void Start()
    {
        GameObject temp;
        playerProjectiles = new List<GameObject>();
        enemyProjectiles = new List<GameObject>();
        for (int i = 0; i < maxBulletCount; i++)
        {
            temp = Instantiate(pooledBullet);
            temp.SetActive(false);
            playerProjectiles.Add(temp);
        }
    }

    public GameObject GetProjectile(string user)
    {
        if (user == "Player")
        {
            for (int i = 0; i < playerProjectiles.Count; i++)
            {
                if (!playerProjectiles[i].activeSelf)
                {
                    playerProjectiles[i].SetActive(true);
                    return playerProjectiles[i];
                }
            }
        }
        else if (user == "Enemy")
        {
            for (int i = 0; i < enemyProjectiles.Count; i++)
            {
                if (!playerProjectiles[i].activeSelf)
                {
                    enemyProjectiles[i].SetActive(true);
                    return enemyProjectiles[i];
                }
            }
        }
        return null;
    }

    public void DeactivateProjectile(GameObject bullet)
    {
        bullet.SetActive(false);
    }
}
