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
    public List<GameObject> gridTaken;

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

    public void TakeSpace(GameObject space)
    {
        gridTaken.Add(space);
        
    }

    public void CheckLineFull()
    {
        int i = 0;
        List<float> fullRowPositions = new List<float>();
        List<int> rowFull = new List<int>();
        float bottomToTop = bottom - 0.5f + 1.0f;
        Debug.Log("...rowFull function...");

        while (bottomToTop < top)
        {
            rowFull.Add(0);
            for (int j = 0; j < gridTaken.Count; j++)
            {
                if (gridTaken[j].transform.position.y == bottomToTop)
                {
                    rowFull[i]++;
                    if (rowFull[i] >= gridWidth)
                    {
                        fullRowPositions.Add(bottomToTop);
                    }
                }
            }
            Debug.Log("rowFull[" + i + "]: " + rowFull[i]);
            Debug.Log(bottomToTop);
            bottomToTop++;
            i++;
        }
        //Remove the objects
        for (int j = 0; j < fullRowPositions.Count; j++)
        {
            for(int k = 0; k < gridTaken.Count; k++)
            {
                if (gridTaken[k].transform.position.y == fullRowPositions[j])
                {
                    Destroy(gridTaken[k]);
                }
            }
        }
    }
}
