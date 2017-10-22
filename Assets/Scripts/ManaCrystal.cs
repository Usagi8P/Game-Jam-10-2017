using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaCrystal : MonoBehaviour {

    

    private Color color;
    private SpriteRenderer spriteRenderer;


    int gridSizeX, gridSizeY;
    //Grid variables
    public int xPos, yPos;
    public GameObject gridSystem;
    private Grid grid;
    Node desiredNode;


    void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        color = spriteRenderer.color;

        grid = gridSystem.GetComponent<Grid>();

        gridSizeX = grid.GetGridSizeX();
        gridSizeY = grid.GetGridSizeY();

        //Set inital position
        xPos = Mathf.RoundToInt(Random.Range(0, 19));
        yPos = Mathf.RoundToInt(Random.Range(0, 11));
        desiredNode = grid.NodeFromGridPosition(xPos, yPos);
        transform.position = new Vector3(desiredNode.worldPosition.x, desiredNode.worldPosition.y, 0);
        if (!grid.NodeFromGridPosition(xPos, yPos).walkable)
        {
            GoRandomLocation();
        }
       
    }

    public void GoRandomLocation()
    {
        xPos = Mathf.RoundToInt(Random.Range(0, 19));
        yPos = Mathf.RoundToInt(Random.Range(0, 11));
        desiredNode = grid.NodeFromGridPosition(xPos, yPos);
        transform.position = new Vector3(desiredNode.worldPosition.x, desiredNode.worldPosition.y, 0);
        if (!grid.NodeFromGridPosition(xPos, yPos).walkable)
        {
            GoRandomLocation();
        }
    }

}
