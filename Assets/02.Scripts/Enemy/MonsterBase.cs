using UnityEngine;
using UnityEngine.AI;

public abstract class MonsterBase : MonoBehaviour
{
    [SerializeField] private float moveSpeed;       // 이동 속도
    [SerializeField] private float detectionRange;  // 감지 범위
    [SerializeField] private float attackPower;     // 공격력
    [SerializeField] private float attackRange;     // 공격 범위
    [SerializeField] private bool isGround;         // 지상 몹인지 확인

    private GameObject player;                      // 플레이어
    private MonsterStateMachine stateMachine;       // 상태머신
    private NavMeshAgent navMeshAgent;              // NavMeshAgent 참조 추가

    // 프로퍼티
    public float MoveSpeed => moveSpeed;
    public float DetectionRange => detectionRange;
    public float AttackPower => attackPower;
    public float AttackRange => attackRange;
    public bool IsGround => isGround;
    public GameObject Player => player;
    public MonsterStateMachine StateMachine => stateMachine;
    public NavMeshAgent NavMeshAgent => navMeshAgent;

    protected virtual void Awake()
    {
        player = GameObject.FindWithTag("Player");
        stateMachine = new MonsterStateMachine(this);
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
        stateMachine?.Update();
    }

    public abstract void Move();    // 지상 / 공중 따른 이동 구현
    public abstract void Attack();  // 근거리 / 원거리 따른 공격 구현
}
