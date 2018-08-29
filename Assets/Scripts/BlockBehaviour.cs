using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBehaviour : MonoBehaviour {

    Level level;
    Vector3 curPos;
    float step = 1;

    public Transform[] blocks;

    [HideInInspector]
    public int transformCase = 0;

    [System.Serializable]
    public class MultiVector3
    {
        public Vector3[] blockTransforms = new Vector3[4];
    }
    public MultiVector3[] rotations;

    void Start () {
        level= GameObject.FindGameObjectWithTag("Level").GetComponent<Level>();
        curPos = transform.position;
        StartCoroutine(MoveDown());
	}

    private void Update()
    {
        if (Input.GetKeyDown("left"))
        {
            transform.Translate(new Vector3(-1, 0, 0), Space.World);

        }
        else if (Input.GetKeyDown("right"))
        {
            transform.Translate(new Vector3(1, 0, 0), Space.World);
        }
        else if (Input.GetKeyDown("up"))
        {
            RotateBlocks();
        }
        else if (Input.GetKeyDown("down"))
        {
            //Move faster
        }
    }

    void RotateBlocks()
    {
        for(int j = 0; j < rotations[transformCase].blockTransforms.Length; j++)
        {
            blocks[j].transform.localPosition = rotations[transformCase].blockTransforms[j];
        }
        transformCase++;
        if(transformCase >= rotations.Length)
        {
            transformCase = 0;
        }
    }

    IEnumerator MoveDown()
    {
        while (true)
        {
            yield return new WaitForSeconds(level.moveTime);
            curPos.y = step * (-1);
            transform.Translate(curPos, Space.World);
        }
    }
}
