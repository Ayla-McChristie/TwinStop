using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempUnit : MonoBehaviour
{
    public Transform target;
    public Vector3 targetOldPosition;
    float speed = 5;
    Vector3[] path;
    Vector3 direction;
    int targetIndex;

    private void Update()
    {
        direction = target.position - this.transform.position;
        //PathManager.RequestPath(transform.position, target.position, OnPathFound);
        if (Input.GetKey(KeyCode.Space))
            if(direction.magnitude >= 2f)
                PathManager.RequestPath(transform.position, target.position, OnPathFound);

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
            if(transform.position == currentWayPoint)
            {
                targetIndex++;
                if(targetIndex >= path.Length)
                {
                    yield break;
                }
                currentWayPoint = path[targetIndex];
            }
            transform.position = Vector3.MoveTowards(transform.position, currentWayPoint, speed * Time.deltaTime);
            yield return null;
        }
    }
}
