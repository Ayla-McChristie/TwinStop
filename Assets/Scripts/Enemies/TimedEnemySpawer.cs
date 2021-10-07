using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedEnemySpawer : EnemyManager
{
    bool onCooldown = true;
    [SerializeField]
    float coolDownTime = 2f;
    float currentCoolDown = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        TimedSpawn();
    }

    void TimedSpawn()
    {
        if (!onCooldown)
        {
            /*
             * TODO Make it so the timed spawner can spawn at a designated spawn point or spawn points
             */
            this.Spawn();
            this.onCooldown = true;
        }
        else
        {
            currentCoolDown -= (1 / coolDownTime) * Time.deltaTime * Time.timeScale;
            if (currentCoolDown <= 0)
            {
                this.onCooldown = false;
                currentCoolDown = coolDownTime;
            }
        }
    }
}
