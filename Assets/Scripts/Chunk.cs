using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class Chunk : MonoBehaviour
{
  
  public static int chunkWidth = 16;
  public static int chunkHeight = 16;
  public static int chunkDepth = 16;
  public Block[,,] blocks = new Block[chunkWidth, chunkHeight, chunkDepth];

  public GameObject transparentChild;
  public bool isGenerated = false;
  
  
  public ChunkPos thisChunkPos;
  public Chunk[] adjacentChunks = new Chunk[6];
  
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

    for (int x = 0; x < chunkWidth; x++)
    {
      for (int z = 0; z < chunkDepth; z++)
      {
        for (int y = 0; y < chunkHeight; y++)
        {
          if (blocks[x, y, z].isAir) continue;

          int faceCounter = 0;
          ref List<Vector3> thisVerts = ref verts;
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
          if (y == chunkHeight - 1)
          {
            if (adjacentChunks[0] != null && adjacentChunks[0].blocks[x, 0, z].isTransparent)
              hasFace = true;
          }
          else if (blocks[x, y + 1, z].isTransparent)
            hasFace = true;

          if (hasFace)
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
            if (adjacentChunks[1] != null && adjacentChunks[1].blocks[x, chunkHeight - 1, z].isTransparent)
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
            if (adjacentChunks[2] != null && adjacentChunks[2].blocks[x, y, chunkDepth - 1].isTransparent)
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
          if (z == chunkDepth - 1)
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
            if (adjacentChunks[3] != null && adjacentChunks[3].blocks[chunkWidth - 1, y, z].isTransparent)
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
          if (x == chunkWidth - 1)
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
            thisTris.Add(vertCountOffset + i * 4 + 2); // tri 1
            thisTris.Add(vertCountOffset + i * 4);
            thisTris.Add(vertCountOffset + i * 4 + 2);
            thisTris.Add(vertCountOffset + i * 4 + 3); // tri 2
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
    for (int x = 0; x < chunkWidth; x++)
    {
      for (int y = 0; y < chunkHeight; y++)
      {
        for (int z = 0; z < chunkDepth; z++)
        {
          blocks[x, y, z] = air;
        }
      }
    }
  }
}