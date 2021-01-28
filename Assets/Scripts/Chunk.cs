using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class Chunk : MonoBehaviour
{
  
  public const int CHUNK_WIDTH = 16;
  public const int CHUNK_HEIGHT = 16;
  public const int CHUNK_DEPTH = 16;
  
  public Block[,,] blocks = new Block[CHUNK_WIDTH, CHUNK_HEIGHT, CHUNK_DEPTH];

  public GameObject transparentChild;
  public bool isGenerated = false;
  
  public ChunkPos thisChunkPos;
  public Chunk[] adjacentChunks = new Chunk[6]; // Used for mesh generation
  
  public void GenerateMesh()
  {
    if (!isGenerated) return;
    Mesh mesh = new Mesh();
    Mesh transparentMesh = new Mesh();

    List<Vector3> verts = new List<Vector3>();
    List<int> tris = new List<int>();
    List<Vector2> uvs = new List<Vector2>();
    
    List<Vector3> verts_t = new List<Vector3>();
    List<int> tris_t = new List<int>();
    List<Vector2> uvs_t = new List<Vector2>();

    for (int x = 0; x < CHUNK_WIDTH; x++)
    {
      for (int z = 0; z < CHUNK_DEPTH; z++)
      {
        for (int y = 0; y < CHUNK_HEIGHT; y++)
        {
          if (blocks[x, y, z].isAir) continue; // Skip this block if it is air (faster than performing a null check)

          int faceCounter = 0; // Face counter is used to calculate the number of triangles to generate
          ref List<Vector3> thisVerts = ref verts; // The lists are passed by reference to reduce checks
          ref List<int> thisTris = ref tris;
          ref List<Vector2> thisUvs = ref uvs;
          if (blocks[x, y, z].isTransparent)
          {
            thisVerts = ref verts_t;
            thisTris = ref tris_t;
            thisUvs = ref uvs_t;
          }
          Vector3 thisPos = new Vector3(x, y, z);
          // Top
          bool hasFace = false;
          if (y == CHUNK_HEIGHT - 1) // Checking adjacent chunks when at the border between chunks.
          {
            if (adjacentChunks[0] != null && adjacentChunks[0].blocks[x, 0, z].isTransparent)
              hasFace = true;
          }
          else if (blocks[x, y + 1, z].isTransparent)
            hasFace = true;

          if (hasFace) // Adding vertices at relative positions for the face direction
          {
            thisVerts.Add(thisPos + new Vector3(0, 1, 0));
            thisVerts.Add(thisPos + new Vector3(0, 1, 1));
            thisVerts.Add(thisPos + new Vector3(1, 1, 1));
            thisVerts.Add(thisPos + new Vector3(1, 1, 0));
            faceCounter++;
            thisUvs.AddRange(blocks[x, y, z].textures[0].GetUVs());
          }

          // Bottom
          hasFace = false;
          if (y == 0)
          {
            if (adjacentChunks[1] != null && adjacentChunks[1].blocks[x, CHUNK_HEIGHT - 1, z].isTransparent)
              hasFace = true;
          }
          else if (blocks[x, y - 1, z].isTransparent)
            hasFace = true;

          if (hasFace)
          {
            thisVerts.Add(thisPos + new Vector3(0, 0, 0));
            thisVerts.Add(thisPos + new Vector3(1, 0, 0));
            thisVerts.Add(thisPos + new Vector3(1, 0, 1));
            thisVerts.Add(thisPos + new Vector3(0, 0, 1));
            faceCounter++;
            thisUvs.AddRange(blocks[x, y, z].textures[1].GetUVs());
          }

          // Front
          hasFace = false;
          if (z == 0)
          {
            if (adjacentChunks[2] != null && adjacentChunks[2].blocks[x, y, CHUNK_DEPTH - 1].isTransparent)
              hasFace = true;
          }
          else if (blocks[x, y, z - 1].isTransparent)
            hasFace = true;

          if (hasFace)
          {
            thisVerts.Add(thisPos + new Vector3(0, 0, 0));
            thisVerts.Add(thisPos + new Vector3(0, 1, 0));
            thisVerts.Add(thisPos + new Vector3(1, 1, 0));
            thisVerts.Add(thisPos + new Vector3(1, 0, 0));
            faceCounter++;
            thisUvs.AddRange(blocks[x, y, z].textures[2].GetUVs());
          }

          // Back
          hasFace = false;
          if (z == CHUNK_DEPTH - 1)
          {
            if (adjacentChunks[4] != null && adjacentChunks[4].blocks[x, y, 0].isTransparent)
              hasFace = true;
          }
          else if (blocks[x, y, z + 1].isTransparent)
            hasFace = true;

          if (hasFace)
          {
            thisVerts.Add(thisPos + new Vector3(1, 0, 1));
            thisVerts.Add(thisPos + new Vector3(1, 1, 1));
            thisVerts.Add(thisPos + new Vector3(0, 1, 1));
            thisVerts.Add(thisPos + new Vector3(0, 0, 1));
            faceCounter++;
            thisUvs.AddRange(blocks[x, y, z].textures[4].GetUVs());
          }

          // Left
          hasFace = false;
          if (x == 0)
          {
            if (adjacentChunks[3] != null && adjacentChunks[3].blocks[CHUNK_WIDTH - 1, y, z].isTransparent)
              hasFace = true;
          }
          else if (blocks[x - 1, y, z].isTransparent)
            hasFace = true;

          if (hasFace)
          {
            thisVerts.Add(thisPos + new Vector3(0, 0, 1));
            thisVerts.Add(thisPos + new Vector3(0, 1, 1));
            thisVerts.Add(thisPos + new Vector3(0, 1, 0));
            thisVerts.Add(thisPos + new Vector3(0, 0, 0));
            faceCounter++;
            thisUvs.AddRange(blocks[x, y, z].textures[3].GetUVs());
          }

          // Right
          hasFace = false;
          if (x == CHUNK_WIDTH - 1)
          {
            if (adjacentChunks[5] != null && adjacentChunks[5].blocks[0, y, z].isTransparent)
              hasFace = true;
          }
          else if (blocks[x + 1, y, z].isTransparent)
            hasFace = true;

          if (hasFace)
          {
            thisVerts.Add(thisPos + new Vector3(1, 0, 0));
            thisVerts.Add(thisPos + new Vector3(1, 1, 0));
            thisVerts.Add(thisPos + new Vector3(1, 1, 1));
            thisVerts.Add(thisPos + new Vector3(1, 0, 1));
            faceCounter++;
            thisUvs.AddRange(blocks[x, y, z].textures[5].GetUVs());
          }

          // Triangles
          int vertCountOffset = thisVerts.Count - 4 * faceCounter; // Gets this block's vertices' offset from the start of the list
          for (int i = 0; i < faceCounter; i++)
          {
            thisTris.Add(vertCountOffset + i * 4);
            thisTris.Add(vertCountOffset + i * 4 + 1);
            thisTris.Add(vertCountOffset + i * 4 + 2); // Tri 1
            thisTris.Add(vertCountOffset + i * 4);
            thisTris.Add(vertCountOffset + i * 4 + 2);
            thisTris.Add(vertCountOffset + i * 4 + 3); // Tri 2
          }
          
        }
      }
    }

    mesh.vertices = verts.ToArray();
    transparentMesh.vertices = verts_t.ToArray();

    mesh.triangles = tris.ToArray();
    transparentMesh.triangles = tris_t.ToArray();

    mesh.uv = uvs.ToArray();
    transparentMesh.uv = uvs_t.ToArray();

    mesh.RecalculateNormals();
    transparentMesh.RecalculateNormals();

    GetComponent<MeshFilter>().mesh = mesh;
    GetComponent<MeshCollider>().sharedMesh = mesh;
    
    transparentChild.GetComponent<MeshFilter>().mesh = transparentMesh;
    transparentChild.GetComponent<MeshCollider>().sharedMesh = transparentMesh;

  }

  public void FillWithAir(Block air)
  {
    for (int x = 0; x < CHUNK_WIDTH; x++)
    {
      for (int y = 0; y < CHUNK_HEIGHT; y++)
      {
        for (int z = 0; z < CHUNK_DEPTH; z++)
        {
          blocks[x, y, z] = air;
        }
      }
    }
  }
}