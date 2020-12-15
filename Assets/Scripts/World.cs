using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class World : MonoBehaviour
{
  public Transform player;
  public int seed = 623456;

  public int horizontalRenderDistance = 3;
  public int verticalRenderDistance = 3;

  public Block airBlock;
  public Block fillerBlock;
  public Block surfaceBlock;
  public Block almostSurfaceBlock;
  public int seaLevel = 16;

  public GameObject chunkPrefab;
  public List<Biome> biomes;
  public FastNoiseLite biomeMap;
  public int biomeBlendAmount;
  public List<FastNoiseLite> caveNoiseLayers;

  public Dictionary<ChunkPos, Chunk> chunks = new Dictionary<ChunkPos, Chunk>();
  
  private List<ChunkPos> chunksToGenerate = new List<ChunkPos>();

  private void Start()
  {
    var position = player.position;
    currentChunk = new ChunkPos(Mathf.FloorToInt(position.x)/Chunk.chunkWidth-1,Mathf.FloorToInt(position.y)/Chunk.chunkHeight-1,Mathf.FloorToInt(position.z)/Chunk.chunkDepth-1);
    foreach (KeyValuePair<ChunkPos, Chunk> chunk in chunks)
    {
      Destroy(chunk.Value.gameObject);
    }
    chunks.Clear();
    foreach (Biome biome in biomes)
    {
      int index = 1;
      foreach (FastNoiseLite noiseLayer in biome.heightMapNoiseLayers)
      {
        noiseLayer.mSeed = (int) (seed * index * 0.75f);
        index++;
      }
    }

    //RenderSettings.fogColor = biomes[0].fogColor;
    //RenderSettings.skybox = biomes[0].skyboxMaterial;
  }
  

  public void ReloadAllChunks()
  {
    foreach (KeyValuePair<ChunkPos, Chunk> chunk in chunks)
    {
      Destroy(chunk.Value.gameObject);
    }
    chunks.Clear();
    UpdateChunks(currentChunk);
  }

  private ChunkPos currentChunk = new ChunkPos(0,0,0);
  private Biome currentPlayerBiome;
  private void Update()
  {
    var position = player.position;
    Biome playerBiome = GetBiome(Mathf.FloorToInt(position.x), Mathf.FloorToInt(position.z));
    if (playerBiome != currentPlayerBiome)
    {
      currentPlayerBiome = playerBiome;
      RenderSettings.fogColor = playerBiome.fogColor;
      RenderSettings.skybox = playerBiome.skyboxMaterial;
    }
    ChunkPos playerChunkPos = new ChunkPos(Mathf.FloorToInt(position.x)/Chunk.chunkWidth,Mathf.FloorToInt(position.y)/Chunk.chunkHeight,Mathf.FloorToInt(position.z)/Chunk.chunkDepth);
    if (playerChunkPos.x != currentChunk.x || playerChunkPos.y != currentChunk.y || playerChunkPos.z != currentChunk.z)
    {
      currentChunk = playerChunkPos;
      UpdateChunks(playerChunkPos);
    }

    // Generate Chunks to Generate
    for (int i = 0; i < 2; i++)
    {
      if (chunksToGenerate.Count == 0) break;
      ChunkPos chunkToGen = chunksToGenerate[0];
      Chunk thisChunk = GenerateChunk(chunkToGen.x, chunkToGen.y, chunkToGen.z);
      chunksToGenerate.Remove(chunkToGen);
    }
    
  }

  private void UpdateChunks(ChunkPos playerChunkPos)
  {
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
          if (!chunksToGenerate.Contains(cp) && (!chunks.TryGetValue(cp, out Chunk thisChunk) || !thisChunk.isGenerated))
          {
            chunksToGenerate.Add(cp);
          }
        }
      }
    }
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

  private Chunk GenerateChunk(int chunkX, int chunkY, int chunkZ)
  {
    ChunkPos thisChunkPos = new ChunkPos(chunkX, chunkY, chunkZ);
    if (!chunks.TryGetValue(thisChunkPos, out Chunk thisChunkObject))
    {
      GameObject thisChunk = GameObject.Instantiate(chunkPrefab, new Vector3(chunkX * Chunk.chunkWidth, chunkY * Chunk.chunkHeight, chunkZ * Chunk.chunkDepth), Quaternion.identity);
            thisChunk.transform.parent = transform;
            thisChunk.name = "Chunk [" + chunkX + ", " + chunkY + ", " + chunkZ + "]";
            thisChunkObject = thisChunk.GetComponent<Chunk>();
            thisChunkObject.FillWithAir(airBlock);
            chunks.Add(thisChunkPos,thisChunkObject);
    }

    thisChunkObject.thisChunkPos = new ChunkPos(chunkX, chunkY, chunkZ);
    
    Biome[,] chunkBiomes = new Biome[Chunk.chunkWidth + 2 * biomeBlendAmount,Chunk.chunkHeight + 2 * biomeBlendAmount];

    for (int x = -biomeBlendAmount; x < Chunk.chunkWidth + biomeBlendAmount; x++)
    {
      for (int z = -biomeBlendAmount; z < Chunk.chunkWidth + biomeBlendAmount; z++)
      {
        chunkBiomes[x + biomeBlendAmount,z + biomeBlendAmount] = GetBiome(chunkX * Chunk.chunkWidth + x, chunkZ * Chunk.chunkDepth + z);
      }
    }

    for (int x = 0; x < Chunk.chunkWidth; x++)
    {
      for (int z = 0; z < Chunk.chunkDepth; z++)
      {
        float perlin = 0f;
        Biome thisBiome = chunkBiomes[x + biomeBlendAmount,z + biomeBlendAmount];
        Dictionary<Biome,int> biomeBlend = new Dictionary<Biome, int>();
        int numOfBiomes = 0;
        for (int blendX = x; blendX <= x + biomeBlendAmount * 2; blendX++)
        {
          for (int blendZ = z; blendZ <= z + biomeBlendAmount * 2; blendZ++)
          {
            numOfBiomes++;
            if (biomeBlend.ContainsKey(chunkBiomes[blendX, blendZ]))
            {
              biomeBlend[chunkBiomes[blendX, blendZ]]++;
            }
            else
            {
              biomeBlend.Add(chunkBiomes[blendX, blendZ], 1);
            }
          }
        }

        foreach (KeyValuePair<Biome, int> blend in biomeBlend)
        {
          float thisBlendPerlin = 0;
          foreach (FastNoiseLite noiseLayer in blend.Key.heightMapNoiseLayers)
          {
            thisBlendPerlin += noiseLayer.GetNoise(chunkX * Chunk.chunkWidth + x, chunkZ * Chunk.chunkDepth + z) * noiseLayer.mAmplitude;
          }

          thisBlendPerlin += blend.Key.surfaceHeight;
          perlin += thisBlendPerlin * blend.Value;
        }

        perlin /= numOfBiomes;

        for (int y = 0; y < Chunk.chunkHeight; y++)
        {
          if (chunkY * Chunk.chunkHeight + y < perlin + seaLevel)
          {
            thisChunkObject.blocks[x, y, z] = thisBiome.fillerBlock;
            if (chunkY * Chunk.chunkHeight + y < seaLevel)
            {
              foreach (FastNoiseLite noiseLayer in caveNoiseLayers)
              {
                float cavePerlin = noiseLayer.GetNoise(chunkX * Chunk.chunkWidth + x, (chunkY * Chunk.chunkHeight + y) * 2, chunkZ * Chunk.chunkDepth + z);
                if (cavePerlin >= noiseLayer.threshold.x && cavePerlin <= noiseLayer.threshold.y)
                {
                  thisChunkObject.blocks[x, y, z] = airBlock;
                }
              }
            }

            if (thisChunkObject.blocks[x, y, z] == airBlock) continue;
            
            foreach (BlockLayer layer in thisBiome.blockLayers)
            {
              if (chunkY * Chunk.chunkHeight + y + layer.height >= perlin + seaLevel)
              {
                thisChunkObject.blocks[x, y, z] = layer.block;
              }
            }
            if (chunkY * Chunk.chunkHeight + y + 1 >= perlin + seaLevel)
            {
              GenerateTreeInChunk(chunkX*Chunk.chunkWidth+x,chunkY*Chunk.chunkHeight+y,chunkZ*Chunk.chunkDepth+z, thisBiome);
            }
            
          }
        }
      }
    }

    //thisChunkObject.blocks[0, 0, 0] = fillerBlock; // Enable for chunk borders

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
    
    thisChunkObject.isGenerated = true;
    thisChunkObject.GenerateMesh();
    return thisChunkObject;
  }

  private void SetBlock(int worldX, int worldY, int worldZ, Block block)
  {
    ChunkPos thisChunkPos = new ChunkPos(Mathf.FloorToInt(worldX / (float)Chunk.chunkWidth), Mathf.FloorToInt(worldY / (float)Chunk.chunkHeight), Mathf.FloorToInt(worldZ / (float)Chunk.chunkDepth));
    if (!chunks.TryGetValue(thisChunkPos, out Chunk thisChunk))
    {
      GameObject thisChunkObject = Instantiate(chunkPrefab, new Vector3(thisChunkPos.x * Chunk.chunkWidth, thisChunkPos.y * Chunk.chunkHeight, thisChunkPos.z * Chunk.chunkDepth), Quaternion.identity);
      thisChunkObject.transform.parent = transform;
      thisChunkObject.name = "Chunk [" + thisChunkPos.x + ", " + thisChunkPos.y + ", " + thisChunkPos.z + "]";
      thisChunk = thisChunkObject.GetComponent<Chunk>();
      thisChunk.FillWithAir(airBlock);
      chunks.Add(thisChunkPos,thisChunk);
    }
    thisChunk.blocks[MathMod(worldX,Chunk.chunkWidth), MathMod(worldY,Chunk.chunkHeight), MathMod(worldZ,Chunk.chunkDepth)] = block;
  }

  private void GenerateTreeInChunk(int worldX, int worldY, int worldZ, Biome biome)
  {
    if (biome.treeTypes.Count == 0) return;
    Random.InitState(worldX*10000+worldZ);
    TreeBiomeInstance thisTree = biome.treeTypes[Mathf.Abs((worldX + worldY + worldZ - seed)) % biome.treeTypes.Count];
    if (!(Random.value < thisTree.probability)) return;
    int treeHeight = Mathf.FloorToInt(Random.value * (thisTree.treeType.heightMinAndMax.y-thisTree.treeType.heightMinAndMax.x) + thisTree.treeType.heightMinAndMax.x);
    int leavesHeight = treeHeight - 1;
    for (int _y = 0; _y < treeHeight; _y++)
    {
      for (int _x = -thisTree.treeType.leavesWidth; _x < thisTree.treeType.trunkWidth + thisTree.treeType.leavesWidth; _x++)
      {
        for (int _z = -thisTree.treeType.leavesWidth; _z < thisTree.treeType.trunkWidth + thisTree.treeType.leavesWidth; _z++)
        {
          if (_y < treeHeight - 1)
          {
            SetBlock(worldX+_x,worldY+_y+leavesHeight,worldZ+_z,thisTree.treeType.leaves);
          }
        }
      }

      for (int _x = 0; _x < thisTree.treeType.trunkWidth; _x++)
      {
        for (int _z = 0; _z < thisTree.treeType.trunkWidth; _z++)
        {
          SetBlock(worldX + _x,worldY+_y+1,worldZ + _z,thisTree.treeType.bark);
        }
      }
    }
  }

  private Biome GetBiome(int worldX, int worldZ)
  {
    float thisValue = biomeMap.GetNoise(worldX, worldZ);
    Biome thisBiome = biomes[Mathf.FloorToInt(thisValue * (biomes.Count))];
    return thisBiome;
  }

  private static int MathMod(int a, int b)
  {
    return ((a % b) + b) % b;
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