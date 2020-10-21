using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Texture", menuName = "Blocky/Texture")]
public class BlockTile : ScriptableObject
{
    public Vector2[] uvs = new Vector2[4];
}
