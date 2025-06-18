using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public abstract class MonsterBase : MonoBehaviour, IDamagable
{
    [Header("기본 스텟")]
    [SerializeField] private int maxHealth;        //몬스터 최대 체력
    [SerializeField] private int health;          // 몬스터 체력
    [SerializeField] private float moveSpeed;       // 이동 속도
    [SerializeField] private float detectionRange;  // 감지 범위
    [SerializeField] private int attackPower;     // 공격력
    [SerializeField] private float attackRange;     // 공격 범위
    [SerializeField] private bool isInvincible;     // 무적인지 확인
    [SerializeField] private bool isGround;         // 지상 몹인지 확인
    [SerializeField] private MonsterType type;      //몬스터 종류

    public UnityAction OnDeath;    
    
    private GameObject player;                      // 플레이어
    private MonsterStateMachine stateMachine;       // 상태머신
    private MonsterAnimationHandler animationHandler;   // 몬스터 애니메이션
    private SpriteRenderer spriteRenderer;              // 몬스터 이미지
    private bool isDead;                            //사망 여부
    private Collider2D collider;

    #region 프로퍼티
    public int MaxHealth { get => maxHealth;set => maxHealth = value;}
    public int Health { get => health; set => health = value; }
    public float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
    public float DetectionRange => detectionRange;
    public int AttackPower => attackPower;
    public float AttackRange => attackRange;
    public bool IsInvincible { get => isInvincible; set => isInvincible = value; }
    public bool IsGround => isGround;
    public GameObject Player => player;
    public MonsterStateMachine StateMachine => stateMachine;
    public MonsterAnimationHandler AnimationHandler => animationHandler;
    public SpriteRenderer SpriteRenderer => spriteRenderer;
    public bool IsDead { get => isDead; set => isDead = value; }

    #endregion

    protected virtual void Awake()
    {
        player = GameObject.FindWithTag("Player");
        stateMachine = new MonsterStateMachine(this);
        animationHandler = GetComponent<MonsterAnimationHandler>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        collider = GetComponent<Collider2D>();
    }

    private void OnEnable()
    {
        isDead = false;
        //체력 초기화
        health = maxHealth;
    }

    protected void Initialize()
    {
        isDead = false;
        health = maxHealth;
    }

    private void OnDisable()
    {
        OnDeath = null;
    }

    protected virtual void Start()
    {
        // 몬스터 종류에 따른 상태 초기화
        if (isGround)
        {
            stateMachine.ChangeState(new GroundIdleState(this));
        }
        else
        {
            stateMachine.ChangeState(new AirIdleState(this));
        }
    }

    protected virtual void Update()
    {
        if (isDead) return;
        
        stateMachine?.Update();
    }

    public abstract void Move();  // 지상 / 공중 따른 이동 구현

    public abstract void Attack();  // 근거리 / 원거리 따른 공격 구현

    public virtual void TakeDamage(int damage)
    {
        if (isInvincible || isDead) return;

        health -= damage;
    }

    public virtual void Die()
    {
        isDead = true;
        collider.enabled = false;
        StartCoroutine(DieRoutine());
    }

    private IEnumerator DieRoutine()
    {
        yield return new WaitForSeconds(1.0f);
        OnDeath?.Invoke();
        PoolManager.Instance.MonsterPool.Return(type, this);
    }
}
