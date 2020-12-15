using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tree", menuName = "Blocky/Tree")]
public class TreeType : ScriptableObject
{
    public Vector2 heightMinAndMax;
    public Block bark;
    public Block leaves;
    public int trunkWidth = 1;
    public int leavesWidth = 2;
}
[System.Serializable]
public class TreeBiomeInstance
{
    public TreeType treeType;
    public float probability = 0.01f;
}