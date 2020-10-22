using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(BlockTile))]
    public class BlockTileEditor : UnityEditor.Editor
    {
        private float tileX = 0;
        private float tileY = 0;
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            BlockTile blockTileScript = (BlockTile) target;
            tileX = EditorGUILayout.FloatField("xPosition", tileX);
            tileY = EditorGUILayout.FloatField("yPosition", tileY);
            if (GUILayout.Button("Set UVs"))
            {
                blockTileScript.uvs = new Vector2[]
                {
                    new Vector2(tileX/16f, tileY/16),
                    new Vector2(tileX/16f, (tileY+1)/16f),
                    new Vector2((tileX+1)/16f, (tileY+1)/16f),
                    new Vector2((tileX+1)/16f, tileY/16f),
                };
                Debug.Log("Set " + blockTileScript.name + " UVs: " + tileX + ", " + tileY);
            }
            
        }
    }
}
