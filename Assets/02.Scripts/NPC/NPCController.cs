using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

public class NpcController : MonoBehaviour
{
    private int isHeal = Animator.StringToHash("IsHeal");
    public UnityAction action;
    
    [SerializeField] private List<PlayableAsset> npcTimeline;
    
    private PlayableDirector director;
    private Animator animator;

    private void Start()
    {
        director = GetComponent<PlayableDirector>();
    }

    public void SetTimeline(DialogueData data)
    {
        if (npcTimeline.Count < 4)
        {
            director.playableAsset = npcTimeline[0];
            return;
        }
        else
        {
            switch (data.type)
            {
                case ActionType.Move :
                    director.playableAsset = npcTimeline[0];
                    break;
                case ActionType.Attack :
                    director.playableAsset = npcTimeline[1];
                    break;
                case ActionType.Heal :
                    director.playableAsset = npcTimeline[2];
                    animator.SetBool(isHeal,true);
                    break;
                case ActionType.Change :
                    director.playableAsset = npcTimeline[3];
                    break;
            }
        }
    }

    public void Playtimeline()
    {
        director.stopped += OnTimelineFinished;
        director.Play();
    }

    private void OnTimelineFinished(PlayableDirector d)
    {
        director.stopped -= OnTimelineFinished;
        action?.Invoke();
    }
}
