using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour {
    //Grid variables
    public int xPos, yPos;
    public GameObject gridSystem;
    private Grid grid;
    private Node desiredNode;
    private int gridSizeX, gridSizeY;

    public GameObject scoreSystemGameObject;




    //Movement variables
    public float slowness;
    private bool waitingToMove;
    public float timeBetweenNodes;
    private float randomNumber;
    private float preRandomNumber;

    //Used to lerp player to desired position
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

        //Set initial value of randomnumber to -1 so that the first run of Movement() will get a previous action number of -1
        randomNumber = -1f;
    }




    private void Update()
    {
        if (!waitingToMove)
        {
            preRandomNumber = randomNumber;
            randomNumber = Random.Range(0f, 1f);
            Movement(randomNumber, preRandomNumber);
        }
    }
    void Movement(float number, float previousNumber)
    {
        //left
        if (number < 0.25f && !waitingToMove)
        {

            xPos = Mathf.Clamp(xPos - 1, 1, gridSizeX - 1);
            StartCoroutine(WaitToMove());
            MoveToGridPoint(xPos, yPos);

        }
        //up
        if (number >= 0.25f && number < 0.5f && !waitingToMove)
        {

            yPos = Mathf.Clamp(yPos + 1, 1, gridSizeY - 1);
            StartCoroutine(WaitToMove());
            MoveToGridPoint(xPos, yPos);

        }
        //down
        if (number >= 0.5f && number < 0.75f && !waitingToMove)
        {

            yPos = Mathf.Clamp(yPos - 1, 1, gridSizeY - 1);
            StartCoroutine(WaitToMove());
            MoveToGridPoint(xPos, yPos);

        }
        //right
        if (number >= 0.75f && !waitingToMove)
        {

            xPos = Mathf.Clamp(xPos + 1, 1, gridSizeX - 1);
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
