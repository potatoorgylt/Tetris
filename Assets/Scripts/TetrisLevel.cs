using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TetrisLevel : MonoBehaviour {

    public float moveTime = 1.0f;

    public int gridWidth;
    public int gridLength;

    private float globalHeight;

    public GameObject gameOverPanel;

    [HideInInspector]
    public List<GameObject> gridTaken;

    [HideInInspector]
    public float bottom;
    [HideInInspector]
    public float leftBorder;
    [HideInInspector]
    public float rightBorder;
    [HideInInspector]
    public float top;

    ScoreAndLevel scoreAndLevel;

    private void Start()
    {
        scoreAndLevel = GetComponent<ScoreAndLevel>();
        GetLevelEdges();
    }

    void GetLevelEdges()
    {
        top = transform.position.y + gridLength / 2;
        bottom = transform.position.y + gridLength / 2 * (-1);
        leftBorder = transform.position.x + gridWidth / 2 * (-1);
        rightBorder = transform.position.x + gridWidth / 2;
    }

    public void TakeSpace(GameObject space)
    {
        gridTaken.Add(space);
    }

    public void CheckHeight(GameObject block)
    {
        Debug.Log(block.transform.position.y);
        if(block.transform.position.y > (gridLength/2)-2)
        {
            GameOver();
        }
    }

    public void GameOver()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0.0F;
    }

    public void Retry()
    {
        Time.timeScale = 1.0F;
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void CheckLineFull()
    {
        int i = 0;
        List<float> fullRowPositions = new List<float>();
        List<int> rowFull = new List<int>();
        float bottomToTop = bottom - 0.5f + 1.0f;

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
                    scoreAndLevel.AddScore(15);
                }
            }
        }

        //Move down above
        for (int j = fullRowPositions.Count - 1; j >= 0; j--)
        {
            for (int m = 0; m < gridTaken.Count; m++)
            {
                if (gridTaken[m].transform.position.y > fullRowPositions[j])
                {
                    gridTaken[m].transform.Translate(0, -1, 0);
                }
            }
        }
    }
}
