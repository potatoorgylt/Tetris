using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBehaviour : MonoBehaviour {
    SpawnBlocks spawn;
    TetrisLevel level;
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

    void Start () {
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
        if (blocks == null)
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
        //for (int j = 0; j < rotations[curRotation].blockPositions.Length; j++)
        //level.TakeSpace(transform.position + rotations[curRotation].blockPositions[j]);
        for (int i = 0; i < blocks.Length; i++)
            level.TakeSpace(blocks[i]);
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
