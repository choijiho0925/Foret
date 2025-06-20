using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [Serializable]
    public class SpawnPoint
    {
        public Transform spawnPoint;
        public MonsterType type;
    }

    public List<SpawnPoint> MushroomSpawnPoints;
    public List<SpawnPoint> RatSpawnPoints;
    public List<SpawnPoint> NightBorneSpawnPoints;
    public List<SpawnPoint> FFMSpawnPoints;
    public List<SpawnPoint> BatSpawnPoints;
    public List<SpawnPoint> NacromancerSpawnPoints;

    private Dictionary<MonsterType, List<MonsterBase>> monsters = new Dictionary<MonsterType, List<MonsterBase>>();

    private void Start()
    {
        SpawnMonsters();
    }

    private void OnEnable()
    {
        EventBus.Subscribe<GameOverEvent>(GameOverHandler);
        EventBus.Subscribe<PlayerReviveEvent>(PlayerReviveHandler);
    }

    private void OnDisable()
    {
        EventBus.UnSubscribe<GameOverEvent>(GameOverHandler);
        EventBus.UnSubscribe<PlayerReviveEvent>(PlayerReviveHandler);
    }

    private void SpawnMonsters()     //모든 몬스터 스폰
    {
        foreach (SpawnPoint spawnPoint in MushroomSpawnPoints)
        {
            Spawn(spawnPoint);
        }
        foreach (SpawnPoint spawnPoint in RatSpawnPoints)
        {
            Spawn(spawnPoint);
        }
        foreach (SpawnPoint spawnPoint in NightBorneSpawnPoints)
        {
            Spawn(spawnPoint);
        }
        foreach (SpawnPoint spawnPoint in FFMSpawnPoints)
        {
            Spawn(spawnPoint);
        }
        foreach (SpawnPoint spawnPoint in BatSpawnPoints)
        {
            Spawn(spawnPoint);
        }
        foreach (SpawnPoint spawnPoint in NacromancerSpawnPoints)
        {
            Spawn(spawnPoint);
        }
    }

    private void Spawn(SpawnPoint spawnPoint)
    {
        //오브젝트 풀링을 통해 몬스터 가져오기
        MonsterBase monster = PoolManager.Instance.MonsterPool.
            Get(spawnPoint.type, spawnPoint.spawnPoint.position, Quaternion.identity);

        //스폰된 모든 몬스터는 monsters 딕셔너리에서 관리
        if (!monsters.ContainsKey(spawnPoint.type))
        {
            monsters.Add(spawnPoint.type, new List<MonsterBase>());
        }
        monsters[spawnPoint.type].Add(monster);
        //몬스터 사망 시 리스트에서 삭제
        monster.OnDeath += (() =>
        {
            monsters[spawnPoint.type].Remove(monster);
        });
    }

    private void GameOverHandler(GameOverEvent evnt)
    {
        //게임 오버 시 모든 몬스터 비활성화
        foreach (MonsterType type in monsters.Keys)
        {
            List<MonsterBase> monsterList = monsters[type];

            foreach (MonsterBase monster in monsterList)
            {
                PoolManager.Instance.MonsterPool.Return(type, monster);
            }

            monsterList.Clear();
        }
    }

    private void PlayerReviveHandler(PlayerReviveEvent evnt)
    {   //플레이어 부활 시 모든 몬스터 스폰
        SpawnMonsters();
    }
}
