using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public struct GameStartEvent { }

public class GameManager : Singleton<GameManager>
{
    private GameData gameData;
    public GameData GameData => gameData;
    public PlayerStat player;
    public Vector3 respawnPoint { get; private set; } //플레이어 리스폰 지점
    public bool CanGoNextStage;         //첫번쨰 보스 클리어 여부
    public bool isSecondPhase;
    public bool skipIntro;      // 저장 데이터가 존재할 시 인트로 스킵
    public bool isFirstPhaseEnd;
    [field: SerializeField] public int mainNpcIndex { get; private set; }
    [field: SerializeField] public int mainNpcPosNum { get; private set; }


    protected override void Awake()
    {
        base.Awake();
        LoadData();
    }
    private void OnEnable()
    {
        EventBus.Subscribe<GameStartEvent>(OnGameStart);
    }

    private void OnDisable()
    {
        EventBus.UnSubscribe<GameStartEvent>(OnGameStart);
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
        gameData.playerHeart = player.currentHeart;
        gameData.playerEnergy = player.currentEnergy;
        gameData.respawnPoint = this.respawnPoint;
        gameData.CanGoNextStage = this.CanGoNextStage;
        gameData.mainNpcIndex = this.mainNpcIndex;
        gameData.mainNpcPosNum = this.mainNpcPosNum;
        
        //SaveLoadManager에게 데이터 저장을 요청
        SaveLoadManager.Instance.SaveGame(gameData);
    }
    public void LoadData()
    {
        //데이터 불러오기 요청
        gameData = SaveLoadManager.Instance.LoadGame();
        
        //데이터가 있다면, 현재 GameManager의 상태에 적용
        if (gameData != null)
        { 
            this.respawnPoint = gameData.respawnPoint;
            this.CanGoNextStage = gameData.CanGoNextStage;
            this.mainNpcIndex = gameData.mainNpcIndex;
            this.mainNpcPosNum = gameData.mainNpcPosNum;
            skipIntro = true;
        }
        else
        {
            gameData = new GameData();
            skipIntro = false;
        }
    }

    //게임 종료 시 자동으로 저장
    private void OnApplicationQuit()
    {
        //게임 씬이 아닐 경우 저장할 필요 X
        if (player == null) return;
        
        SaveData();
    }

    private void OnGameStart(GameStartEvent e)
    {
        Debug.Log("GameStart 이벤트 수신 -> MainScene을 로드합니다.");
        if (skipIntro)
        {
            SceneManager.LoadScene("MainScene");
        }
        else
        {
            SceneManager.LoadScene("IntroScene");
        }
    }
}
