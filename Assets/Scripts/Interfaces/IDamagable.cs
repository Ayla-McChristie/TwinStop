using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    public float Health { get; }
    void TakeDamage();
    void TakeDamage(float damageAmount);
}
