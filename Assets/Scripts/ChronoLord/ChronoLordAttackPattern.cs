using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChronoLordAttackPattern : MonoBehaviour
{
    Animator MyAnimator;

    [SerializeField]
    GameObject[] Chargers, TestEnemies;

    GameObject[] waveToCheck;

    bool IsVulnerable;

    int waveIndex;

    string TriggerToSet;
    // Start is called before the first frame update
    void Start()
    {
        MyAnimator = GetComponent<Animator>();
        IsVulnerable = false;
        waveIndex = 1;
        waveToCheck = Chargers;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsWaveFinished(waveToCheck) && !IsVulnerable)
        {
            MyAnimator.SetTrigger(TriggerToSet);
        }
    }

    bool IsWaveFinished(GameObject[] enemiesInWave)
    {
        foreach (GameObject enemy in enemiesInWave)
        {
            if (enemy.activeInHierarchy)
            {
                return false;
            }
        }
        return true;
    }

    void MakeInvulnerable()
    {
        MyAnimator.SetBool("Vulnerable", false);
        IsVulnerable = false;
    }

    void MakeVulnerable()
    {
        MyAnimator.SetBool("Vulnerable", true);
        IsVulnerable = true;
    }

    void IncrementWave()
    {
        switch (waveIndex)
        {
            case 1:
                waveToCheck = Chargers;
                TriggerToSet = "ChargerWaveEnd";
                break;
            case 2:
                waveToCheck = TestEnemies;
                TriggerToSet = "TestEnemyWaveEnd";
                break;
        }
        waveIndex++;
        MakeVulnerable();
    }
}
