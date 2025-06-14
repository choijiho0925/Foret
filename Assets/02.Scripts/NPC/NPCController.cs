using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

public class NpcController : MonoBehaviour
{
    public UnityAction action;
    
    [SerializeField] private PlayableAsset npcMove;
    [SerializeField] private PlayableAsset npcAttack;
    
    private PlayableDirector director;

    private void Start()
    {
        director = GetComponent<PlayableDirector>();
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
