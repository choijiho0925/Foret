using UnityEngine;

public struct BossStartEvent
{
    public string bossName;
    public AudioClip bossBGM;

    public BossStartEvent(string bossName, AudioClip bossBGM)
    {
        this.bossName = bossName;
        this.bossBGM = bossBGM;
    }
}
public struct BossClearEvent
{
    
}
