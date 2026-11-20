using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    PathRequestManager pathRequestManager;
    [SerializeField] private Grid _grid;

    private void Start()
    {
        if(_grid == null)
        {
            _grid = GetComponent<Grid>();
        }
        if(pathRequestManager == null)
        {
            pathRequestManager = GetComponent<PathRequestManager>();
        }
    }


    public void StartFindPath(Vector3 startPos, Vector3 targetPos)
    {
        StartCoroutine(FindPath(startPos, targetPos));
    }
    private IEnumerator FindPath(Vector3 startPos,  Vector3 targetPos)
    {

        Vector3[] waypoints = new Vector3[0];
        bool pathSuccess = false;

        Node startNode = _grid.NodeFromWorldPoint(startPos);
        Node targetNode = _grid.NodeFromWorldPoint(targetPos);

        //if(startNode.Walkable && targetNode.Walkable)
        //{
        //    List<Node> openSet = new List<Node>();
        //    HashSet<Node> closedSet = new HashSet<Node>();

        //    openSet.Add(startNode);

        //    while (openSet.Count > 0)
        //    {
        //        Node currentNode = openSet[0];
        //        for (int i = 1; i < openSet.Count; i++)
        //        {
        //            if (openSet[i].FCost < currentNode.FCost || (openSet[i].FCost == currentNode.FCost && openSet[i].HCost < currentNode.HCost))
        //            {
        //                currentNode = openSet[i];
        //            }
        //        }

        //        openSet.Remove(currentNode);
        //        closedSet.Add(currentNode);

        //        if (currentNode == targetNode)
        //        {
        //            pathSuccess = true;
        //            break;
        //        }

        //        foreach (Node neighbour in _grid.GetNeighbours(currentNode))
        //        {
        //            if (!neighbour.Walkable || closedSet.Contains(neighbour))
        //            {
        //                continue;
        //            }

        //            int newMovementCostToNeighbour = currentNode.GCost + GetDistance(currentNode, neighbour);
        //            if (newMovementCostToNeighbour < neighbour.GCost || !openSet.Contains(neighbour))
        //            {
        //                neighbour.GCost = newMovementCostToNeighbour;
        //                neighbour.HCost = GetDistance(neighbour, targetNode);
        //                neighbour.Parent = currentNode;

        //                if (!openSet.Contains(neighbour))
        //                {
        //                    openSet.Add(neighbour);
        //                }
        //            }

        //        }

        //    }
        //}

        if (startNode.Walkable && targetNode.Walkable)
        {
            Heap<Node> openSet = new Heap<Node>(_grid.MaxSize);
            HashSet<Node> closedSet = new HashSet<Node>();

            startNode.GCost = 0;
            startNode.HCost = GetDistance(startNode, targetNode);
            startNode.Parent = null;
            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                Node currentNode = openSet.RemoveFirst();
                closedSet.Add(currentNode);

                if (currentNode == targetNode) { pathSuccess = true; break; }

                foreach (Node neighbour in _grid.GetNeighbours(currentNode))
                {
                    if (!neighbour.Walkable || closedSet.Contains(neighbour)) continue;

                    int newCost = currentNode.GCost + GetDistance(currentNode, neighbour);
                    bool inOpen = openSet.Contains(neighbour);
                    if (newCost < neighbour.GCost || !inOpen)
                    {
                        neighbour.GCost = newCost;
                        neighbour.HCost = GetDistance(neighbour, targetNode);
                        neighbour.Parent = currentNode;

                        if (!inOpen) openSet.Add(neighbour);
                        else openSet.UpdateItem(neighbour);
                    }
                }
            }
        }

        yield return null;
        if (pathSuccess)
        {
            waypoints = RetracePath(startNode, targetNode);
        }
        pathRequestManager.FinishedProcessPath(waypoints, pathSuccess);
    }

    Vector3[] RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.Parent;
        }
        Vector3[] waypoints = SimplifyPath(path);
        Array.Reverse(waypoints);
        return waypoints;

    }

    Vector3[] SimplifyPath(List<Node> path)
    {
        List<Vector3> waypoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;

        if (path.Count == 1)
        {
            waypoints.Add(path[0].WorldPosition); return waypoints.ToArray();
        }


        for (int i = 1; i < path.Count; i++)
        {
            Vector2 directionNew = new Vector2(path[i-1].GridX - path[i].GridX, path[i - 1].GridY - path[i].GridY);
            if(directionNew != directionOld)
            {
                waypoints.Add(path[i].WorldPosition);
            }
            directionOld = directionNew;
        }
        return waypoints.ToArray();
    }

    private int GetDistance(Node nodeA, Node nodeB)
    {
        int distX = Mathf.Abs(nodeA.GridX - nodeB.GridX);
        int distY = Mathf.Abs(nodeA.GridY - nodeB.GridY);

        if(distX > distY)
        {
            return 14 * distY + 10 * (distX - distY);
        }
        return 14 * distX + 10 * (distY - distX);

    }



}
