using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Gargoyle : Enemy
{
    protected enum State {Active, DeActive,Attack }
    State state;

    [SerializeField]
    public float speed = 3;
    [SerializeField]
    public int health = 10;
    [SerializeField]
    public int damage = 1;

    Vector3 destination;
    NavMeshPath path;
    float recalculatePathTime;
    int index = 1;
    bool collideWithPlayer;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        this.Health = health;
        state = State.DeActive;
        path = new NavMeshPath();
        deathSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    public override void FixedUpdate()
    {
        CalculatePath();
        SwitchState();
        CheckTimeIsStopped();
        DeathSoundClipTime();
        //Distance2Player();
        base.FixedUpdate();
    }

    void SwitchState()
    {
        switch (state)
        {
            case State.Active:
                GoTowardsPlayer();
                break;
            case State.Attack:
                AttackTemp();
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

    void AttackTemp()
    {
        //this.transform.Rotate(Vector3.right * Time.unscaledDeltaTime *100);
    }

    void Distance2Player()
    {
        destination = target.transform.position - this.transform.position;
        if (destination.magnitude < 2f)
            state = State.Attack;

    }

    void GoTowardsPlayer()
    {
        if (index >= path.corners.Length)
            return;
        float distance = Vector3.Distance(this.transform.position, path.corners[index]);
        if(distance < 1f)
        {
            index++;
            return;
        }
        this.transform.position = Vector3.MoveTowards(this.transform.position, path.corners[index], agent.speed / 50);
    }

    void TurnToPlayer()
    {
        Vector3 targetDir = target.transform.position - this.transform.position;
        Vector3 direction = Vector3.RotateTowards(this.transform.forward, targetDir, agent.angularSpeed * Time.deltaTime, 0.0f);
        this.transform.rotation = Quaternion.LookRotation(direction);
    }

    void CalculatePath()
    {
        NavMesh.CalculatePath(this.transform.position, target.transform.position, NavMesh.AllAreas, path);
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        if (TimeManager.Instance.isTimeStopped)
            base.OnCollisionEnter(collision);

        if (collision.transform.tag == "Player")
            collideWithPlayer = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        collideWithPlayer = false;
    }
}
