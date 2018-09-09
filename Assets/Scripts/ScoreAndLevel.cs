using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreAndLevel : MonoBehaviour {

    public Text scoreText;
    public Text levelText;
    private int totalScore = 0;
    public int amountForNextLvl = 1000;
    private int totalAmountForNextLvl;

    private int curLevel = 1;
    TetrisLevel tetrisLevel;

    private void Start()
    {
        tetrisLevel = GetComponent<TetrisLevel>();
        scoreText.text = "Score: " + totalScore;
        levelText.text = "Level: " + curLevel;
        totalAmountForNextLvl = amountForNextLvl;
    }

    public void AddScore(int amount)
    {
        totalScore += amount;
        scoreText.text = "Score: " + totalScore;
        if (totalScore >= totalAmountForNextLvl)
        {
            LevelUp();
            totalAmountForNextLvl += amountForNextLvl;
        }
    }

    public void LevelUp()
    {
        curLevel++;
        levelText.text = "Level: " + curLevel;
        if (curLevel <= 9)
        {
            tetrisLevel.moveTime -= 0.1f;
        }
        else
        {
            tetrisLevel.moveTime -= 0.005f;
        }
    }
}
