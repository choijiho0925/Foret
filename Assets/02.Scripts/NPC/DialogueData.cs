using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/DialogueData")]
public class DialogueData : ScriptableObject
{
    public int indexnum;
    public string npcName; 
    public List<string>[] dialogues;
}
