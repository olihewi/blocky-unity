using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    public static int chunkWidth = 16;
    public static int chunkHeight = 16;
    public static int chunkDepth = 16;
    
    //public GameObject cubePrefab;
    public Block[,,] blocks = new Block[chunkWidth,chunkHeight,chunkDepth];

    public void GenerateMesh()
    {
        Mesh mesh = new Mesh();

        List<Vector3> verts = new List<Vector3>();
        List<int> tris = new List<int>();
        List<Vector2> uvs = new List<Vector2>();

        for (int x = 0; x < chunkWidth; x++)
        {
            for (int z = 0; z < chunkDepth; z++)
            {
                for (int y = 0; y < chunkHeight; y++)
                {
                    if (blocks[x, y, z] == null) continue;
                    
                    int faceCounter = 0;
                    Vector3 thisPos = new Vector3(x, y, z);
                    // Top
                    if (y == chunkHeight-1 || blocks[x,y + 1,z] == null)
                    {
                        verts.Add(thisPos + new Vector3(0, 1, 0));
                        verts.Add(thisPos + new Vector3(0, 1, 1));
                        verts.Add(thisPos + new Vector3(1, 1, 1));
                        verts.Add(thisPos + new Vector3(1, 1, 0));
                        faceCounter++;
                        uvs.AddRange(blocks[x,y,z].textures[0].uvs);
                    }
                    // Bottom
                    if (y == 0 || blocks[x,y - 1,z] == null)
                    {
                        verts.Add(thisPos + new Vector3(0, 0, 0));
                        verts.Add(thisPos + new Vector3(1, 0, 0));
                        verts.Add(thisPos + new Vector3(1, 0, 1));
                        verts.Add(thisPos + new Vector3(0, 0, 1));
                        faceCounter++;
                        uvs.AddRange(blocks[x, y, z].textures[1].uvs);
                    }
                    // Front
                    if (z == 0 || blocks[x, y, z - 1] == null)
                    {
                        verts.Add(thisPos + new Vector3(0, 0, 0));
                        verts.Add(thisPos + new Vector3(0, 1, 0));
                        verts.Add(thisPos + new Vector3(1, 1, 0));
                        verts.Add(thisPos + new Vector3(1, 0, 0));
                        faceCounter++;
                        uvs.AddRange(blocks[x, y, z].textures[2].uvs);
                    }
                    // Back
                    if (z == chunkDepth-1 || blocks[x, y, z + 1] == null)
                    {
                        verts.Add(thisPos + new Vector3(1, 0, 1));
                        verts.Add(thisPos + new Vector3(1, 1, 1));
                        verts.Add(thisPos + new Vector3(0, 1, 1));
                        verts.Add(thisPos + new Vector3(0, 0, 1));
                        faceCounter++;
                        uvs.AddRange(blocks[x, y, z].textures[4].uvs);
                    }
                    // Left
                    if (x == 0 || blocks[x - 1, y, z] == null)
                    {
                        verts.Add(thisPos + new Vector3(0, 0, 1));
                        verts.Add(thisPos + new Vector3(0, 1, 1));
                        verts.Add(thisPos + new Vector3(0, 1, 0));
                        verts.Add(thisPos + new Vector3(0, 0, 0));
                        faceCounter++;
                        uvs.AddRange(blocks[x, y, z].textures[3].uvs);
                    }
                    // Right
                    if (x == chunkWidth-1 || blocks[x + 1, y, z] == null)
                    {
                        verts.Add(thisPos + new Vector3(1, 0, 0));
                        verts.Add(thisPos + new Vector3(1, 1, 0));
                        verts.Add(thisPos + new Vector3(1, 1, 1));
                        verts.Add(thisPos + new Vector3(1, 0, 1));
                        faceCounter++;
                        uvs.AddRange(blocks[x, y, z].textures[5].uvs);
                    }

                    // Triangles
                    int vertCountOffset = verts.Count - 4 * faceCounter; // Gets this block's vertices' offset from the start of the list
                    for (int i = 0; i < faceCounter; i++)
                    {
                        tris.Add(vertCountOffset + i * 4);
                        tris.Add(vertCountOffset + i * 4 + 1);
                        tris.Add(vertCountOffset + i * 4 + 2); // tri 1
                        tris.Add(vertCountOffset + i * 4);
                        tris.Add(vertCountOffset + i * 4 + 2);
                        tris.Add(vertCountOffset + i * 4 + 3); // tri 2
                    }
                }
            }
        }
        mesh.vertices = verts.ToArray();

        mesh.triangles = tris.ToArray();

        mesh.uv = uvs.ToArray();
        
        mesh.RecalculateNormals();

        GetComponent<MeshFilter>().mesh = mesh;
    }
}
