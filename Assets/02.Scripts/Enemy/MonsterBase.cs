using UnityEngine;

public abstract class MonsterBase : MonoBehaviour, IDamagable
{
    [Header("기본 스텟")]
    [SerializeField] private int health;          // 몬스터 체력
    [SerializeField] private float moveSpeed;       // 이동 속도
    [SerializeField] private float detectionRange;  // 감지 범위
    [SerializeField] private int attackPower;     // 공격력
    [SerializeField] private float attackRange;     // 공격 범위
    [SerializeField] private bool isGround;         // 지상 몹인지 확인
    [SerializeField] private bool isBoss;           // 보스 몹인지 확인

    private GameObject player;                      // 플레이어
    private MonsterStateMachine stateMachine;       // 상태머신
    private MonsterAnimationHandler animationHandler;   // 몬스터 애니메이션
    private SpriteRenderer spriteRenderer;              // 몬스터 이미지

    #region 프로퍼티
    public int Health { get => health; set => health = value; }
    public float MoveSpeed => moveSpeed;
    public float DetectionRange => detectionRange;
    public int AttackPower => attackPower;
    public float AttackRange => attackRange;
    public bool IsGround => isGround;
    public bool IsBoss => isBoss;
    public GameObject Player => player;
    public MonsterStateMachine StateMachine => stateMachine;
    public MonsterAnimationHandler AnimationHandler => animationHandler;
    public SpriteRenderer SpriteRenderer => spriteRenderer;
    #endregion

    protected virtual void Awake()
    {
        player = GameObject.FindWithTag("Player");
        stateMachine = new MonsterStateMachine(this);
        animationHandler = GetComponent<MonsterAnimationHandler>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    protected virtual void Start()
    {
        if (!isBoss)
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
            return;
        }
    }

    protected virtual void Update()
    {
        stateMachine?.Update();
    }

    public virtual void Move()  // 지상 / 공중 따른 이동 구현
    {

    }    

    public abstract void Attack();  // 근거리 / 원거리 따른 공격 구현

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            if (isGround)
                // 지상 몬스터 죽음 상태 전환
                return;
            else
                stateMachine.ChangeState(new AirDeadState(this));

            return;
        }

        animationHandler.Damage();
        if (isGround)
            // 지상 몬스터 상태 전환
            return;
        else
            stateMachine.ChangeState(new AirDamageState(this));
    }

    public void Die()
    {
        Destroy(gameObject, 1.0f);
    }
}
