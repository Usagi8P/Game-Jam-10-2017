﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public int xPos, yPos;
    public GameObject gridSystem;
    private Grid grid;
    Node desiredNode;
    private int gridSizeX, gridSizeY;

    public float slowness;
    private bool waitingToMove;
    public float timeBetweenNodes;

    private Vector3 StartingPostition;
    private Vector3 DestinationPosition;

    void Start()
    {
        grid = gridSystem.GetComponent<Grid>();

        gridSizeX = grid.GetGridSizeX();
        gridSizeY = grid.GetGridSizeY();
        //Set inital position
        desiredNode = grid.NodeFromGridPosition(xPos, yPos);
        transform.position = new Vector3(desiredNode.worldPosition.x, desiredNode.worldPosition.y, 0);
    }

    


    private void FixedUpdate()
    {
        //Get inputs
        if (Input.GetKey(KeyCode.A) && !waitingToMove && grid.NodeFromGridPosition(xPos - 1, yPos).walkable) { 
            xPos = Mathf.Clamp(xPos - 1, 0, gridSizeX-1);
            StartCoroutine(WaitToMove());
            MoveToGridPoint(xPos, yPos);
        }

        else if (Input.GetKey(KeyCode.W) && !waitingToMove && grid.NodeFromGridPosition(xPos, yPos + 1).walkable)
        {
            yPos = Mathf.Clamp(yPos + 1, 0, gridSizeY-1);
            StartCoroutine(WaitToMove());
            MoveToGridPoint(xPos, yPos);
        }

        else if (Input.GetKey(KeyCode.S) && !waitingToMove && grid.NodeFromGridPosition(xPos, yPos - 1).walkable)
        {
            yPos = Mathf.Clamp(yPos - 1, 0, gridSizeY-1);
            StartCoroutine(WaitToMove());
            MoveToGridPoint(xPos, yPos);
        }

        else if (Input.GetKey(KeyCode.D) && !waitingToMove && grid.NodeFromGridPosition(xPos+1, yPos).walkable)
        {
            xPos = Mathf.Clamp(xPos + 1, 0, gridSizeX-1);
            StartCoroutine(WaitToMove());
            MoveToGridPoint(xPos, yPos);
        }

        
    }

    IEnumerator WaitToMove()
    {
        waitingToMove = true;
        yield return new WaitForSeconds(slowness);
        waitingToMove = false;
    }

    IEnumerator WaitAndMove(Vector3 posA, Vector3 posB)
    {
        float startTime = Time.time; // Time.time contains current frame time, so remember starting point
        
        while (Time.time - startTime <= timeBetweenNodes)
        { // until one second passed
            Debug.Log(Time.time - startTime);
            transform.position = Vector3.Lerp(posA, posB, (Time.time - startTime)/timeBetweenNodes); // lerp from A to B in one second
            yield return 1; // wait for next frame
        }
        transform.position = new Vector3(desiredNode.worldPosition.x, desiredNode.worldPosition.y, 0);
    }

        void MoveToGridPoint(int x, int y)
    {

        desiredNode = grid.NodeFromGridPosition(x, y);
        StartCoroutine(WaitAndMove(transform.position, new Vector3(desiredNode.worldPosition.x, desiredNode.worldPosition.y, 0)));
        
    }


}
