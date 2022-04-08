using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChronoLordAttackPattern : MonoBehaviour
{
    Animator MyAnimator;

    [SerializeField]
    GameObject[] Chargers;
    // Start is called before the first frame update
    void Start()
    {
        MyAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsWaveFinished(Chargers))
        {
            MyAnimator.SetTrigger("ChargerWaveEnd");
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
        MyAnimator.SetBool("Vulnerable", true);
        return true;
    }
}
