using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacedBlocks : MonoBehaviour {

    public List<GameObject> blocksObj = new List<GameObject>();
    public float blockPosY;

    public PlacedBlocks()
    {
        for (int i = 0; i < blocksObj.Count; i++)
        {
            blockPosY = blocksObj[i].transform.position.y;
        }
    }
    public void MoveDown()
    {

    }

    public void Destroy(int index)
    {
        Destroy(blocksObj[index]);
        blocksObj.Remove(blocksObj[index]);
    }

    public void ClaimSpace(GameObject placedBlock)
    {
        blocksObj.Add(placedBlock);
        //blockPosY = blocksObj[i].transform.position.y;
    }
}
