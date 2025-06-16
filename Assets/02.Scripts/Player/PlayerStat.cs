using System.Collections;
using UnityEngine;

public class PlayerStat : MonoBehaviour, IDamagable
{
    [SerializeField] private PlayerData playerData;
    
    public int currentHeart{ get; private set; }        //현재 하트 수
    public int currentEnergy{ get; private set; }    //현재 에너지
    
    //추가 스탯 
    private int bonusMaxHeart;
    private int bonusMaxEnergy;
    private float bonusSpeed;
    private float bonusJumpForce;
    private float bonusDashForce;
    private int bonusAttackDamage;
    private int bonusThrowDamage;
    
    private Animator animator;
    
    private static readonly int animIDHit = Animator.StringToHash("IsHit");
    private static readonly int animIDDie = Animator.StringToHash("IsDie");

    public bool isInvincible;
    public int CurrentMaxHeart => playerData.MaxHeart + bonusMaxHeart;
    public int CurrentMaxEnergy => playerData.MaxEnergy + bonusMaxEnergy;
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
        
        if (currentHeart <= 0)
        {
            currentHeart = 0;
            Die();
        }
    }

    public bool Heal(int heal)
    {
        if (heal < 0 || currentHeart == CurrentMaxHeart) return false;
        
        currentHeart = Mathf.Min(currentHeart + heal, CurrentMaxHeart);
        for (int i = 0; i < heal; i++)
        {
            UIManager.Instance.Recovery();
        }
        return true;
    }

    public bool UseEnergy(int energy)
    {
        if (energy < 0) return false;
        
        currentEnergy = Mathf.Max(currentEnergy - energy, 0);

        return true;
    }

    public bool RestoreEnergy(int energy)
    {
        if (energy < 0) return false;
        
        currentEnergy = Mathf.Min(currentEnergy + energy, CurrentMaxEnergy);

        return true;
    }

    public void Recover()
    {
        //최대 체력이거나 에너지가 부족하면 회복 불가
        if (currentHeart == CurrentMaxHeart || UseEnergy(playerData.RecoverCost))
        {
            Heal(playerData.RecoverAmount);
        }
    }

    public void DamageAndRespawn(int damage)
    {   //데미지 처리 및 리스폰까지
        TakeDamage(damage);
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
