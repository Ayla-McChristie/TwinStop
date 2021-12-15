using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charger : Enemy
{
    enum State {Charge, stall, Rotate }

    [SerializeField]
    public float speed = 20;
    [SerializeField]
    public float rotSpeed = 2;
    [SerializeField]
    public float health = 10;
    [SerializeField]
    public float damage = 2;
    [SerializeField]
    float stallTime = 2;

    AudioClip shieldHitSound;
    AudioClip moveSound;
    AudioSource[] sounds;

    State state;
    float timer;
    Vector3 targetDis;
    Vector3 hitPos;
    // Start is called before the first frame update
    public override void Start()
    {
        this.Speed = speed;
        this.Health = health;
        this.Damage = damage;
        //this.agent.speed = speed;
        rigidbody = GetComponent<Rigidbody>();
        sounds = GetComponents<AudioSource>();
        shieldHitSound = sounds[1].clip;
        moveSound = sounds[2].clip;
        state = State.Charge;
        deathSound = GetComponent<AudioSource>();
        base.Start();
    }

    // Update is called once per frame
    public override void FixedUpdate()
    {
        if (!isDead)
        {
            SlowSound();
            targetDis = (new Vector3(target.transform.position.x, this.transform.position.y, target.transform.position.z) - this.transform.position).normalized;
            switch (state)
            {
                case State.Charge:
                    ChargeTowards();
                    break;
                //case State.stall:
                //    Stall();
                    //break;
                case State.Rotate:
                    RotateToPlayer();
                    break;
            }
            Debug.Log(Health);
            if (CanSeeTarget())
                state = State.Charge;
            CheckIfPlayerIsBehind();
            if (!sounds[2].isPlaying)
                sounds[2].PlayOneShot(moveSound);
        }
        base.FixedUpdate();
        DeathSoundClipTime();
    }

    void SlowSound()
    {
        if (TimeManager.Instance.isTimeStopped)
        {
            foreach (AudioSource a in sounds)
                a.pitch = .5f;
        }
        else
        {
            foreach (AudioSource a in sounds)
                a.pitch = 1f;
        }
    }

    void CheckIfPlayerIsBehind()
    {
        if (!(Vector3.Dot(this.transform.forward, targetDis) > 0))//calculates the dot product of the two vectors. Sees whethere the player is in front on this gameobject
        {
            state = State.Rotate;
        }
    }

    void RotateToPlayer()
    {
        Quaternion rot2Target = Quaternion.LookRotation(target.transform.position - this.transform.position);
        this.transform.rotation = Quaternion.RotateTowards(this.transform.rotation, rot2Target, rotSpeed * Time.deltaTime);
    }

    void Stall()
    {
        if (timer >= stallTime)
        {
            timer = 0;
            state = State.Rotate;
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
        this.agent.SetDestination(target.transform.position);

    }

    bool CanSeeTarget()
    {
        RaycastHit hit;
        if (Physics.Raycast(this.transform.position, this.transform.forward, out hit, fovDist, mask) 
                            && hit.collider.gameObject.tag == "Player")
            return true;
        return false;
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "PlayerBullet")
            sounds[1].PlayOneShot(shieldHitSound);
    }
}
