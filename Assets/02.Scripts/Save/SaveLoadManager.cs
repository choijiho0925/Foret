using System;
using UnityEngine;
using System.IO;

public class SaveLoadManager : Singleton<SaveLoadManager>
{
    public const string saveFileName = "savegame.json";
    private string saveFilePath;

    protected override void Awake()
    {
        base.Awake();
        saveFilePath = Path.Combine(Application.persistentDataPath, saveFileName);
    }

    public void SaveGame(GameData data)
    {
        // GameData 객체를 JSON 문자열로 변환
        string json = JsonUtility.ToJson(data, true);
        // 파일에 JSON 문자열 쓰기
        File.WriteAllText(saveFilePath, json);
        Debug.Log("게임 데이터 저장 완료: " + saveFilePath);
    }

    public GameData LoadGame()
    {
        // 저장된 파일이 있는지 확인
        if (File.Exists(saveFilePath))
        {
            // 파일에서 JSON 문자열 읽기
            string json = File.ReadAllText(saveFilePath);
            // JSON 문자열을 GameData 객체로 변환
            GameData data = JsonUtility.FromJson<GameData>(json);
            Debug.Log("게임 데이터 불러오기 완료.");
            return data;
        }
        else
        {
            Debug.Log("저장된 데이터가 없습니다. 기본값으로 시작합니다.");
            return null;
        }
    }

    public void DeleteSave()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, saveFileName);

        if (File.Exists(saveFilePath))
        {
            File.Delete(saveFilePath);
            Debug.Log("저장 데이터 삭제 완료.");
        }
    }
}