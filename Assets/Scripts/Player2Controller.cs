using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player2Controller : MonoBehaviour {
    public int timeBetweenCasts;
    private Animator animator;
    public Text manaText;
    public GameObject manaCrystal;
    private int mana;
    public bool frozen;
    private bool waitingToCast;

    private Color color;
    private SpriteRenderer spriteRenderer;

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

        zombies = GameObject.FindGameObjectsWithTag("zombie");

        scoreSystem = scoreSystemGameObject.GetComponent<ScoreSystem>();
        ghost = GameObject.FindGameObjectWithTag("ghost");

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
        KillZombie();
        //Get inputs
        if (Input.GetKey("left") && !waitingToMove && grid.NodeFromGridPosition(xPos - 1, yPos).walkable && !frozen)
        {
            xPos = Mathf.Clamp(xPos - 1, 0, gridSizeX - 1);
            StartCoroutine(WaitToMove());
            animator.runtimeAnimatorController = (RuntimeAnimatorController)Resources.Load("Necromancer/Necromancer_4", typeof(RuntimeAnimatorController));
            MoveToGridPoint(xPos, yPos);
        }

        if (Input.GetKey("up") && !waitingToMove && grid.NodeFromGridPosition(xPos, yPos + 1).walkable && !frozen)
        {
            yPos = Mathf.Clamp(yPos + 1, 0, gridSizeY - 1);
            StartCoroutine(WaitToMove());
            MoveToGridPoint(xPos, yPos);
        }

        if (Input.GetKey("down") && !waitingToMove && grid.NodeFromGridPosition(xPos, yPos - 1).walkable && !frozen)
        {
            yPos = Mathf.Clamp(yPos - 1, 0, gridSizeY - 1);
            StartCoroutine(WaitToMove());
            MoveToGridPoint(xPos, yPos);
        }

        if (Input.GetKey("right") && !waitingToMove && grid.NodeFromGridPosition(xPos + 1, yPos).walkable && !frozen)
        {
            xPos = Mathf.Clamp(xPos + 1, 0, gridSizeX - 1);
            StartCoroutine(WaitToMove());
            animator.runtimeAnimatorController = (RuntimeAnimatorController)Resources.Load("Necromancer/Necromancer_0", typeof(RuntimeAnimatorController));
            MoveToGridPoint(xPos, yPos);
        }
        if (Input.GetKey(KeyCode.RightShift) && mana > 0 && !waitingToCast)
        {
            StartCoroutine(WaitToCast());
            foreach (GameObject zombie in zombies)
            {
                zombie.GetComponent<ZombieController>().SpeedUpZomb();
            }
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

    private void KillZombie()
    {
        foreach (GameObject zom in zombies)
        {
            if (zom.GetComponent<ZombieController>().xPos == xPos && zom.GetComponent<ZombieController>().yPos == yPos && zom.GetComponent<ZombieController>().isDead == true)
            {
                if (animator.runtimeAnimatorController != (RuntimeAnimatorController)Resources.Load("Necromancer/Necromancer_2", typeof(RuntimeAnimatorController)))//If animation is the praising the lord animation
                {
                    StartCoroutine(CastAnimation(animator.runtimeAnimatorController));
                }
                
                grid.NodeFromGridPosition(xPos, yPos).zWalkable = true;
                Debug.Log(grid.NodeFromGridPosition(xPos, yPos).zWalkable);
                zom.GetComponent<ZombieController>().isDead = false;
            }
        }
    }
    public void MakeFrozen()
    {
        StartCoroutine(Frozen());
    }

    IEnumerator Frozen()
    {
        frozen = true;
        color.r = 0.5f;
        spriteRenderer.color = color;
        yield return new WaitForSeconds(1);
        color.r = 1f;
        spriteRenderer.color = color;
        frozen = false;
    }
    IEnumerator SubtractScore()
    {
        scoreSystem.SetP2Score(scoreSystem.GetP2Score() - 1);
        yield return new WaitForSeconds(scoreSubtractionRate);
    }
    IEnumerator CastAnimation(RuntimeAnimatorController previousAnimation)
    {
        animator.runtimeAnimatorController = (RuntimeAnimatorController)Resources.Load("Necromancer/Necromancer_2", typeof(RuntimeAnimatorController));
        yield return new WaitForSeconds(0.5f);
        animator.runtimeAnimatorController = previousAnimation;
    }

    IEnumerator WaitToCast()
    {
        waitingToCast = true;
        yield return new WaitForSeconds(timeBetweenCasts);
        waitingToCast = false;
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
