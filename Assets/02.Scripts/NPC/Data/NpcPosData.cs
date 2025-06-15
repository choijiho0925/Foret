using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "NpcPos/NpcPosData")]
public class NpcPosData : ScriptableObject
{
    public List<Vector2> pos;
}