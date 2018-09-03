using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisLevel : MonoBehaviour {

    public float moveTime = 1.0f;
    float timePassed = 0f;

    public int gridWidth;
    public int gridLength;

    public Vector3[,] grid;
    //[HideInInspector]
    public List<Transform> gridTaken;

    [HideInInspector]
    public float bottom;
    [HideInInspector]
    public float leftBorder;
    [HideInInspector]
    public float rightBorder;
    [HideInInspector]
    public float top;

    private void Start()
    {
        GetLevelEdges();
        InitializeGrid();
    }

    void GetLevelEdges()
    {
        top = transform.position.y + gridLength / 2;
        bottom = transform.position.y + gridLength / 2 * (-1);
        leftBorder = transform.position.x + gridWidth / 2 * (-1);
        rightBorder = transform.position.x + gridWidth / 2;
    }

    void InitializeGrid()
    {
        float posX = transform.position.x + gridWidth / 2 * (-1);
        float posY = transform.position.y + gridLength / 2 * (-1);

        grid = new Vector3[gridLength, gridWidth];

        for (int i = 0; i < gridLength; i++)
        {
            for (int j = 0; j < gridWidth-1; j++)
            {
                posX++;
                grid[i, j] = new Vector3(posX, posY, 0);
                //Debug.Log(grid[i, j]);
            }
            posX = transform.position.x + gridWidth / 2 * (-1);
            posY++;
        }
    }

    public void TakeSpace(Transform space)
    {
        gridTaken.Add(space);
        
    }
    private void Update()
    {
        timePassed += Time.deltaTime;
        if (timePassed >= moveTime)
        {
            RowFull();
            timePassed = 0f;
        }
    }

    void RowFull()
    {
        int i = 0;
        List<int> rowFull = new List<int>();
        float bottomToTop = bottom - 0.5f;
        Debug.Log("...rowFull function...");
        while (bottomToTop < top)
        {
            for (int j = 0; j < gridTaken.Count; j++)
            {
                rowFull.Add(0);
                Debug.Log(bottomToTop);
                if(gridTaken[j].position.y == bottomToTop)
                {
                    rowFull[i]++;
                    Debug.Log("rowFull[" + j + "]: " + rowFull[j]);
                    if(rowFull[j] >= gridWidth-1)
                    {
                        Destroy(gridTaken[j]);
                        gridTaken.Remove(gridTaken[j]);
                    }
                }
            }
            bottomToTop++;
            i++;
        }
    }
}
