using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour {

    public float moveTime = 1;

    public int gridWidth;
    public int gridLength;

    public Vector3[,] grid;

    private void Start()
    {
        InitializeGrid();
    }

    void InitializeGrid()
    {
        float posX = gridWidth / 2 * (-1);
        float posY = gridLength / 2 * (-1);

        grid = new Vector3[gridLength, gridWidth];

        for (int i = 0; i < gridLength; i++)
        {
            for (int j = 0; j < gridWidth-1; j++)
            {
                posX++;
                grid[i, j] = new Vector3(posX, posY, 0);
                Debug.Log(grid[i, j]);
            }
            posX = gridWidth / 2 * (-1);
            posY++;
        }
    }
}
