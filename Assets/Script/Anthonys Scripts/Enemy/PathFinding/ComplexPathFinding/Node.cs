using UnityEngine;

public class Node
{
    public bool Walkable;
    public Vector3 WorldPosition;
    public int GridX;
    public int GridY;


    public int GCost;
    public int HCost;
    public Node Parent;


    public Node(bool _walkable, Vector3 _worldPosition, int _gridX, int _gridY)
    {
        Walkable = _walkable;
        WorldPosition = _worldPosition;
        GridX = _gridX;
        GridY = _gridY;
    }

    public int FCost
    {
        get { return GCost + HCost; }
    }
}
