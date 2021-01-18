using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Block", menuName = "Blocky/Block")]
public class Block : ScriptableObject // ScriptableObjects are data stored in the form of objects (found in Data/Blocks)
{
  public bool isAir = false;
  public BlockTile[] textures = new BlockTile[6]; // 0 = top, 1 = bottom, 2 = front, 3 = left, 4 = back, 5 = right
  public bool isTransparent = false; // determines mesh type and if faces adjacent to this block should be rendered
}