using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieController : MonoBehaviour {

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    public Sprite deadSprite;
    public Sprite aliveSprite;

    private Color color;

    //Grid variables
    public int xPos, yPos;
    public GameObject gridSystem;
    private Grid grid;
    private Node desiredNode;
    private int gridSizeX, gridSizeY;

    public GameObject scoreSystemGameObject;
    private ScoreSystem scoreSystem;

    //Zombie variables
    public bool isDead;

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
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        color = spriteRenderer.color;
        scoreSystem = scoreSystemGameObject.GetComponent<ScoreSystem>();
        grid = gridSystem.GetComponent<Grid>();

        gridSizeX = grid.GetGridSizeX();
        gridSizeY = grid.GetGridSizeY();
        //Set inital position
        desiredNode = grid.NodeFromGridPosition(xPos, yPos);
        transform.position = new Vector3(desiredNode.worldPosition.x, desiredNode.worldPosition.y, 0);

        if (!grid.NodeFromGridPosition(xPos, yPos).walkable)
        {
            GoRandomLocation();
        }

        //Set initial value of randomnumber to -1 so that the first run of Movement() will get a previous action number of -1
        randomNumber = -1f;
        StartCoroutine(AddScore());
    }

    private void GoRandomLocation()
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


    private void Update()
    {
        if (isDead)
        {
            spriteRenderer.sprite = deadSprite;
            animator.runtimeAnimatorController = null;
        }
        else
        {
            spriteRenderer.sprite = aliveSprite;
            if (!waitingToMove)
            {
                preRandomNumber = randomNumber;
                randomNumber = Random.Range(0f, 1f);
                Movement(randomNumber, preRandomNumber);
            }
            
        }
        
    }
    void Movement(float number, float previousNumber)
    {
        //left
        if (number < 0.25f && !waitingToMove && grid.NodeFromGridPosition(xPos - 1, yPos).walkable && grid.NodeFromGridPosition(xPos - 1, yPos).zWalkable)
        {

                xPos = Mathf.Clamp(xPos - 1, 1, gridSizeX - 1);
                StartCoroutine(WaitToMove());
                animator.runtimeAnimatorController = (RuntimeAnimatorController)Resources.Load("Zombie/Zombie_left up", typeof(RuntimeAnimatorController));
                MoveToGridPoint(xPos, yPos);

        }
        //up
        if (number >= 0.25f && number < 0.5f && !waitingToMove && grid.NodeFromGridPosition(xPos, yPos + 1).walkable && grid.NodeFromGridPosition(xPos, yPos + 1).zWalkable)
        {

                yPos = Mathf.Clamp(yPos + 1, 1, gridSizeY - 1);
                StartCoroutine(WaitToMove());
                MoveToGridPoint(xPos, yPos);

        }
        //down
        if (number >= 0.5f && number < 0.75f && !waitingToMove && grid.NodeFromGridPosition(xPos, yPos - 1).walkable && grid.NodeFromGridPosition(xPos, yPos - 1).zWalkable)
        {

                yPos = Mathf.Clamp(yPos - 1, 1, gridSizeY - 1);
                StartCoroutine(WaitToMove());
                MoveToGridPoint(xPos, yPos);

        }
        //right
        if (number >= 0.75f && !waitingToMove && grid.NodeFromGridPosition(xPos + 1, yPos).walkable && grid.NodeFromGridPosition(xPos + 1, yPos).zWalkable)
        {

                xPos = Mathf.Clamp(xPos + 1, 1, gridSizeX - 1);
                StartCoroutine(WaitToMove());
                animator.runtimeAnimatorController = (RuntimeAnimatorController)Resources.Load("Zombie/Zombie_right_up", typeof(RuntimeAnimatorController));
                MoveToGridPoint(xPos, yPos);
        }
    }
    public void SpeedUpZomb()
    {
        
        StartCoroutine(SpeedUp());
    }
    IEnumerator SpeedUp()
    {
        color.g = 0.5f;
        color.b = 0.5f;
        spriteRenderer.color = color;
        slowness = slowness / 2;
        yield return new WaitForSeconds(3);
        color.b = 1f;
        color.g = 1f;
        spriteRenderer.color = color;
        slowness = slowness * 2;
    }

    IEnumerator AddScore()
    {
        while (true)
        {
            if (isDead)
            {
                scoreSystem.SetP1Score(scoreSystem.GetP1Score() + 1);
            }
            else
            {
                scoreSystem.SetP2Score(scoreSystem.GetP2Score() + 1);
            }
            yield return new WaitForSeconds(5);
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
