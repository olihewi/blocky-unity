using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Editor
{
  [CustomEditor(typeof(Biome))]
  public class BiomeEditor : UnityEditor.Editor
  {
    public override void OnInspectorGUI()
    {
      DrawUILine(new Color(0.5f,0.5f,0.5f));
      Biome biomeScript = (Biome) target;
      EditorGUI.BeginChangeCheck();
      biomeScript.fillerBlock = (Block) EditorGUILayout.ObjectField("Filler Block", biomeScript.fillerBlock, typeof(Block), false);
      DrawUILine(new Color(0.5f,0.5f,0.5f));
      EditorGUILayout.LabelField("Block Layers", EditorStyles.boldLabel);
      int layerIndex = 0;
      if (biomeScript.blockLayers == null)
      {
        biomeScript.blockLayers = new List<BlockLayer>();
      }
      foreach (BlockLayer layer in biomeScript.blockLayers)
      {
        DrawUILine(new Color(0.5f,0.5f,0.5f));
        layer.block = (Block) EditorGUILayout.ObjectField("Block", layer.block, typeof(Block), false);
        layer.height = EditorGUILayout.IntSlider("Height", layer.height, 1, 32);
        if (GUILayout.Button("Remove Layer"))
        {
          biomeScript.blockLayers.RemoveAt(layerIndex);
        }
        layerIndex++;
      }
      if (GUILayout.Button("New Layer"))
      {
        biomeScript.blockLayers.Add(new BlockLayer());
      }
      DrawUILine(new Color(0.5f,0.5f,0.5f));
      if (biomeScript.heightMapNoiseLayers == null)
      {
        biomeScript.heightMapNoiseLayers = new List<FastNoiseLite>();
      }
      EditorGUILayout.LabelField("Height Map Noise Layers", EditorStyles.boldLabel);
      int noiseLayerIndex = 0;
      foreach (FastNoiseLite noiseLayer in biomeScript.heightMapNoiseLayers)
      {
        DrawUILine(new Color(0.5f, 0.5f, 0.5f));
        noiseLayer.mNoiseType = (FastNoiseLite.NoiseType) EditorGUILayout.EnumPopup("Noise Type", noiseLayer.mNoiseType);
        noiseLayer.mFrequency = EditorGUILayout.FloatField("Frequency", noiseLayer.mFrequency);
        noiseLayer.mAmplitude = EditorGUILayout.FloatField("Amplitude", noiseLayer.mAmplitude);
        noiseLayer.blendingMode = (FastNoiseLite.BlendingOperator) EditorGUILayout.EnumPopup("Blending Mode", noiseLayer.blendingMode);
        noiseLayer.threshold = EditorGUILayout.Vector2Field("Threshold", noiseLayer.threshold);
        noiseLayer.mFractalType = (FastNoiseLite.FractalType) EditorGUILayout.EnumPopup("Fractal Type", noiseLayer.mFractalType);
        if (GUILayout.Button("Remove Layer"))
        {
          biomeScript.heightMapNoiseLayers.RemoveAt(noiseLayerIndex);
        }
        noiseLayerIndex++;
      }
      if (GUILayout.Button("New Layer"))
      {
        biomeScript.heightMapNoiseLayers.Add(new FastNoiseLite());
      }

      biomeScript.surfaceHeight = EditorGUILayout.IntSlider("Surface Height", biomeScript.surfaceHeight, 0, 64);
      DrawUILine(new Color(0.5f, 0.5f, 0.5f));
      EditorGUILayout.LabelField("Trees", EditorStyles.boldLabel);
      if (biomeScript.treeTypes == null)
      {
        biomeScript.treeTypes = new List<TreeBiomeInstance>();
      }
      for (int i = 0; i < biomeScript.treeTypes.Count; i++)
      {
        biomeScript.treeTypes[i].treeType = (TreeType) EditorGUILayout.ObjectField(biomeScript.treeTypes[i].treeType, typeof(TreeType), false);
        biomeScript.treeTypes[i].probability = EditorGUILayout.Slider("Probability", biomeScript.treeTypes[i].probability, 0.001f, 0.1f);
      }
      if (GUILayout.Button("New Tree"))
      {
        biomeScript.treeTypes.Add(new TreeBiomeInstance());
      }
      if (GUILayout.Button("Remove Tree"))
      {
        biomeScript.treeTypes.RemoveAt(biomeScript.treeTypes.Count-1);
      }
      DrawUILine(new Color(0.5f, 0.5f, 0.5f));
      biomeScript.fogColor = EditorGUILayout.ColorField("Fog Colour", biomeScript.fogColor);
      biomeScript.skyboxMaterial = (Material) EditorGUILayout.ObjectField("Skybox Material", biomeScript.skyboxMaterial, typeof(Material), false);
      if (EditorGUI.EndChangeCheck())
      {
        Debug.Log("Ended change check");
        EditorUtility.SetDirty(target);
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

