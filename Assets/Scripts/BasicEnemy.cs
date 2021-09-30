using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : Enemy
{
    [SerializeField]
    public float speed = 10;
    [SerializeField]
    public float health = 10;
    [SerializeField]
    public float damage = 2;

    // Start is called before the first frame update
    void Start()
    {
        this.Health = health;
        this.Speed = speed;
        this.Damage = damage;
    }

    // Update is called once per frame
    public override void Update()
    {
        this.Seek(new Vector3(100, 5, 67));
        base.Update();
    }
}
