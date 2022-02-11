using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public bool doIFollow, needToMove;
    //this is a dumb idea to make the jank camera system work - Aidan
    public bool safetyBuffer = false;

    GameObject currentTarget;

    [SerializeField]
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        doIFollow = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (doIFollow)
        {
            this.transform.position = player.transform.position;
        }
        if(needToMove)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, currentTarget.transform.position, 100 * Time.deltaTime);
            if (this.transform.position == currentTarget.transform.position)
            {
                needToMove = false;
            }
            
        }
    }

    public void Move(GameObject newTarget)
    {
        if (safetyBuffer)
        {
            safetyBuffer = !safetyBuffer;
        }
        else
        {
            currentTarget = newTarget;
            needToMove = true;
        }
    }
}
