using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tree", menuName = "Blocky/Tree")]
public class TreeType : ScriptableObject
{
    public Vector2 heightMinAndMax;

    public Block bark;
    public Block leaves;
}
