using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Editor
{
  [CustomEditor(typeof(World))]
  public class WorldGenEditor : UnityEditor.Editor
  {
    public override void OnInspectorGUI()
    {
      DrawUILine(new Color(0.5f, 0.5f, 0.5f));
      World worldScript = (World) target;
      worldScript.seed = EditorGUILayout.IntField("Seed", worldScript.seed);
      if (GUILayout.Button("Randomize"))
      {
        worldScript.seed = Random.Range(-9999999, 9999999);
      }

      DrawUILine(new Color(0.5f, 0.5f, 0.5f));
      EditorGUILayout.LabelField("Render Distances", EditorStyles.boldLabel);
      worldScript.horizontalRenderDistance = EditorGUILayout.IntSlider("Horizontal Render Distance", worldScript.horizontalRenderDistance, 1, 32);
      worldScript.verticalRenderDistance = EditorGUILayout.IntSlider("Vertical Render Distance", worldScript.verticalRenderDistance, 1, 32);
      DrawUILine(new Color(0.5f, 0.5f, 0.5f));
      EditorGUILayout.LabelField("World Generation Options", EditorStyles.boldLabel);
      worldScript.airBlock = (Block) EditorGUILayout.ObjectField("Air Block", worldScript.airBlock, typeof(Block), false);
      worldScript.fillerBlock = (Block) EditorGUILayout.ObjectField("Filler Block", worldScript.fillerBlock, typeof(Block), false);
      worldScript.surfaceBlock = (Block) EditorGUILayout.ObjectField("Surface Block", worldScript.surfaceBlock, typeof(Block), false);
      worldScript.almostSurfaceBlock = (Block) EditorGUILayout.ObjectField("Almost Surface Block", worldScript.almostSurfaceBlock, typeof(Block), false);
      worldScript.seaLevel = EditorGUILayout.IntField("Sea Level", worldScript.seaLevel);
      EditorGUILayout.LabelField("Biomes", EditorStyles.boldLabel);
      for (int i = 0; i < worldScript.biomes.Count; i++)
      {
        worldScript.biomes[i] = (Biome) EditorGUILayout.ObjectField(worldScript.biomes[i], typeof(Biome), false);
        if (GUILayout.Button("Remove Biome"))
        {
          worldScript.biomes.RemoveAt(i);
        }
      }
      if (GUILayout.Button("New Biome"))
      {
        worldScript.biomes.Add(CreateInstance<Biome>());
      }
      DrawUILine(new Color(0.5f,0.5f,0.5f));
      EditorGUILayout.LabelField("Biome Map", EditorStyles.boldLabel);
      worldScript.biomeMap.mSeed = worldScript.seed;
      worldScript.biomeMap.mNoiseType = FastNoiseLite.NoiseType.Cellular;
      worldScript.biomeMap.mCellularDistanceFunction = FastNoiseLite.CellularDistanceFunction.Hybrid;
      worldScript.biomeMap.mCellularReturnType = FastNoiseLite.CellularReturnType.CellValue;
      worldScript.biomeMap.mCellularJitterModifier = EditorGUILayout.Slider("Biome Jitter",worldScript.biomeMap.mCellularJitterModifier, 0, 3);
      worldScript.biomeMap.mFrequency = EditorGUILayout.Slider("Biome Frequency", worldScript.biomeMap.mFrequency, 0.0001f, 0.1f);
      worldScript.biomeBlendAmount = EditorGUILayout.IntSlider("Biome Blend", worldScript.biomeBlendAmount, 0, 8);
      DrawUILine(new Color(0.5f, 0.5f, 0.5f));
      EditorGUILayout.LabelField("Cave Noise Layers", EditorStyles.boldLabel);
      int noiseLayerIndex = 0;
      if (worldScript.caveNoiseLayers == null)
      {
        worldScript.caveNoiseLayers = new List<FastNoiseLite>();
      }

      foreach (FastNoiseLite noiseLayer in worldScript.caveNoiseLayers)
      {
        DrawUILine(new Color(0.5f, 0.5f, 0.5f));
        EditorGUILayout.LabelField("Noise Layer", EditorStyles.boldLabel);
        noiseLayer.mSeed = (int) (worldScript.seed * noiseLayerIndex * 0.75f);
        noiseLayer.mNoiseType = (FastNoiseLite.NoiseType) EditorGUILayout.EnumPopup("Noise Type", noiseLayer.mNoiseType);
        noiseLayer.mFrequency = EditorGUILayout.FloatField("Frequency", noiseLayer.mFrequency);
        noiseLayer.threshold = EditorGUILayout.Vector2Field("Threshold", noiseLayer.threshold);
        noiseLayer.mFractalType = (FastNoiseLite.FractalType) EditorGUILayout.EnumPopup("Fractal Type", noiseLayer.mFractalType);
        noiseLayer.mCellularDistanceFunction = (FastNoiseLite.CellularDistanceFunction) EditorGUILayout.EnumPopup("Cellular Distance Function", noiseLayer.mCellularDistanceFunction);
        noiseLayer.mCellularReturnType = (FastNoiseLite.CellularReturnType) EditorGUILayout.EnumPopup("Cellular Return Type", noiseLayer.mCellularReturnType);
        if (GUILayout.Button("Remove Layer"))
        {
          worldScript.caveNoiseLayers.RemoveAt(noiseLayerIndex);
        }

        DrawUILine(new Color(0.5f, 0.5f, 0.5f));
        noiseLayerIndex++;
      }
      if (GUILayout.Button("New Layer"))
      {
        worldScript.caveNoiseLayers.Add(new FastNoiseLite());
      }
      DrawUILine(new Color(0.5f, 0.5f, 0.5f));
      if (GUILayout.Button("Generate") && Application.isPlaying)
      {
        worldScript.ReloadAllChunks();
      }

    }
    
    

    private static void DrawUILine(Color color, int thickness = 1, int padding = 10)
    {
      Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(padding + thickness));
      r.height = thickness;
      r.y += padding / 2;
      r.x -= 2;
      r.width += 6;
      EditorGUI.DrawRect(r, color);
    }
  }
}