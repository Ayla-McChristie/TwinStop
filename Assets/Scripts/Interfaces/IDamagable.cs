using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    public float Health { get; set; }
    void TakeDamage();
    void TakeDamage(float damageAmmount);
}
