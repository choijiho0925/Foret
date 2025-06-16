using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

public class NpcController : MonoBehaviour
{
    
    public UnityAction action;

    [SerializeField] private GameObject mainNpc;
    [SerializeField] private List<PlayableAsset> npcTimeline;
    [SerializeField] private NpcPosData npcPosData;
    [SerializeField] private Animator mainNpcAnimator;
    [SerializeField] private Collider2D bossRoomCollider;
    
    private PlayableDirector director;
    private GameManager gameManager;
    private int posnum;
    private bool[] isAnimationPlay;

    private void Start()
    {
        director = GetComponent<PlayableDirector>();
        gameManager = GameManager.Instance;
        mainNpcAnimator.enabled = false;
        posnum = 0;
        GoNextPos();
        bossRoomCollider.gameObject.SetActive(false);
        isAnimationPlay = new bool[npcTimeline.Count];
        for (int i = 0; i < isAnimationPlay.Length; i++)
        {
            isAnimationPlay[i] = false;
        }
    }

    public bool SetTimeline(DialogueData data)
    {
        if (npcTimeline.Count < 4)
        {
            if (!isAnimationPlay[0])
            {
                director.playableAsset = npcTimeline[0];
                isAnimationPlay[0] = true;
                return true;
            }
            return false;
        }
        switch (data.type)
        {
            case ActionType.Move :
                if(!isAnimationPlay[0]) return false;
                director.playableAsset = npcTimeline[0];
                isAnimationPlay[0] = true;
                return true;
            case ActionType.Attack :
                if(!isAnimationPlay[1]) return false;
                director.playableAsset = npcTimeline[1];
                isAnimationPlay[1] = true;
                return true;
            case ActionType.Heal :
                if(!isAnimationPlay[2]) return false;
                director.playableAsset = npcTimeline[2];
                isAnimationPlay[2] = true;
                return true;
            case ActionType.Change :
                if(!isAnimationPlay[3]) return false;
                director.playableAsset = npcTimeline[3];
                isAnimationPlay[3] = true;
                return true;
        }
        return false;
    }

    public void PlayTimeline()
    {
        director.stopped += OnTimelineFinished;
        if (mainNpcAnimator.enabled == false)
        {
            mainNpcAnimator.enabled = true;
        }
        director.Play();
    }

    private void OnTimelineFinished(PlayableDirector d)
    {
        director.stopped -= OnTimelineFinished;
        action?.Invoke();
    }

    public void GoNextPos()//npc위치 변경
    {
        if (posnum >= npcPosData.pos.Count)
        {
            posnum = npcPosData.pos.Count;
        }
        mainNpc.transform.position = npcPosData.pos[posnum];
        posnum++;
    }
    
    public void PlusIndex()
    {
        gameManager.NextIndex();
    }
   public void StartBossRoom()
   {
       bossRoomCollider.gameObject.SetActive(true);
       GameManager.Instance.SetRespawnPoint(transform.position);
   }
}
