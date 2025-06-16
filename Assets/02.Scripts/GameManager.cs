using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public struct GameStartEvent { }

public class GameManager : Singleton<GameManager>
{
    public Vector3 respawnPoint { get; private set; } //플레이어 리스폰 지점
    public bool CanGoNextStage;
    public bool isSecondPhase;
    public int mainNpcIndex { get; private set; }
    public void SetRespawnPoint(Vector3 point)
    {
        respawnPoint = point;
    }

    public void NextIndex(int index)
    {
        index++;
    }

    private void OnEnable()
    {
        EventBus.Subscribe<GameStartEvent>(OnGameStart);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<GameStartEvent>(OnGameStart);
    }

    private void OnGameStart(GameStartEvent e)
    {
        Debug.Log("GameStart 이벤트 수신 -> MainScene을 로드합니다.");
        SceneManager.LoadScene("MainScene");
    }
}
