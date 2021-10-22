using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System;
public class Pathfinding : MonoBehaviour
{
    PathManager pm;
    Grid grid;

    private void Awake(){
        pm = GetComponent<PathManager>();
        grid = GetComponent<Grid>();
    }

    public void StartFindPath(Vector3 startPos, Vector3 targetPos)
    {
        StartCoroutine(FindPath(startPos, targetPos));
    }

    IEnumerator FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();

        Vector3[] wayPoint = new Vector3[0];
        bool pathSuccess = false;

        Node startNode = grid.NodeFromWorldPoint(startPos);
        Node targetNode = grid.NodeFromWorldPoint(targetPos);
        if (startNode.walkable && targetNode.walkable)
        {
            List<Node> openSet = new List<Node>();
            HashSet<Node> closedSet = new HashSet<Node>();
            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                Node currentNode = openSet[0];
                for (int i = 1; i < openSet.Count; i++)
                    if (openSet[i].fCost < currentNode.fCost || openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                        currentNode = openSet[i];

                openSet.Remove(currentNode);
                closedSet.Add(currentNode);

                if (currentNode == targetNode)
                {
                    sw.Stop();
                    print("Path found: " + sw.ElapsedMilliseconds + " ms");
                    pathSuccess = true;
                    break;
                }

                foreach (Node neighbor in grid.GetNeighbors(currentNode))
                {
                    if (!neighbor.walkable || closedSet.Contains(neighbor))
                        continue;

                    int newMoveCost2Neighbor = currentNode.gCost + GetDistance(currentNode, neighbor);

                    if (newMoveCost2Neighbor < neighbor.gCost || !openSet.Contains(neighbor))
                    {
                        neighbor.gCost = newMoveCost2Neighbor;
                        neighbor.hCost = GetDistance(neighbor, targetNode);
                        neighbor.parent = currentNode;

                        if (!openSet.Contains(neighbor))
                            openSet.Add(neighbor);
                    }

                }
            }
        }
        yield return null;
        if (pathSuccess)
            wayPoint = RetracePath(startNode, targetNode);

        pm.FinishedProcessPath(wayPoint, pathSuccess);
    }

    int GetDistance(Node a, Node b)
    {
        int distX = Mathf.Abs(a.gridX - b.gridX);
        int distY = Mathf.Abs(a.gridY - b.gridY);

        if (distX > distY)
            return 14 * distY + 10 * (distX - distY);
        return 14 * distX + 10 * (distY - distX);
    }

    Vector3[] RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;
        
        while(currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        
        Vector3[] wayPoints = SimplifyPath(path);
        path.Reverse();
        grid.path = path;
        Array.Reverse(wayPoints);


        return wayPoints;
    }

    Vector3[] SimplifyPath(List<Node> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;

        for(int i = 1; i <path.Count; i++)
        {
            Vector2 directionNew = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);
            if( directionNew != directionOld)
            {
                waypoints.Add(path[i].worldPos);
            }
            directionOld = directionNew;
        }

        return waypoints.ToArray(); ;
    }
}
