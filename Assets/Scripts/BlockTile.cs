using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Texture", menuName = "Blocky/Texture")]
public class BlockTile : ScriptableObject // ScriptableObjects are data stored in the form of objects (found in Data/Textures)
{
  public Vector2[] uvs = new Vector2[4]; // UV coordinates for this texture

  public Vector2[] GetUVs()
  {
    return new Vector2[4]
    {
      uvs[0] + new Vector2(0.001f, 0.001f), // Offset is used to fix texture bleeding
      uvs[1] + new Vector2(0.001f, -0.001f),
      uvs[2] + new Vector2(-0.001f, -0.001f),
      uvs[3] + new Vector2(-0.001f, 0.001f)
    };
  }
}