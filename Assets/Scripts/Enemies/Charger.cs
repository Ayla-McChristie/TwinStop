using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charger : Enemy
{
    enum State {Charge, stall }

    [SerializeField]
    public float speed = 20;
    [SerializeField]
    public float rotSpeed = 2;
    [SerializeField]
    public float health = 10;
    [SerializeField]
    public float damage = 2;

    State state;
    float timer;

    // Start is called before the first frame update
    public override void Start()
    {
        this.Speed = speed;
        this.Health = health;
        this.Damage = damage;
        //this.agent.speed = speed;
        rigidbody = GetComponent<Rigidbody>();

        state = State.Charge;
        base.Start();
    }

    // Update is called once per frame
    public override void FixedUpdate()
    {
        Debug.Log(Health);
        switch (state)
        {
            case State.Charge:
                ChargeTowards();

                break;
            case State.stall:
                GetTarget();
                break;
        }
    }

    void GetTarget()
    {
        if (timer >= 3)
        {
            timer = 0;
            state = State.Charge;
        }
        else
            timer += Time.deltaTime;
    }

    public void DamageTaken(float damage)
    {
        this.TakeDamage(damage);
    }

    void ChargeTowards()
    {
        Vector3 distance = target.transform.position - this.transform.position;
        this.agent.SetDestination(target.transform.position);
        //this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(target.transform.position), Time.deltaTime * rotSpeed);
        this.transform.LookAt(target.transform.position);
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player")
            state = State.stall;
    }
}
