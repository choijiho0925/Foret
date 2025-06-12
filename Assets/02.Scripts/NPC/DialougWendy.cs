using UnityEngine;

public class DialougWendy : DialogueNPC
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void OnTriggerStay2D(Collider2D other)
    {
        base.OnTriggerStay2D(other);
    }

    public override void StartDialogue()
    {
        base.StartDialogue();
    }

    public override void ShowNextLine()
    {
        base.ShowNextLine();
    }
}
