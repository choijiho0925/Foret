using Cinemachine;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

public class NpcController : MonoBehaviour
{
    [SerializeField] private GameObject mainNpc;
    [SerializeField] private List<PlayableAsset> npcTimeline;
    [SerializeField] private NpcPosData npcPosData;
    [SerializeField] private Animator mainNpcAnimator;
    [SerializeField] private Collider2D bossRoomCollider;
    [SerializeField] private CinemachineVirtualCamera bossRoomCamera;

    public UnityAction action;
    public bool canInteract;

    private PlayableDirector director;
    private GameManager gameManager;

    private void Start()
    {
        director = GetComponent<PlayableDirector>();
        gameManager = GameManager.Instance;
        if (gameManager.mainNpcIndex < 2)
        {
            mainNpcAnimator.enabled = false;
        }
        else
        {
            mainNpcAnimator.enabled = true;
        }
        bossRoomCollider.gameObject.SetActive(false);
        canInteract = true;
        if (!mainNpc.activeSelf)
        {
            mainNpc.SetActive(true);
        }
        mainNpc.transform.position = npcPosData.pos[gameManager.mainNpcPosNum];
    }

    public void SetTimeline(DialogueData data)
    {
        switch (data.type)
        {
            case ActionType.Move:
                director.playableAsset = npcTimeline[3];
                break;
            case ActionType.Attack:
                director.playableAsset = npcTimeline[1];
                break;
            case ActionType.Heal:
                director.playableAsset = npcTimeline[2];
                break;
            case ActionType.Change:
                if (data.npcName == "밥")
                {
                    director.playableAsset = npcTimeline[0];
                }
                else
                {
                    director.playableAsset = npcTimeline[4];
                }
                break;
        }
    }

    public void PlayTimeline()
    {
        director.stopped += OnTimelineFinished;
        if (mainNpcAnimator.enabled == false)
        {
            mainNpcAnimator.enabled = true;
        }

        if (gameManager.mainNpcIndex == 5)
        {
            UIManager.Instance.dialogueController.gameObject.SetActive(false);
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
        gameManager.NextPosNum();
        if (gameManager.mainNpcPosNum >= npcPosData.pos.Count)
        {
            return;
        }
        mainNpc.transform.position = npcPosData.pos[gameManager.mainNpcPosNum];
    }

    public void PlusIndex()
    {
        gameManager.NextIndex();
    }
    public void StartBossRoom()
    {
        bossRoomCamera.Priority = 20;
        bossRoomCollider.gameObject.SetActive(true);
        GameManager.Instance.SetRespawnPoint(transform.position);
        mainNpc.SetActive(false);
    }

    public void CanInteract()
    {
        canInteract = true;
    }
}
