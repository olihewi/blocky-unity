using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
  public Transform player;
  public int seed = 623456;

  public int horizontalRenderDistance = 3;
  public int verticalRenderDistance = 3;

  public Block fillerBlock;
  public Block surfaceBlock;
  public Block almostSurfaceBlock;
  public int seaLevel = 16;

  public GameObject chunkPrefab;
  public List<FastNoiseLite> heightMapNoiseLayers;

  public Dictionary<ChunkPos, Chunk> chunks = new Dictionary<ChunkPos, Chunk>();
  
  private List<ChunkPos> chunksToGenerate = new List<ChunkPos>();

  private void Start()
  {
    var position = player.position;
    currentChunk = new ChunkPos(Mathf.FloorToInt(position.x)/16-1,Mathf.FloorToInt(position.y)/16-1,Mathf.FloorToInt(position.z)/16-1);
    foreach (KeyValuePair<ChunkPos, Chunk> chunk in chunks)
    {
      Destroy(chunk.Value.gameObject);
    }
    chunks.Clear();
  }
  

  public void ReloadAllChunks()
  {
    foreach (KeyValuePair<ChunkPos, Chunk> chunk in chunks)
    {
      Destroy(chunk.Value.gameObject);
    }
  }

  private ChunkPos currentChunk = new ChunkPos(0,0,0);
  private void Update()
  {
    var position = player.position;
    ChunkPos playerChunkPos = new ChunkPos(Mathf.FloorToInt(position.x)/16,Mathf.FloorToInt(position.y)/16,Mathf.FloorToInt(position.z)/16);
    if (playerChunkPos.x != currentChunk.x || playerChunkPos.y != currentChunk.y || playerChunkPos.z != currentChunk.z)
    {
      currentChunk = playerChunkPos;
      // Generate New Chunks
      for (int x = currentChunk.x - horizontalRenderDistance; x <= currentChunk.x + horizontalRenderDistance; x++)
      {
        for (int y = currentChunk.y - verticalRenderDistance; y <= currentChunk.y + verticalRenderDistance; y++)
        {
          for (int z = currentChunk.z - horizontalRenderDistance; z <= currentChunk.z + horizontalRenderDistance; z++)
          {
            if ((new Vector3(x,y,z) - new Vector3(currentChunk.x,currentChunk.y,currentChunk.z)).magnitude > horizontalRenderDistance)
              continue;
            ChunkPos cp = new ChunkPos(x,y,z);
            if (!chunks.ContainsKey(cp) && !chunksToGenerate.Contains(cp))
            {
              chunksToGenerate.Add(cp);
            }
          }
        }
      }
      // Destroy Old Chunks
      List<ChunkPos> chunksToDelete = new List<ChunkPos>();
      foreach (KeyValuePair<ChunkPos, Chunk> chunk in chunks)
      {
        ChunkPos cp = chunk.Key;
        if (Mathf.Abs(cp.x - playerChunkPos.x) > horizontalRenderDistance || Mathf.Abs(cp.y - playerChunkPos.y) > verticalRenderDistance || Mathf.Abs(cp.z - playerChunkPos.z) > horizontalRenderDistance)
        {
          chunksToDelete.Add(cp);
        }
      }
      foreach (ChunkPos cpos in chunksToDelete)
      {
        Destroy(chunks[cpos].gameObject);
        chunks.Remove(cpos);
      }
    }

    // Generate Chunks to Generate
    for (int i = 0; i < 2; i++)
    {
      if (chunksToGenerate.Count == 0) break;
      ChunkPos chunkToGen = chunksToGenerate[0];
      chunks.Add(chunkToGen,GenerateChunk(chunkToGen.x,chunkToGen.y,chunkToGen.z));
      chunksToGenerate.Remove(chunkToGen);
    }
    
  }

  private Chunk GenerateChunk(int chunkX, int chunkY, int chunkZ)
  {
    GameObject thisChunk = GameObject.Instantiate(chunkPrefab, new Vector3(chunkX * 16, chunkY * 16, chunkZ * 16), Quaternion.identity);
    thisChunk.transform.parent = transform;
    thisChunk.name = "Chunk [" + chunkX + ", " + chunkY + ", " + chunkZ + "]";
    Chunk thisChunkObject = thisChunk.GetComponent<Chunk>();

    for (int x = 0; x < Chunk.chunkWidth; x++)
    {
      for (int z = 0; z < Chunk.chunkDepth; z++)
      {
        float perlin = 0f;
        foreach (FastNoiseLite noiseLayer in heightMapNoiseLayers)
        {
          float thisPerlinLayer = noiseLayer.GetNoise(chunkX * 16 + x, chunkZ * 16 + z);
          if (thisPerlinLayer >= noiseLayer.threshold.x && thisPerlinLayer <= noiseLayer.threshold.y)
          {
            switch (noiseLayer.blendingMode)
            {
              case FastNoiseLite.BlendingOperator.Add:
                perlin += thisPerlinLayer * noiseLayer.mAmplitude;
                break;
              case FastNoiseLite.BlendingOperator.Subtract:
                perlin -= thisPerlinLayer * noiseLayer.mAmplitude;
                break;
              case FastNoiseLite.BlendingOperator.Multiply:
                perlin *= thisPerlinLayer * noiseLayer.mAmplitude;
                break;
              case FastNoiseLite.BlendingOperator.Divide:
                perlin /= thisPerlinLayer * noiseLayer.mAmplitude;
                break;
              case FastNoiseLite.BlendingOperator.Square:
                perlin = Mathf.Pow(perlin, thisPerlinLayer * 2);
                break;
              case FastNoiseLite.BlendingOperator.Set:
                perlin = thisPerlinLayer * noiseLayer.mAmplitude;
                break;
            }
          }
        }

        for (int y = 0; y < Chunk.chunkHeight; y++)
        {
          if (chunkY * 16 + y < perlin + seaLevel)
          {
            //GameObject.Instantiate(cubePrefab, new Vector3((chunkPosX * 16) + x, y, (chunkPosZ * 16) + z), Quaternion.identity);
            thisChunkObject.blocks[x, y, z] = fillerBlock;
            if (chunkY * 16 + y + 4 >= perlin + seaLevel)
            {
              thisChunkObject.blocks[x, y, z] = almostSurfaceBlock;
            }

            if (chunkY * 16 + y + 1 >= perlin + seaLevel)
            {
              thisChunkObject.blocks[x, y, z] = surfaceBlock;
            }
          }
        }
      }
    }

    Chunk neighbourChunk;
    if (chunks.TryGetValue(new ChunkPos(chunkX, chunkY + 1, chunkZ), out neighbourChunk)) // Top
    {
      neighbourChunk.adjacentChunks[1] = thisChunkObject;
      thisChunkObject.adjacentChunks[0] = neighbourChunk;
      neighbourChunk.GenerateMesh();
    }

    if (chunks.TryGetValue(new ChunkPos(chunkX, chunkY - 1, chunkZ), out neighbourChunk)) // Bottom
    {
      neighbourChunk.adjacentChunks[0] = thisChunkObject;
      thisChunkObject.adjacentChunks[1] = neighbourChunk;
      neighbourChunk.GenerateMesh();
    }

    if (chunks.TryGetValue(new ChunkPos(chunkX, chunkY, chunkZ - 1), out neighbourChunk)) // Front
    {
      neighbourChunk.adjacentChunks[4] = thisChunkObject;
      thisChunkObject.adjacentChunks[2] = neighbourChunk;
      neighbourChunk.GenerateMesh();
    }

    if (chunks.TryGetValue(new ChunkPos(chunkX, chunkY, chunkZ + 1), out neighbourChunk)) // Back
    {
      neighbourChunk.adjacentChunks[2] = thisChunkObject;
      thisChunkObject.adjacentChunks[4] = neighbourChunk;
      neighbourChunk.GenerateMesh();
    }

    if (chunks.TryGetValue(new ChunkPos(chunkX - 1, chunkY, chunkZ), out neighbourChunk)) // Left
    {
      neighbourChunk.adjacentChunks[5] = thisChunkObject;
      thisChunkObject.adjacentChunks[3] = neighbourChunk;
      neighbourChunk.GenerateMesh();
    }

    if (chunks.TryGetValue(new ChunkPos(chunkX + 1, chunkY, chunkZ), out neighbourChunk)) // Right
    {
      neighbourChunk.adjacentChunks[3] = thisChunkObject;
      thisChunkObject.adjacentChunks[5] = neighbourChunk;
      neighbourChunk.GenerateMesh();
    }

    //thisChunkObject.GenerateMesh();

    return thisChunkObject;
  }
  
  public Block getBlock(int worldX, int worldY, int worldZ)
  {
    return chunks[new ChunkPos(worldX / 16, worldY / 16, worldZ / 16)].blocks[worldX % 16, worldY % 16, worldZ % 16];
  }
  
}

public struct ChunkPos
{
  public int x, y, z;

  public ChunkPos(int x, int y, int z)
  {
    this.x = x;
    this.y = y;
    this.z = z;
  }
}