using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBehaviour : MonoBehaviour {
    ScoreAndLevel scoreAndLevel;
    SpawnBlocks spawn;
    TetrisLevel level;

    public int addScoreAmount = 50;

    float step = 1;
    bool moving = true;

    public GameObject[] blocks;

    public int nextRotation = 0;
    private int curRotation = 0;

    [System.Serializable]
    public class MultiVector3
    {
        public Vector3[] blockPositions = new Vector3[4];
    }
    public MultiVector3[] rotations;

    float moveSpeed;

    float lrKeyDelay = 0.1f;
    float lrTimePassed = 0f;

    float keyDelay = 0.05f;  // 1 second
    float timePassed = 0f;

    //Mobile
    private Vector3 fp;   //First touch position
    private Vector3 lp;   //Last touch position
    private float dragDistance;  //minimum distance for a swipe to be registered
    //

    void Start () {
        //Mobile
        dragDistance = Screen.height * 5 / 100; //dragDistance is 5% height of the screen
        //

        scoreAndLevel = GameObject.FindGameObjectWithTag("Level").GetComponent<ScoreAndLevel>();
        level = GameObject.FindGameObjectWithTag("Level").GetComponent<TetrisLevel>();
        spawn = transform.parent.GetComponent<SpawnBlocks>();

        moveSpeed = level.moveTime;
    }

    private void Update()
    {
        if (moving)
        {
            timePassed += Time.deltaTime;
            lrTimePassed += Time.deltaTime;

            //Mobile
            if (Input.touchCount == 1) // user is touching the screen with a single touch
            {
                Touch touch = Input.GetTouch(0); // get the touch
                if (touch.phase == TouchPhase.Began) //check for the first touch
                {
                    fp = touch.position;
                    lp = touch.position;
                }
                else if (touch.phase == TouchPhase.Moved) // update the last position based on where they moved
                {
                    lp = touch.position;

                    //Check if drag distance is greater than 20% of the screen height
                    if (Mathf.Abs(lp.x - fp.x) > dragDistance || Mathf.Abs(lp.y - fp.y) > dragDistance)
                    {//It's a drag
                     //check if the drag is vertical or horizontal
                        if (Mathf.Abs(lp.x - fp.x) > Mathf.Abs(lp.y - fp.y))
                        {   //If the horizontal movement is greater than the vertical movement...
                            if ((lp.x > fp.x))  //If the movement was to the right)
                            {   //Right swipe
                                Debug.Log("Right Drag");
                                if (lrTimePassed >= lrKeyDelay)
                                {
                                    lrTimePassed = 0f;
                                    MoveBlock(new Vector3(1, 0, 0));
                                }
                            }
                            else
                            {   //Left swipe
                                Debug.Log("Left Drag");
                                if (lrTimePassed >= lrKeyDelay)
                                {
                                    lrTimePassed = 0f;
                                    MoveBlock(new Vector3(-1, 0, 0));
                                }
                            }
                        }
                        else
                        {   //the vertical movement is greater than the horizontal movement
                            if (lp.y > fp.y)  //If the movement was up
                            {   //Up swipe
                                Debug.Log("Up Drag");
                            }
                            else
                            {   //Down swipe
                                Debug.Log("Down Drag");
                                if (timePassed >= keyDelay)
                                {
                                    timePassed = 0f;
                                    MoveDown();
                                }
                            }
                        }
                    }
                }
                else if (touch.phase == TouchPhase.Ended) //check if the finger is removed from the screen
                {
                    lp = touch.position;  //last touch position. Ommitted if you use list

                    if (Mathf.Abs(lp.x - fp.x) > dragDistance || Mathf.Abs(lp.y - fp.y) > dragDistance){}
                    else
                    {
                        RotateBlocks();
                    }
                }
            }
            //EndOfMobile

            if (Input.GetKey("left"))
            {
                if (lrTimePassed >= lrKeyDelay)
                {
                    lrTimePassed = 0f;
                    MoveBlock(new Vector3(-1, 0, 0));
                }
            }
            else if (Input.GetKey("right"))
            {
                if (lrTimePassed >= lrKeyDelay)
                {
                    lrTimePassed = 0f;
                    MoveBlock(new Vector3(1, 0, 0));
                }
            }
            else if (Input.GetKeyDown("up"))
            {
                RotateBlocks();
            }

            if (Input.GetKey("down"))
            {
                if (timePassed >= keyDelay)
                {
                    timePassed = 0f;
                    MoveDown();
                }
            }
            else
            {
                if (timePassed >= moveSpeed)
                {
                    timePassed = 0f;
                    MoveDown();
                }
            }
        }
        if (transform.childCount == 0)
            Destroy(gameObject);
    }

    void MoveBlock(Vector3 direction)
    {
        bool blockMovement = false;
        for (int i = 0; i < rotations[curRotation].blockPositions.Length; i++)
        {
            if (direction.x < 0)
            {

                for (int j = 0; j < level.gridTaken.Count; j++)
                {
                    if(level.gridTaken[j] != null)
                        if(transform.position.y + rotations[curRotation].blockPositions[i].y == level.gridTaken[j].transform.position.y)
                        {
                            if (transform.position.x + rotations[curRotation].blockPositions[i].x == level.gridTaken[j].transform.position.x + 1) //Check for left obstacle
                            {
                                blockMovement = true;
                            }
                        }
                }
                if (transform.position.x + rotations[curRotation].blockPositions[i].x < level.leftBorder + 1) //Check for left border
                    blockMovement = true;
            }
            else if (direction.x > 0)
            {
                for (int j = 0; j < level.gridTaken.Count; j++)
                {
                    if (level.gridTaken[j] != null)
                    if (transform.position.y + rotations[curRotation].blockPositions[i].y == level.gridTaken[j].transform.position.y)
                    {
                        if (transform.position.x + rotations[curRotation].blockPositions[i].x == level.gridTaken[j].transform.position.x - 1) //Check for left obstacle
                        {
                            blockMovement = true;
                        }
                    }
                }
                if (transform.position.x + rotations[curRotation].blockPositions[i].x > level.rightBorder - 1)
                    blockMovement = true;
            }
        }
        if(blockMovement == false)
        {
            transform.Translate(direction, Space.World);
            blockMovement = false;
        }
    }

    void RotateBlocks()
    {
        int toRotate = 0;
        bool canRotate = false;
        bool canRotateBlockCheck = true;

        for (int i = 0; i < rotations[nextRotation].blockPositions.Length; i++)
        {
            if (transform.position.x + rotations[nextRotation].blockPositions[i].x > level.leftBorder && transform.position.x + rotations[nextRotation].blockPositions[i].x < level.rightBorder)
            {
                if (transform.position.y + rotations[nextRotation].blockPositions[i].y > level.bottom)
                {
                    if (level.gridTaken.Count != 0)
                    {
                        for (int j = 0; j < level.gridTaken.Count; j++)
                        {
                            if (level.gridTaken[j] != null)
                                if (transform.position + rotations[nextRotation].blockPositions[i] == level.gridTaken[j].transform.position)
                                {
                                    canRotateBlockCheck = false;
                                }
                        }
                    }
                    toRotate++;
                }
            }
        }

        if (toRotate >= rotations[nextRotation].blockPositions.Length)
            canRotate = true;

        if (canRotate == true && canRotateBlockCheck == true)
        {
            for (int j = 0; j < rotations[nextRotation].blockPositions.Length; j++)
            {
                blocks[j].transform.localPosition = rotations[nextRotation].blockPositions[j];
            }

            nextRotation++;
            curRotation = nextRotation - 1;
            if (nextRotation >= rotations.Length)
            {
                nextRotation = 0;
            }
            if (curRotation < 0)
                curRotation = 0;
        }
    }

    void MoveDown()
    {
        level.gridTaken.RemoveAll(GameObject => GameObject == null); //clear list to avoid errors
        bool onGround = CheckIfBottom();
        bool blockBelow = false;
        if (!onGround)
            blockBelow = CheckForBlockBelow();
        if (!onGround && !blockBelow)
            transform.Translate(new Vector3(0, step * (-1), 0), Space.World);
    }

    void PlaceBlock()
    {
        moving = false;
        scoreAndLevel.AddScore(addScoreAmount);
        for (int i = 0; i < blocks.Length; i++)
        {
            level.CheckHeight(blocks[i]);
            level.TakeSpace(blocks[i]);
        }
        level.CheckLineFull();
        spawn.CreateBlock();
    }

    bool CheckIfBottom()
    {
        for (int i = 0; i < rotations[curRotation].blockPositions.Length; i++)
        {
            if (transform.position.y + rotations[curRotation].blockPositions[i].y < level.bottom + 1)
            {
                PlaceBlock();
                return true;
            }
        }

        return false;
    }

    bool CheckForBlockBelow()
    {
        bool canPlace = false;
        for (int i = 0; i < rotations[curRotation].blockPositions.Length; i++)
        {
            for(int j = 0; j < level.gridTaken.Count; j++)
            {
                if (level.gridTaken[j] != null)
                    if (transform.position.y + rotations[curRotation].blockPositions[i].y == level.gridTaken[j].transform.position.y + 1)
                    {
                        if (transform.position.x + rotations[curRotation].blockPositions[i].x == level.gridTaken[j].transform.position.x)
                        {
                            canPlace = true;
                            break;
                        }
                    }
            }
        }
        if(canPlace == true)
            PlaceBlock();
        return canPlace;
    }
}
