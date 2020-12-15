using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Builder : MonoBehaviour
{
  public World world;
  public Inventory inventory;

  public Block airBlock;
  public GameObject breakBlockParticles;
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
      GameObject brokenParticles = Instantiate(breakBlockParticles, new Vector3(Mathf.FloorToInt(targetPoint.x)+0.5f, Mathf.FloorToInt(targetPoint.y)+0.5f, Mathf.FloorToInt(targetPoint.z)+0.5f), Quaternion.identity);
      ParticleSystem particleSystem = brokenParticles.GetComponent<ParticleSystem>();
      ParticleSystem.TextureSheetAnimationModule tex = particleSystem.textureSheetAnimation;
      Vector2 uvStart = thisChunk.blocks[blockIndexX, blockIndexY, blockIndexZ].textures[3].uvs[0];
      tex.startFrame = uvStart.x/16 + (1-uvStart.y) + 1/256f - 16/256f;
      particleSystem.Play();
      thisChunk.blocks[blockIndexX, blockIndexY, blockIndexZ] = airBlock;
      thisChunk.GenerateMesh();

      if (blockIndexX == 0) thisChunk.adjacentChunks[3].GenerateMesh();
      else if (blockIndexX == Chunk.chunkWidth-1) thisChunk.adjacentChunks[5].GenerateMesh();
      if (blockIndexY == 0) thisChunk.adjacentChunks[1].GenerateMesh();
      else if (blockIndexY == Chunk.chunkHeight-1) thisChunk.adjacentChunks[0].GenerateMesh();
      if (blockIndexZ == 0) thisChunk.adjacentChunks[2].GenerateMesh();
      else if (blockIndexZ == Chunk.chunkDepth-1) thisChunk.adjacentChunks[4].GenerateMesh();
    }
    else
    {
      thisChunk.blocks[blockIndexX, blockIndexY, blockIndexZ] = inventory.currentBlock;
      thisChunk.GenerateMesh();
      if (blockIndexX == 0) thisChunk.adjacentChunks[3].GenerateMesh();
      else if (blockIndexX == Chunk.chunkWidth-1) thisChunk.adjacentChunks[5].GenerateMesh();
      if (blockIndexY == 0) thisChunk.adjacentChunks[1].GenerateMesh();
      else if (blockIndexY == Chunk.chunkHeight-1) thisChunk.adjacentChunks[0].GenerateMesh();
      if (blockIndexZ == 0) thisChunk.adjacentChunks[2].GenerateMesh();
      else if (blockIndexZ == Chunk.chunkDepth-1) thisChunk.adjacentChunks[4].GenerateMesh();
    }
  }
}