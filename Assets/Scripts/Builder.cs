using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Builder : MonoBehaviour // Applied to the camera, this script allows placing and breaking of blocks
{
  public World world; 
  public Inventory inventory;

  public Block airBlock;
  public GameObject breakBlockParticles;
  public float buildDistance = 8; // Distance of the raycast for placing/breaking

  public LayerMask groundLayer;

  private FreeCamera _freeCamera;

  // Update is called once per frame
  private void Start()
  {
    _freeCamera = GetComponent<FreeCamera>();
  }

  void Update()
  {
    bool leftClick = Input.GetMouseButtonDown(0);
    bool rightClick = Input.GetMouseButtonDown(1);
    if (!leftClick && !rightClick) return;
    if (!_freeCamera.isCameraRotating) return;
    if (!Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo, buildDistance, groundLayer)) return;
    Vector3 targetPoint; // Location that the raycast hits
    if (rightClick)
    {
      targetPoint = hitInfo.point - hitInfo.normal * 0.1f; // Move into the block
    }
    else
    {
      targetPoint = hitInfo.point + hitInfo.normal * 0.1f; // Move out of the block
    }

    int chunkPosX = Mathf.FloorToInt(targetPoint.x / Chunk.CHUNK_WIDTH);
    int chunkPosY = Mathf.FloorToInt(targetPoint.y / Chunk.CHUNK_HEIGHT);
    int chunkPosZ = Mathf.FloorToInt(targetPoint.z / Chunk.CHUNK_DEPTH);

    ChunkPos chunkPos = new ChunkPos(chunkPosX,chunkPosY,chunkPosZ); // Determining chunk coordinates for the block affected

    Chunk thisChunk = world.chunks[chunkPos]; // Using the chunk coordinates to access the given chunk

    int blockIndexX = Mathf.FloorToInt(targetPoint.x) - chunkPosX*Chunk.CHUNK_WIDTH; // Narrowing the float coordinates
    int blockIndexY = Mathf.FloorToInt(targetPoint.y) - chunkPosY*Chunk.CHUNK_HEIGHT;// to integers which can be used
    int blockIndexZ = Mathf.FloorToInt(targetPoint.z) - chunkPosZ*Chunk.CHUNK_DEPTH; // to access the specific block.
    
    if (rightClick)
    {
      GameObject brokenParticles = Instantiate(breakBlockParticles, new Vector3(Mathf.FloorToInt(targetPoint.x)+0.5f, Mathf.FloorToInt(targetPoint.y)+0.5f, Mathf.FloorToInt(targetPoint.z)+0.5f), Quaternion.identity);
      ParticleSystem particleSystem = brokenParticles.GetComponent<ParticleSystem>();
      ParticleSystem.TextureSheetAnimationModule tex = particleSystem.textureSheetAnimation;
      Vector2 uvStart = thisChunk.blocks[blockIndexX, blockIndexY, blockIndexZ].textures[3].uvs[0];
      tex.startFrame = uvStart.x/16 + (1-uvStart.y) + 1/256f - 16/256f; // Block breaking particles have the texture of the block broken
      AudioSource blockBreakingSource = particleSystem.GetComponent<AudioSource>();
      blockBreakingSource.clip = thisChunk.blocks[blockIndexX, blockIndexY, blockIndexZ].audioMaterial.GetAudioClip(BlockMaterial.AudioEvent.Breaking);
      blockBreakingSource.Play();
      particleSystem.Play();
      thisChunk.blocks[blockIndexX, blockIndexY, blockIndexZ] = airBlock; // Setting the required block to air (destroying)
      thisChunk.GenerateMesh(); // Regenerating mesh and...

      if (blockIndexX == 0) thisChunk.adjacentChunks[3].GenerateMesh(); // If at the edge, regenerating neighbouring meshes
      else if (blockIndexX == Chunk.CHUNK_WIDTH-1) thisChunk.adjacentChunks[5].GenerateMesh();
      if (blockIndexY == 0) thisChunk.adjacentChunks[1].GenerateMesh();
      else if (blockIndexY == Chunk.CHUNK_HEIGHT-1) thisChunk.adjacentChunks[0].GenerateMesh();
      if (blockIndexZ == 0) thisChunk.adjacentChunks[2].GenerateMesh();
      else if (blockIndexZ == Chunk.CHUNK_DEPTH-1) thisChunk.adjacentChunks[4].GenerateMesh();
    }
    else if (thisChunk.blocks[blockIndexX,blockIndexY,blockIndexZ].isAir)
    {
      thisChunk.blocks[blockIndexX, blockIndexY, blockIndexZ] = inventory.GetHotbarItem(); // Setting to currently selected block
      thisChunk.GenerateMesh();
      if (blockIndexX == 0) thisChunk.adjacentChunks[3].GenerateMesh();
      else if (blockIndexX == Chunk.CHUNK_WIDTH-1) thisChunk.adjacentChunks[5].GenerateMesh();
      if (blockIndexY == 0) thisChunk.adjacentChunks[1].GenerateMesh();
      else if (blockIndexY == Chunk.CHUNK_HEIGHT-1) thisChunk.adjacentChunks[0].GenerateMesh();
      if (blockIndexZ == 0) thisChunk.adjacentChunks[2].GenerateMesh();
      else if (blockIndexZ == Chunk.CHUNK_DEPTH-1) thisChunk.adjacentChunks[4].GenerateMesh();
    }
  }
}