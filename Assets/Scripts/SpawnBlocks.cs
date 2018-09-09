using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnBlocks : MonoBehaviour {

    public GameObject[] tetrisBlocks;
    int randomBlock;
    public Sprite[] nextBlockSpr;
    public Image blockImg;

	// Use this for initialization
	void Start () {
        randomBlock = Random.Range(0, tetrisBlocks.Length);
        CreateBlock();
	}

    public void CreateBlock()
    {    
        var newBlock = Instantiate(tetrisBlocks[randomBlock], transform.position, Quaternion.identity);
        newBlock.transform.parent = gameObject.transform;
        randomBlock = Random.Range(0, tetrisBlocks.Length);
        ShowImage(randomBlock);
    }

    void ShowImage(int index)
    {
        blockImg.sprite = nextBlockSpr[index];
        switch (index)
        {
            default:
                blockImg.transform.localScale = new Vector3(0.36f, 0.19f, 0.2f);
                break;
            case 0:
                blockImg.transform.localScale = new Vector3(0.45f, 0.135f, 0.15f);
                break;
            case 3:
                blockImg.transform.localScale = new Vector3(0.24f, 0.19f, 0.21f);
                break;
        }
        
    }
}
