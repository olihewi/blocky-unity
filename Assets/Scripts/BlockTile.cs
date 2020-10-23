using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Texture", menuName = "Blocky/Texture")]
public class BlockTile : ScriptableObject
{
    public Vector2[] uvs = new Vector2[4];

    public Vector2[] GetUVs()
    {
        return new Vector2[4]
        {
            uvs[0] + new Vector2(0.01f,0.01f),
            uvs[1] + new Vector2(0.01f,-0.01f),
            uvs[2] + new Vector2(-0.01f,-0.01f),
            uvs[3] + new Vector2(-0.01f,0.01f)
        };
    }
}
