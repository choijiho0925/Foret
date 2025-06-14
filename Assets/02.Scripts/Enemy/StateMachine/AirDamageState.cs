using UnityEngine;

public class AirDamageState : IState
{
    private MonsterBase monster;
    private float stunDuration = 0.5f;
    private float timer;

    public AirDamageState(MonsterBase monster)
    {
        this.monster = monster;
    }

    public void Enter()
    {
        // Damage 상태 진입
        monster.AnimationHandler.Damage();
        timer = 0;
    }

    public void Exit()
    {
        // Damage 상태 해제
    }

    public void Update()
    {
        // 데미지 처리
        timer += Time.deltaTime;
        if (timer >= stunDuration)
        {
            // 일정 시간 뒤 다시 대기 상태로 전환
            monster.StateMachine.ChangeState(new AirIdleState(monster));
        }
    }
}
