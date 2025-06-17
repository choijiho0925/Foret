using UnityEngine;

[System.Serializable]
public class GameData
{
    //저장 데이터 필드
    public Vector3 respawnPoint;
    public bool CanGoNextStage;
    public int mainNpcIndex;
    public int mainNpcPosNum;

    // 생성자: 데이터가 없을 때 사용할 기본값 설정
    public GameData()
    {
        respawnPoint = new Vector3(-5.5f, -6f, 0);
        CanGoNextStage = false;
        mainNpcIndex = 0;
        mainNpcPosNum = 0;
    }
}