using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charger : Enemy
{
    enum State {Aim, Charge, Attack}

    [SerializeField]
    public float speed = 20;
    [SerializeField]
    public float health = 10;
    [SerializeField]
    public float damage = 2;

    State state;
    Vector3[] path;
    Vector3 direction;
    Vector3 targetLoc;
    int targetIndex;
    float timer;
    bool scaledUp = false;

    // Start is called before the first frame update
    public override void Start()
    {
        this.Speed = speed;
        this.Health = health;
        this.Damage = damage;

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
                base.FixedUpdate();
                break;
            case State.Attack:
                Attack();
                break;
        }
    }

    void GetTarget()
    {
        if (timer >= 3)
        {
            targetLoc = target.transform.position;
            timer = 0;
            state = State.Charge;
        }
        else
            timer += Time.deltaTime;
    }

    void Attack()
    {
        //Temporary
        //if(scaledUp)
        //{
        //    this.transform.localScale -= new Vector3(0.7f, 0.7f, 0.7f) * Time.deltaTime;
        //    if (this.transform.localScale.magnitude <= (new Vector3(1f, 1f, 1f).magnitude))
        //    {
        //        scaledUp = false;
        //        state = State.Charge;
        //    }
        //}
        //else if(!scaledUp)
        //{
        //    this.transform.localScale += new Vector3(0.5f, 0.5f, 0.5f) * Time.deltaTime;
        //    if(this.transform.localScale.magnitude > (new Vector3(1.5f, 1.5f, 1.5f).magnitude))
        //        scaledUp = true;
        //}
    }
    
    public void DamageTaken(float damage)
    {
        this.TakeDamage(damage);
    }

    void ChargeTowards()
    {
        Vector3 distance = target.transform.position - this.transform.position;
        if (distance.magnitude > 2f)
        {
            transform.position = Vector3.MoveTowards(this.transform.position, target.transform.position, speed * Time.deltaTime);//PathManager.RequestPath(transform.position, targetLoc, OnPathFound);
        }
        else
            state = State.Attack;
        
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            StopCoroutine("FollowPath");
            path = newPath;
            targetIndex = 0;
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator FollowPath()
    {
        Vector3 currentWayPoint = path[0];
        while (true)
        {
            if (transform.position == currentWayPoint)
            {
                targetIndex++;
                if (targetIndex >= path.Length)
                {
                    yield break;
                }
                currentWayPoint = new Vector3(path[targetIndex].x, this.transform.position.y, path[targetIndex].z);
            }
            //this.Seek(currentWayPoint); Debug.Log(currentWayPoint);
            this.transform.position = Vector3.MoveTowards(this.transform.position, currentWayPoint, speed * Time.deltaTime);
            yield return null;
        }
    }
}
