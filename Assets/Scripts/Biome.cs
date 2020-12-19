using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Biome", menuName = "Blocky/Biome")]
[System.Serializable]
public class Biome : ScriptableObject
{
  public Block fillerBlock;
  public List<BlockLayer> blockLayers;
  public List<FastNoiseLite> heightMapNoiseLayers;
  public int surfaceHeight = 0;
  public List<TreeBiomeInstance> treeTypes;
  public Color fogColor = new Color(0.75f,0.875f,0.95f);
  public Material skyboxMaterial;
}
[System.Serializable]
public class BlockLayer
{
  public Block block;
  public int height = 1;
}
