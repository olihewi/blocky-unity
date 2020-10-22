using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    public int seed = 623456;

    public int horizontalRenderDistance = 3;
    public int verticalRenderDistance = 3;

    public Block fillerBlock;
    public Block surfaceBlock;
    public Block almostSurfaceBlock;
    public int seaLevel = 16;
    public float perlinFrequency = 40f;
    public float perlinAmplitude = 16f;

    public GameObject chunkPrefab;
    
    private Dictionary<ChunkPos, Chunk> chunks = new Dictionary<ChunkPos,Chunk>();
    
    private void Start()
    {
        StartCoroutine(LoadChunks(false));
    }

    private IEnumerator LoadChunks(bool slow)
    {
        foreach (KeyValuePair<ChunkPos, Chunk> chunk in chunks)
        {
            Destroy(chunk.Value.gameObject);
        }
        chunks.Clear();
        
        for (int x = 0; x < 2 * horizontalRenderDistance - 1; x++)
        {
            for (int y = 0; y < 2 * verticalRenderDistance - 1; y++)
            {
                for (int z = 0; z < 2 * horizontalRenderDistance - 1; z++)
                {
                    chunks.Add(new ChunkPos(x-horizontalRenderDistance+1,y-verticalRenderDistance+1,z-horizontalRenderDistance+1), GenerateChunk(x-horizontalRenderDistance+1,y-verticalRenderDistance+1,z-horizontalRenderDistance+1));
                    if (slow)
                    {
                        yield return null;
                    }
                    
                }
            }
        }
    }

    public void ReloadAllChunks(bool slow)
    {
        StartCoroutine(LoadChunks(slow));
    }
    
    private void Update()
    {
        
    }

    private Chunk GenerateChunk(int chunkX, int chunkY, int chunkZ)
    {
        GameObject thisChunk = GameObject.Instantiate(chunkPrefab, new Vector3(chunkX * 16, chunkY * 16, chunkZ * 16), Quaternion.identity);
        thisChunk.transform.parent = transform;
        thisChunk.name = "Chunk [" + chunkX + ", " + chunkY + ", " + chunkZ + "]";
        Chunk thisChunkObject = thisChunk.GetComponent<Chunk>();
        
        for (int x = 0; x < Chunk.chunkWidth; x++)
        {
            for (int z = 0; z < Chunk.chunkHeight; z++)
            {
                float perlin = Mathf.PerlinNoise((seed + (chunkX * 16) + x) / perlinFrequency, (seed + (chunkZ * 16) + z) / perlinFrequency) * perlinAmplitude;
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

        thisChunkObject.GenerateMesh();

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
