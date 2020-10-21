using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(BlockTile))]
    public class BlockTileEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            BlockTile blockTileScript = (BlockTile) target;
            float tileX = (blockTileScript.uvs[0].x - 0.001f) * 64f;
            float tileY = (blockTileScript.uvs[0].y - 0.001f) * 64f;
            tileX = EditorGUILayout.FloatField("xPosition", tileX);
            tileY = EditorGUILayout.FloatField("yPosition", tileY);
            blockTileScript.uvs = new Vector2[]
            {
                new Vector2(tileX/64f, tileY/64),
                new Vector2(tileX/64f, (tileY+1)/64f),
                new Vector2((tileX+1)/64f, (tileY+1)/64f),
                new Vector2((tileX+1)/64f, tileY/64f),
            };
        }
    }
}
