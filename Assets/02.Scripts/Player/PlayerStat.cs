using System;
using UnityEngine;

public class PlayerStat : MonoBehaviour, IDamagable
{
    [SerializeField] private PlayerData playerData;
    
    public float currentHP{ get; private set; }        //현재 체력
    public float currentEnergy{ get; private set; }    //현재 에너지
    
    //추가 스탯 
    private float bonusMaxHealth;
    private float bonusMaxEnergy;
    private float bonusSpeed;
    private float bonusJumpForce;
    private float bonusDashForce;
    private int bonusAttackDamage;
    private int bonusThrowDamage;
    
    private Animator animator;
    
    private static readonly int animIDHit = Animator.StringToHash("IsHit");
    private static readonly int animIDDie = Animator.StringToHash("IsDie");
    
    
    public float CurrentMaxHealth => playerData.MaxHealth + bonusMaxHealth;
    public float CurrentMaxEnergy => playerData.MaxEnergy + bonusMaxEnergy;
    public float CurrentMoveSpeed => playerData.MoveSpeed + bonusSpeed;
    public float CurrentJumpForce => playerData.JumpForce + bonusJumpForce;
    public float CurrentDashForce => playerData.DashForce + bonusDashForce;
    public int CurrentAttackDamage => playerData.AttackDamage + bonusAttackDamage;
    public int CurrentThrowDamage => playerData.ThrowDamage + bonusThrowDamage;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        
        currentHP = CurrentMaxHealth;
        currentEnergy = CurrentMaxEnergy;
    }

    public void TakeDamage(float damage)
    {
        if (damage < 0) return;

        currentHP -= damage;
        Debug.Log("Player Damaged! Current Health : " + currentHP + "/" + CurrentMaxHealth);
        if (currentHP <= 0)
        {
            currentHP = 0;
            Die();
        }
    }

    public void Die()
    {
       animator.SetBool(animIDDie, true);
       //사망 관련 로직
    }
}
