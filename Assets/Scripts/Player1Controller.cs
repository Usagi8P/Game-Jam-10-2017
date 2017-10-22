using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player1Controller : MonoBehaviour {
    public int timeBetweenCasts;
    private Animator animator;
    public Text manaText;
    public GameObject manaCrystal;
    private bool waitingToCast;

    private int mana;

    private Color color;
    private SpriteRenderer spriteRenderer;

    public GameObject player2;

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
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        color = spriteRenderer.color;

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
        manaText.text = "Mana : " + mana.ToString();
        TouchingManaCrystal();
        TouchingGhost();
        CastSpellOnZombie();
        //Get inputs
        if (Input.GetKey(KeyCode.A) && !waitingToMove && grid.NodeFromGridPosition(xPos - 1, yPos).walkable) { 
            xPos = Mathf.Clamp(xPos - 1, 0, gridSizeX-1);
            StartCoroutine(WaitToMove());
            animator.runtimeAnimatorController = (RuntimeAnimatorController)Resources.Load("Hunter/Hunter_4", typeof(RuntimeAnimatorController));
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
            animator.runtimeAnimatorController = (RuntimeAnimatorController)Resources.Load("Hunter/Hunter_0", typeof(RuntimeAnimatorController));
            MoveToGridPoint(xPos, yPos);
        }

        if (Input.GetKey(KeyCode.E) && mana > 0 && !waitingToCast)
        {
            StartCoroutine(WaitToCast());
            player2.GetComponent<Player2Controller>().MakeFrozen();
            mana--;
        }
    }   

    private void TouchingGhost()
    {
        if (ghost.GetComponent<GhostController>().xPos == xPos && ghost.GetComponent<GhostController>().yPos == yPos)
        {
            StartCoroutine(SubtractScore());
        }
    }

    private void TouchingManaCrystal()
    {
        if (manaCrystal.GetComponent<ManaCrystal>().xPos == xPos && manaCrystal.GetComponent<ManaCrystal>().yPos == yPos)
        {
            manaCrystal.GetComponent<ManaCrystal>().GoRandomLocation();
            mana++;
        }
    }

    private void CastSpellOnZombie()
    {
        foreach (GameObject zom in zombies)
        {
            if (zom.GetComponent<ZombieController>().xPos == xPos && zom.GetComponent<ZombieController>().yPos == yPos && zom.GetComponent<ZombieController>().isDead == false)
            {
                if (animator.runtimeAnimatorController != (RuntimeAnimatorController)Resources.Load("Hunter/Hunter_2", typeof(RuntimeAnimatorController)))
                {
                    StartCoroutine(CastAnimation(animator.runtimeAnimatorController));
                }
                grid.NodeFromGridPosition(xPos, yPos).zWalkable = false;
                Debug.Log(grid.NodeFromGridPosition(xPos, yPos).zWalkable);
                zom.GetComponent<ZombieController>().isDead = true;
            }
        }
    }
    IEnumerator WaitToCast()
    {
        waitingToCast = true;
        yield return new WaitForSeconds(timeBetweenCasts);
        waitingToCast = false;
    }

    IEnumerator CastAnimation(RuntimeAnimatorController previousAnimation)
    {
        animator.runtimeAnimatorController = (RuntimeAnimatorController)Resources.Load("Hunter/Hunter_2", typeof(RuntimeAnimatorController));
        yield return new WaitForSeconds(0.2f);
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
