using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public struct GameStartEvent { }

public class GameManager : Singleton<GameManager>
{
    public Vector3 respawnPoint { get; private set; } //플레이어 리스폰 지점
    public bool CanGoNextStage;         //첫번쨰 보스 클리어 여부
    public bool isSecondPhase;
    [field: SerializeField] public int mainNpcIndex { get; private set; }
    [field: SerializeField] public int mainNpcPosNum { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        LoadData();
    }

    public void SetRespawnPoint(Vector3 point)
    {
        respawnPoint = point;
    }

    public void NextIndex()
    {
        mainNpcIndex++;
    }

    public void NextPosNum()
    {
        mainNpcPosNum++;
    }

    public void SaveData()
    {
        //현재 게임 상태를 담을 GameData 객체 생성
        GameData data = new GameData();
        data.respawnPoint = this.respawnPoint;
        data.CanGoNextStage = this.CanGoNextStage;
        data.mainNpcIndex = this.mainNpcIndex;
        data.mainNpcPosNum = this.mainNpcPosNum;
        
        //SaveLoadManager에게 데이터 저장을 요청
        SaveLoadManager.Instance.SaveGame(data);
    }
    public void LoadData()
    {
        //데이터 불러오기 요청
        GameData data = SaveLoadManager.Instance.LoadGame();
        
        //데이터가 있다면, 현재 GameManager의 상태에 적용
        if (data != null)
        {
            this.respawnPoint = data.respawnPoint;
            this.CanGoNextStage = data.CanGoNextStage;
            this.mainNpcIndex = data.mainNpcIndex;
            this.mainNpcPosNum = data.mainNpcPosNum;
        }
    }

    //게임 종료 시 자동으로 저장
    private void OnApplicationQuit()
    {   
        SaveData();
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
