using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node {

    public  bool walkable;
    public bool zWalkable;
    public Vector2 worldPosition;

    public Node(bool _walkable, Vector2 _worldPosition, bool _zWalkwable)
    {
        walkable = _walkable;
        zWalkable = _zWalkwable;
        worldPosition = _worldPosition;
    }
}
