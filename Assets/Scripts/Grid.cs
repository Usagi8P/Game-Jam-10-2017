using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour {
    public GameObject[] obsticals;
    public GameObject zombie;

    public GameObject[] zombies;
    public GameObject tile;
    public GameObject wall;

    public LayerMask unwalkwableMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    Node[,] grid;

    float nodeDiameter;
    int gridSizeX, gridSizeY;

    void Awake()
    {
        
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        Debug.Log(gridSizeX.ToString() + "     " + gridSizeY.ToString());
        CreateGrid();
    }
    private void Start()
    {
        
    }

    void CreateGrid()
    {
        int ranBlock, ranX, ranY;
        


        grid = new Node[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.up * gridWorldSize.y / 2;
        for (int i = 0; i < 2; i++)
        {
            ranBlock = Mathf.RoundToInt(Random.Range(1, 3));
            ranX = Mathf.RoundToInt(Random.Range(-gridWorldSize.x / 4, gridWorldSize.x / 4));
            ranY = Mathf.RoundToInt(Random.Range(-gridWorldSize.y / 4, gridWorldSize.y / 4));
            Instantiate(obsticals[ranBlock], new Vector3(ranX, ranY, 0f), new Quaternion(0f, 0f, 0f, 0f));
        }
        for (int i = 0; i < 2; i++)
        {
            ranX = Mathf.RoundToInt(Random.Range(-gridWorldSize.x / 4, gridWorldSize.x / 4));
            ranY = Mathf.RoundToInt(Random.Range(-gridWorldSize.y / 4, gridWorldSize.y / 4));

            Instantiate(obsticals[0], new Vector3(ranX, ranY, 0f), new Quaternion(0f, 0f, 0f, 0f));
        }
        
        //spawn map
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.up * (y * nodeDiameter + nodeRadius);
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkwableMask));
                grid[x, y] = new Node(walkable, new Vector2(worldPoint.x, worldPoint.y), walkable);
                if (walkable)
                {
                    Instantiate(tile, worldPoint + Vector3.forward, new Quaternion(0f, 0f, 0f, 0f));
                }
                else
                {
                    Instantiate(wall, worldPoint + Vector3.forward, new Quaternion(0f, 0f, 0f, 0f));
                }
                
            }
        }
        
        

    }

    private void SpawnZombie(int x, int y)
    {
        if (!grid[x, y].walkable || !grid[x, y].zWalkable)
        {
            SpawnZombie(Random.Range(0, gridSizeX), Random.Range(0, gridSizeY));
        }
        else
        {
            Instantiate(zombie, grid[x,y].worldPosition, new Quaternion(0f,0f,0f,0f));
        }
    }

    public int GetGridSizeX()
    {
        return gridSizeX;
    }
    public int GetGridSizeY()
    {
        return gridSizeY;
    }

    public Node NodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = Mathf.Clamp01((worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x);
        float percentY = Mathf.Clamp01((worldPosition.y + gridWorldSize.y / 2) / gridWorldSize.y);

        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

        return grid[x, y];
    }

    public Node NodeFromGridPosition(int x, int y)
    {
        return grid[x, y];
    }
    //Testing
    /*
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, gridWorldSize.y,1));

        if (grid != null)
        {
            foreach (Node n in grid)
            {
                Gizmos.color = (n.walkable) ? Color.white : Color.red;
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - 0.1f));
            }
        }
    }
    */
    
}
