using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2Controller : MonoBehaviour {

    //Grid variables
    public int xPos, yPos;
    public GameObject gridSystem;
    private Grid grid;
    Node desiredNode;
    private int gridSizeX, gridSizeY;

    //Movement variables
    public float slowness;
    private bool waitingToMove;
    public float timeBetweenNodes;

    private GameObject[] zombies;

    //Used to lerp player to desired position
    private Vector3 StartingPostition;
    private Vector3 DestinationPosition;

    void Start()
    {
        zombies = GameObject.FindGameObjectsWithTag("zombie");

        grid = gridSystem.GetComponent<Grid>();

        gridSizeX = grid.GetGridSizeX();
        gridSizeY = grid.GetGridSizeY();
        //Set inital position
        desiredNode = grid.NodeFromGridPosition(xPos, yPos);
        transform.position = new Vector3(desiredNode.worldPosition.x, desiredNode.worldPosition.y, 0);
    }




    private void Update()
    {

        KillZombie();
        //Get inputs
        if (Input.GetKey("left") && !waitingToMove && grid.NodeFromGridPosition(xPos - 1, yPos).walkable)
        {
            xPos = Mathf.Clamp(xPos - 1, 0, gridSizeX - 1);
            StartCoroutine(WaitToMove());
            MoveToGridPoint(xPos, yPos);
        }

        if (Input.GetKey("up") && !waitingToMove && grid.NodeFromGridPosition(xPos, yPos + 1).walkable)
        {
            yPos = Mathf.Clamp(yPos + 1, 0, gridSizeY - 1);
            StartCoroutine(WaitToMove());
            MoveToGridPoint(xPos, yPos);
        }

        if (Input.GetKey("down") && !waitingToMove && grid.NodeFromGridPosition(xPos, yPos - 1).walkable)
        {
            yPos = Mathf.Clamp(yPos - 1, 0, gridSizeY - 1);
            StartCoroutine(WaitToMove());
            MoveToGridPoint(xPos, yPos);
        }

        if (Input.GetKey("right") && !waitingToMove && grid.NodeFromGridPosition(xPos + 1, yPos).walkable)
        {
            xPos = Mathf.Clamp(xPos + 1, 0, gridSizeX - 1);
            StartCoroutine(WaitToMove());
            MoveToGridPoint(xPos, yPos);
        }
    }

    private void KillZombie()
    {
        foreach (GameObject zom in zombies)
        {
            if (zom.GetComponent<ZombieController>().xPos == xPos && zom.GetComponent<ZombieController>().yPos == yPos)
            {
                grid.NodeFromGridPosition(xPos, yPos).zWalkable = true;
                Debug.Log(grid.NodeFromGridPosition(xPos, yPos).zWalkable);
                zom.GetComponent<ZombieController>().isDead = false;
            }
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
            transform.position = Vector3.Lerp(posA, posB, (Time.time - startTime) / timeBetweenNodes); // lerp from A to B in one second
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
