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
    [SerializeField]private int posnum;

    private void Start()
    {
        director = GetComponent<PlayableDirector>();
        gameManager = GameManager.Instance;
        mainNpcAnimator.enabled = false;
        posnum = 0;
        GoNextPos();
        bossRoomCollider.gameObject.SetActive(false);
    }

    public void SetTimeline(DialogueData data)
    {
        if (npcTimeline.Count < 4)
        {
            director.playableAsset = npcTimeline[0];
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
