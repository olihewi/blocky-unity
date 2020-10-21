using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Block", menuName = "Blocky/Block")]
public class Block : ScriptableObject
{
    public BlockTile[] textures = new BlockTile[6];
}