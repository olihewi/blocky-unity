using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Builder : MonoBehaviour
{
  public World world;
  public Inventory inventory;

  public Block airBlock;
  public float buildDistance = 5;

  public LayerMask groundLayer;
  // Update is called once per frame
  void Update()
  {
    bool leftClick = Input.GetMouseButtonDown(0);
    bool rightClick = Input.GetMouseButtonDown(1);
    if (!leftClick && !rightClick) return;
    if (!Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo, buildDistance, groundLayer)) return;
    Vector3 targetPoint;
    if (rightClick)
    {
      targetPoint = hitInfo.point - hitInfo.normal * 0.1f; // move into the block
    }
    else
    {
      targetPoint = hitInfo.point + hitInfo.normal * 0.1f; // move towards the block
    }

    int chunkPosX = Mathf.FloorToInt(targetPoint.x / Chunk.chunkWidth);
    int chunkPosY = Mathf.FloorToInt(targetPoint.y / Chunk.chunkHeight);
    int chunkPosZ = Mathf.FloorToInt(targetPoint.z / Chunk.chunkDepth);

    ChunkPos chunkPos = new ChunkPos(chunkPosX,chunkPosY,chunkPosZ);

    Chunk thisChunk = world.chunks[chunkPos];

    int blockIndexX = Mathf.FloorToInt(targetPoint.x) - chunkPosX*Chunk.chunkWidth;
    int blockIndexY = Mathf.FloorToInt(targetPoint.y) - chunkPosY*Chunk.chunkHeight;
    int blockIndexZ = Mathf.FloorToInt(targetPoint.z) - chunkPosZ*Chunk.chunkDepth;
    
    if (rightClick)
    {
      thisChunk.blocks[blockIndexX, blockIndexY, blockIndexZ] = airBlock;
      thisChunk.GenerateMesh();
    }
    else
    {
      thisChunk.blocks[blockIndexX, blockIndexY, blockIndexZ] = inventory.currentBlock;
      thisChunk.GenerateMesh();
    }
  }
}