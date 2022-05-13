using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniBossEnemySpawn : MonoBehaviour
{
    [SerializeField] List<EnemyWave> enemyWave;
    [SerializeField] List<GameObject> enemy;

    int enemyWaveCount;
    int waveCountMax;
    float deactiveTimer;
    void Start()
    {
        waveCountMax = enemyWave.Count;
        //wDebug.Log(enemyWaveCount + " " + waveCountMax);
    }

    // Update is called once per frame
    public void GetEnemyWave()
    {
        if (enemyWaveCount >= waveCountMax)
            return;
        if (enemyWave[enemyWaveCount].enemyList == null)
            return;
        EnemyManager.Instance.SpawnEnemies(enemyWave[enemyWaveCount].enemyList, this.transform);
        enemyWaveCount++;
    }

    public void SpawnEnemy()
    {
        if (enemy == null)
            return;
        EnemyManager.Instance.SpawnEnemies(enemy, this.transform);
    }

    [System.Serializable]
    public class EnemyWave
    {
        public List<GameObject> enemyList;
    }

}
