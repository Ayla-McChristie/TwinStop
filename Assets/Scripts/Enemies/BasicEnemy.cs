using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : Enemy
{
    [SerializeField]
    public float speed = 50;
    [SerializeField]
    public float health = 10;
    [SerializeField]
    public float damage = 2;

    [SerializeField]
    

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        this.Health = health;
        this.Speed = speed;
        this.Damage = damage;
    }

    // Update is called once per frame
    public override void Update()
    {
        this.Seek(target.transform.position);
        base.Update();
    }
}
