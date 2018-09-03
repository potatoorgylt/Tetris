using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBlocks : MonoBehaviour {

    public GameObject[] tetrisBlocks;
    int randomBlock;
	// Use this for initialization
	void Start () {
        CreateBlock();
	}

    public void CreateBlock()
    {
        randomBlock = Random.Range(0, tetrisBlocks.Length);
        var newBlock = Instantiate(tetrisBlocks[randomBlock], transform.position, Quaternion.identity);
        newBlock.transform.parent = gameObject.transform;
    }
}
