using System.Collections;
using UnityEngine;

public class PlayerStat : MonoBehaviour, IDamagable
{
    [SerializeField] private PlayerData playerData;
    
    public float currentHeart{ get; private set; }        //현재 하트 수
    public float currentEnergy{ get; private set; }    //현재 에너지
    
    //추가 스탯 
    private float bonusMaxHeart;
    private float bonusMaxEnergy;
    private float bonusSpeed;
    private float bonusJumpForce;
    private float bonusDashForce;
    private int bonusAttackDamage;
    private int bonusThrowDamage;
    
    private Animator animator;
    
    private static readonly int animIDHit = Animator.StringToHash("IsHit");
    private static readonly int animIDDie = Animator.StringToHash("IsDie");

    public bool isInvincible;
    public float CurrentMaxHeart => playerData.MaxHeart + bonusMaxHeart;
    public float CurrentMaxEnergy => playerData.MaxEnergy + bonusMaxEnergy;
    public float CurrentMoveSpeed => playerData.MoveSpeed + bonusSpeed;
    public float CurrentJumpForce => playerData.JumpForce + bonusJumpForce;
    public float CurrentDashForce => playerData.DashForce + bonusDashForce;
    public int CurrentAttackDamage => playerData.AttackDamage + bonusAttackDamage;
    public int CurrentThrowDamage => playerData.ThrowDamage + bonusThrowDamage;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        
        currentHeart = CurrentMaxHeart;
        currentEnergy = CurrentMaxEnergy;
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible || damage < 0) return;

        currentHeart -= damage;
        StartCoroutine(Damaged());
        for (int i = 0; i < damage; i++)
        {
            UIManager.Instance.TakeDamage();
        }
        
        Debug.Log("Player Damaged! Current Heart : " + currentHeart + "/" + CurrentMaxHeart);
        if (currentHeart <= 0)
        {
            currentHeart = 0;
            Die();
        }
    }

    public void DamageAndRespawn(int damage)
    {   //데미지 처리 및 리스폰까지
        TakeDamage(1);
        transform.position = GameManager.Instance.respawnPoint;
    }
    public void Die()
    {
       animator.SetBool(animIDDie, true);
       //사망 관련 로직
    }

    private IEnumerator Damaged()   //피격 후 무적 코루틴
    {
        animator.SetBool(animIDHit, true);
        isInvincible = true;
        yield return new WaitForSeconds(2.0f);
        animator.SetBool(animIDHit, false);
        isInvincible = false;
    }
}
