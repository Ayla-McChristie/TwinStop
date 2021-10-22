using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PathManager : MonoBehaviour
{
    Queue<PathRequest> pathRequestQ = new Queue<PathRequest>();
    PathRequest currentPathR;

    static PathManager instance;
    Pathfinding pF;

    bool isProcessingPath;
    private void Awake()
    {
        instance = this;
        pF = GetComponent<Pathfinding>();
    }


    public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callBack)
    {
        PathRequest newRequest = new PathRequest(pathStart, pathEnd, callBack);
        instance.pathRequestQ.Enqueue(newRequest);
        instance.TryProcessNext();
    }

    void TryProcessNext()
    {
        if(!isProcessingPath && pathRequestQ.Count > 0)
        {
            currentPathR = pathRequestQ.Dequeue();
            isProcessingPath = true;
            pF.StartFindPath(currentPathR.pathStart, currentPathR.pathEnd);
        }
    }

    public void FinishedProcessPath(Vector3[] path, bool success)
    {
        currentPathR.callBack(path, success);
        isProcessingPath = false;
        TryProcessNext();
    }
    struct PathRequest
    {
        public Vector3 pathStart;
        public Vector3 pathEnd;
        public Action<Vector3[], bool> callBack;

        public PathRequest(Vector3 _pathStart, Vector3 _pathEnd, Action<Vector3[], bool> _callBack)
        {
            pathStart = _pathStart;
            pathEnd = _pathEnd;
            callBack = _callBack;
        }
    }
}
