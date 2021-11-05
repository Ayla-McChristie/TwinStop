using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    public int Health { get; set; }
    void TakeDamage();
    void TakeDamage(int damageAmmount);
}
