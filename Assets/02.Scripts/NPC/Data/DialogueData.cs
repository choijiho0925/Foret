using System.Collections.Generic;
using UnityEngine;

public enum ActionTiming { None, Before, After }
public enum ActionType { Move, Attack, Heal, Change }

[CreateAssetMenu(menuName = "Dialogue/DialogueData")]
public class DialogueData : ScriptableObject
{
    public ActionTiming timing;
    public ActionType type;
    public string npcName;
    public List<string> dialogues;
    public bool isScene;
}
