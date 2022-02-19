using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Turtle : Enemy
{
    enum State { Attack, Defend}
    // Start is called before the first frame update
    [SerializeField]
    public float health = 10;
    [SerializeField]
    public float damage = 2;

    State state;
    public override void Start()
    {
        base.Start();
        agent = GetComponent<NavMeshAgent>();
        state = State.Attack;
    }

    // Update is called once per frame
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        SwitchState();
        FreezeNavMeshAgent();
    }

    void SwitchState()
    {
        switch (state)
        {
            case State.Attack:
                MoveToPlayer();
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
}
