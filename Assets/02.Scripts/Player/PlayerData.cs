using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName =  "Data/PlayerData", fileName = "PlayerData")]
public class PlayerData : ScriptableObject
{
    public int MaxHeart;       //하트
    public int MaxEnergy;       //에너지
    public float MoveSpeed;     //이동속도
    public float JumpForce;     //점프력
    public float DashForce;     //대쉬 거리
    public int AttackDamage;  //공격력(평타)
    public int ThrowDamage;   //공격력(투사체)
}
