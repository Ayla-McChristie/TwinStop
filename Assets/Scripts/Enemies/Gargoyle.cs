using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Gargoyle : Enemy
{
    protected enum State {Active, DeActive }
    State state;

    [SerializeField]
    public float speed = 3;
    [SerializeField]
    public int health = 10;
    [SerializeField]
    public int damage = 1;

    Vector3 oldPosition;
    NavMeshPath path;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        state = State.DeActive;
        this.agent.speed = agent.speed / Time.deltaTime;
        //this.agent.acceleration = agent.acceleration / Time.deltaTime;
        this.agent.angularSpeed = agent.angularSpeed / Time.deltaTime;
        path = new NavMeshPath();
        oldPosition = target.transform.position;
    }

    // Update is called once per frame
    public override void FixedUpdate()
    {
        CalculatePath();
        SwitchState();
        CheckTimeIsStopped();
    }

    void SwitchState()
    {
        switch (state)
        {
            case State.Active:
                GoTowardsPlayer();
                break;
            case State.DeActive:
                break;
        }
    }

    void CheckTimeIsStopped()
    {
        if (!TimeManager.Instance.isTimeStopped)
        {
            state = State.DeActive;
            agent.isStopped = true;
            return;
        }
        state = State.Active;
        agent.isStopped = false;
    }

    void GoTowardsPlayer()
    {
        for (int i = 0; i < path.corners.Length; i++)
            Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red);
    }

    void TurnToPlayer()
    {
        Vector3 targetDir = target.transform.position - this.transform.position;
        Vector3 direction = Vector3.RotateTowards(this.transform.forward, targetDir, agent.angularSpeed * Time.deltaTime, 0.0f);
        this.transform.rotation = Quaternion.LookRotation(direction);
    }

    void CalculatePath()
    {
        if (oldPosition == target.transform.position)
        {
            oldPosition = target.transform.position;
            path.ClearCorners();
            return;
        }
        NavMesh.CalculatePath(this.transform.position, target.transform.position, NavMesh.AllAreas, path);
    }
}
