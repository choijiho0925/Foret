using System.Collections.Generic;
using UnityEngine;

public enum ActionType {Move, Wait, Heal, Attack, Change}

[CreateAssetMenu(menuName = "NPCAction/NPCActionData")]
public class NPCActionData : ScriptableObject
{
    public List<NPCActionData> actions;
}

[System.Serializable]
public class NpcAction
{
    public ActionType type;
    public float duration;
    public Vector3 targetPosition;
    public GameObject effectPrefab;
}
