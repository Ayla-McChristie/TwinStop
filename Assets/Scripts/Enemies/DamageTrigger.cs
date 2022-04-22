using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTrigger : MonoBehaviour
{
    float damageRate;
    bool isDealDamage;
    GameObject target;
    private void Start()
    {
        isDealDamage = false;
    }

    private void Update()
    {
        if (!isDealDamage)
            return;

        if (damageRate < 3f)
        {
            damageRate += Time.unscaledDeltaTime;
            return;
        }
        //target.GetComponent<PlayerStats>().TakeDamage();
        DealDamage();
        damageRate = 0;
    }

    void DealDamage()
    {
        target.GetComponent<PlayerStats>().TakeDamage();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform.tag == "Player")
        {
            target = other.gameObject;
            isDealDamage = true;
            DealDamage();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        isDealDamage = false;
        damageRate = 0;
    }
}
