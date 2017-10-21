using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1Controller : MonoBehaviour {

    private Animator animator;

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
    private GameObject ghost;

    public GameObject scoreSystemGameObject;
    private ScoreSystem scoreSystem;
    public float scoreSubtractionRate;

    //Used to lerp player to desired position
    private Vector3 StartingPostition;
    private Vector3 DestinationPosition;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();

        scoreSystem = scoreSystemGameObject.GetComponent<ScoreSystem>();
        ghost = GameObject.FindGameObjectWithTag("ghost");

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
        TouchingGhost();
        CastSpellOnZombie();
        //Get inputs
        if (Input.GetKey(KeyCode.A) && !waitingToMove && grid.NodeFromGridPosition(xPos - 1, yPos).walkable) { 
            xPos = Mathf.Clamp(xPos - 1, 0, gridSizeX-1);
            animator.runtimeAnimatorController = (RuntimeAnimatorController)Resources.Load("Hunter/Hunter_4", typeof(RuntimeAnimatorController));
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
            animator.runtimeAnimatorController = (RuntimeAnimatorController)Resources.Load("Hunter/Hunter_0", typeof(RuntimeAnimatorController));
            StartCoroutine(WaitToMove());
            MoveToGridPoint(xPos, yPos);
        }
    }

    private void TouchingGhost()
    {
        if (ghost.GetComponent<GhostController>().xPos == xPos && ghost.GetComponent<GhostController>().yPos == yPos)
        {
            StartCoroutine(SubtractScore());
        }
    }

    private void CastSpellOnZombie()
    {
        foreach (GameObject zom in zombies)
        {
            if (zom.GetComponent<ZombieController>().xPos == xPos && zom.GetComponent<ZombieController>().yPos == yPos)
            {
                StartCoroutine(CastAnimation(animator.runtimeAnimatorController));
                grid.NodeFromGridPosition(xPos, yPos).zWalkable = false;
                Debug.Log(grid.NodeFromGridPosition(xPos, yPos).zWalkable);
                zom.GetComponent<ZombieController>().isDead = true;
            }
        }
    }

    IEnumerator CastAnimation(RuntimeAnimatorController previousAnimation)
    {
        animator.runtimeAnimatorController = (RuntimeAnimatorController)Resources.Load("Hunter/Hunter_2", typeof(RuntimeAnimatorController));
        yield return new WaitForSeconds(0.5f);
        animator.runtimeAnimatorController = previousAnimation;
    }
    IEnumerator SubtractScore()
    {
        scoreSystem.SetP1Score(scoreSystem.GetP1Score() - 1);
        yield return new WaitForSeconds(scoreSubtractionRate);
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
