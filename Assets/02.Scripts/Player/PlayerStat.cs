using _02.Scripts.Player;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStat : MonoBehaviour, IDamagable
{
    [SerializeField] private PlayerData playerData;
    [SerializeField] private GameObject recoverEffect;

    public int currentHeart { get; private set; }        //현재 하트 수
    public int currentEnergy { get; private set; }    //현재 에너지
    public bool isDead { get; private set; }

    //추가 스탯 
    private int bonusMaxHeart;
    private int bonusMaxEnergy;
    private float bonusSpeed;
    private float bonusJumpForce;
    private float bonusDashForce;
    private int bonusAttackDamage;
    private int bonusThrowDamage;

    private GameManager gameManager;
    private PlayerCtrl playerCtrl;
    private PlayerSFX playerSFX;
    private PlayerMovement playerMovement;
    private Animator animator;
    private UIManager uiManager;
    private Coroutine energyGenRoutine;

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
        playerCtrl = GetComponent<PlayerCtrl>();
        playerSFX = GetComponentInChildren<PlayerSFX>();
        playerMovement = GetComponent<PlayerMovement>();
        gameManager = GameManager.Instance;
        gameManager.player = this;
        uiManager = UIManager.Instance;

        currentHeart = gameManager.GameData.playerHeart;
        currentEnergy = gameManager.GameData.playerEnergy;
        transform.position = gameManager.GameData.respawnPoint;
        //에너지 기본 재생 루틴
        energyGenRoutine = StartCoroutine(EnergyGenRoutine());
    }

    //게임 시작 시 체력, 에너지 값 설정
    public void Init(int heart, int energy)
    {
        currentHeart = heart;
        currentEnergy = energy;
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible || damage < 0 || isDead) return;

        playerSFX.PlayHitClip();
        currentHeart -= damage;
        StartCoroutine(Damaged());
        for (int i = 0; i < damage; i++)
        {
            uiManager.TakeDamage();
        }

        if (currentHeart <= 0)
        {
            currentHeart = 0;
            Die();
        }
    }

    public bool Heal(int heal)
    {
        if (heal < 0 || currentHeart == CurrentMaxHeart || isDead) return false;

        currentHeart = Mathf.Min(currentHeart + heal, CurrentMaxHeart);
        for (int i = 0; i < heal; i++)
        {
            uiManager.Recovery();
        }
        return true;
    }

    public bool UseEnergy(int energy)
    {
        if (energy > currentEnergy || energy < 0 || isDead) return false;

        currentEnergy = Mathf.Max(currentEnergy - energy, 0);
        uiManager.UpdateGauge(((float)currentEnergy / CurrentMaxEnergy));
        return true;
    }

    public bool RestoreEnergy(int energy)
    {
        if (energy < 0 || isDead) return false;

        currentEnergy = Mathf.Min(currentEnergy + energy, CurrentMaxEnergy);
        uiManager.UpdateGauge(((float)currentEnergy / CurrentMaxEnergy));
        return true;
    }

    public void Recover()
    {
        if (isDead) return;

        //최대 체력이거나 에너지가 부족하면 회복 불가
        if (currentHeart != CurrentMaxHeart && UseEnergy(playerData.RecoverCost))
        {
            Heal(playerData.RecoverAmount);
            StartCoroutine(RecoverEffect());
        }
    }

    public void DamageAndRespawn(int damage)
    {   //데미지 처리 및 리스폰까지
        TakeDamage(damage);
        transform.position = gameManager.respawnPoint;
    }

    public void Die()
    {
        animator.SetBool(animIDDie, true);
        //사망 관련 로직
        isDead = true;
        playerCtrl.canControl = false;
        playerMovement.Stop();

        EventBus.Raise(new GameOverEvent());     //게임 오버 이벤트 실행
    }

    public void Revive()
    {
        if (!isDead) return;

        isDead = false;
        animator.SetBool(animIDDie, false);
        playerCtrl.canControl = true;
        Heal(CurrentMaxHeart);
        transform.position = GameManager.Instance.respawnPoint;

        energyGenRoutine = StartCoroutine(EnergyGenRoutine());

        EventBus.Raise(new PlayerReviveEvent());
    }

    private IEnumerator Damaged()   //피격 후 무적 코루틴
    {
        animator.SetBool(animIDHit, true);
        isInvincible = true;
        yield return new WaitForSeconds(2.0f);
        animator.SetBool(animIDHit, false);
        isInvincible = false;
    }

    private IEnumerator RecoverEffect() //회복 이팩트
    {
        recoverEffect.SetActive(true);
        yield return new WaitForSeconds(0.7f);
        recoverEffect.SetActive(false);
    }

    private IEnumerator EnergyGenRoutine()
    {
        while (!isDead)
        {
            yield return new WaitForSeconds(1.0f);
            RestoreEnergy(2);
        }
    }
}
