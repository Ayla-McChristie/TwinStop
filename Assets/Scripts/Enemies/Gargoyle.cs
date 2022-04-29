using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;
public class Gargoyle : Enemy
{
    protected enum State {Active, DeActive,Attack, Offline }
    State state;

    [SerializeField]
    public float speed = 3;
    [SerializeField]
    public int health = 10;
    [SerializeField]
    public int damage = 1;
    [SerializeField]
    VisualEffect fireBreath;
    [SerializeField]
    SentryTrigger sTrigger;


    CapsuleCollider fireB;
    float distance;
    NavMeshPath path;
    float recalculatePathTime;
    int index = 1;
    bool collideWithPlayer;
    bool triggered;
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        fireBreath.Stop();
        this.Health = health;
        if (sTrigger != null)
            state = State.Offline;
        else
            state = State.DeActive;
        path = new NavMeshPath();
        deathSound = GetComponent<AudioSource>();
        fireB = this.transform.Find("FireBreath").gameObject.GetComponent<CapsuleCollider>();
        fireB.height = 0;
        fireB.enabled = false;
        triggered = false;
    }

    // Update is called once per frame
    public override void FixedUpdate()
    {
        TriggerActivated();
        CalculatePath();
        SwitchState();
        CheckTimeIsStopped();
        DeathSoundClipTime();
        base.FixedUpdate();
    }

    void TriggerActivated()
    {
        if (triggered)
            return;
        if (sTrigger == null)
            return;
        if (this.sTrigger.isTriggered)
        {
            triggered = true;
            state = State.DeActive;
        }
    }

    void SwitchState()
    {
        switch (state)
        {
            case State.Offline:
                break;
            case State.Active:
                GoTowardsPlayer();
                TurnToPlayer();
                //Distance2Player();
                AttackTemp();
                break;
            case State.Attack:
                AttackTemp();
                break;
            case State.DeActive:
                TurnOffFire();
                break;
        }
    }

    void CheckTimeIsStopped()
    {
        if (state == State.Offline)
            return;
        if (!TimeManager.Instance.isTimeStopped)
        {
            state = State.DeActive;
            agent.isStopped = true;
            return;
        }
        if (state == State.Attack)
            return;
        state = State.Active;
        agent.isStopped = false;
    }

    void AttackTemp()
    {
        //VFXManager.fixedTimeStep = VFXManager.maxDeltaTime;
        GrowFireBreath();
        fireBreath.Play();
        fireB.enabled = true;
    }

    void TurnOffFire()
    {
        fireB.enabled = false;
        fireBreath.Stop();
        fireB.height = 0;
    }

    void Distance2Player()
    {
        distance = Vector3.Distance(this.transform.position,target.transform.position );
        if (distance < 3f)
        {
            agent.isStopped = true;
            transform.position += Vector3.zero;
            return;
        }
        agent.isStopped = false;
    }

    void GoTowardsPlayer()
    {
        if (index >= path.corners.Length)
        {
            index = 0;
            return;
        }
        float distance = Vector3.Distance(this.transform.position, path.corners[index]);
        if(distance < 3f)
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

    void GrowFireBreath()
    {
        if (fireB.height < 2.45f)
            fireB.height += 2f * Time.unscaledDeltaTime;
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        if (TimeManager.Instance.isTimeStopped)
            base.OnCollisionEnter(collision);

        if (collision.transform.tag == "Player")
            collideWithPlayer = true;
        if (collision.transform.tag == "PlayerBullet")
            this.transform.GetComponentInChildren<BubbleShield>().HitShield(collision.transform.position);
    }

    private void OnCollisionExit(Collision collision)
    {
        collideWithPlayer = false;
    }
}
