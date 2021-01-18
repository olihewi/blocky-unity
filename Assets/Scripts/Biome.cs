using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Biome", menuName = "Blocky/Biome")]
[System.Serializable]
public class Biome : ScriptableObject // ScriptableObjects are data stored in the form of objects (found in Data/Biomes)
{
  public Block fillerBlock; // Default block of the terrain (stone in most cases)
  public List<BlockLayer> blockLayers; // List of layers of blocks at the surface (dirt / grass)
  public List<FastNoiseLite> heightMapNoiseLayers; // List of noise layers that are used to determine the biome heightmap
  public int surfaceHeight = 0; // Used for plateaus, glaciers, etc. Base surface height that the heightmap is added to
  public List<TreeBiomeInstance> treeTypes; // List of tree features that generate in this biome and their probabilities
  public Color fogColor = new Color(0.75f,0.875f,0.95f);
  public Material skyboxMaterial;
}
[System.Serializable]
public class BlockLayer
{
  public Block block;
  public int height = 1;
}
