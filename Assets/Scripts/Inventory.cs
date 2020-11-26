using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<Block> allBlocks = new List<Block>();
    public Block[] hotbar = new Block[10];

    public Block currentBlock;
    // Start is called before the first frame update
    void Start()
    {
        currentBlock = hotbar[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            currentBlock = hotbar[9];
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentBlock = hotbar[0];
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentBlock = hotbar[1];
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentBlock = hotbar[2];
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            currentBlock = hotbar[3];
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            currentBlock = hotbar[4];
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            currentBlock = hotbar[5];
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            currentBlock = hotbar[6];
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            currentBlock = hotbar[7];
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            currentBlock = hotbar[8];
        }
    }
}
