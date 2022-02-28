using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Turtle : Enemy
{
    enum State { Attack, Defend}
    // Start is called before the first frame update
    [SerializeField]
    public float health = 15;
    [SerializeField]
    public float damage = 1;
    [SerializeField]
    public float attackRate = 2;
    [SerializeField]
    public float speed = 2;

    State state;

    float attackCounter;
    bool isCollideWithPlayer;
    public override void Start()
    {
        base.Start();
        this.Health = health;
        agent = GetComponent<NavMeshAgent>();
        agent.speed = speed;
        state = State.Attack;
        deathSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    public override void FixedUpdate()
    {
        SwitchState();
        FreezeNavMeshAgent();
        DeathSoundClipTime();
        base.FixedUpdate();
    }

    void SwitchState()
    {
        switch (state)
        {
            case State.Attack:
                MoveToPlayer();
                AttackPlayer();
                break;
            case State.Defend:
                break;
        }
    }

    void MoveToPlayer()
    {
        agent.SetDestination(target.transform.position);
    }

    void FreezeNavMeshAgent()
    {
        if (TimeManager.Instance.isTimeStopped)
        {
            agent.isStopped = true;
            return;
        }
        agent.isStopped = false;
    }

    void AttackPlayer()
    {
        Vector3 distance = target.transform.position - this.transform.position;
        if (!(distance.magnitude < 1f))
            return;
        if (IsAttackRate())
            Debug.Log("Hit Player"); //DamagePlayer();
    }

    bool IsAttackRate()
    {
        if (attackCounter < attackRate)
        {
            attackCounter += Time.deltaTime;
            return false;
        }
        attackCounter = 0;
        return true;
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player")
            isCollideWithPlayer = true; 
        if (!TimeManager.Instance.isTimeStopped)
            base.OnCollisionEnter(collision);
    }

    private void OnCollisionExit(Collision collision)
    {
        isCollideWithPlayer = false;
    }
}
