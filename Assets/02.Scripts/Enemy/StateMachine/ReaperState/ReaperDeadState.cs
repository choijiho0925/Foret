using System.Threading;
using UnityEngine;

public class ReaperDeadState : IState
{
    private Reaper boss;

    public ReaperDeadState(Reaper boss)
    {
        this.boss = boss;
    }

    public void Enter()
    {
        // Dead 상태 진입
        boss.BossAnimationHandler.Dead();
        boss.Die();

        // 페이드 아웃 시작
        FadeController.Instance.FadeOut(() =>
        {
            // 페이드 인
            FadeController.Instance.FadeIn();
        });
    }

    public void Exit()
    {
        // Dead 상태 해제
    }

    public void Update()
    {
        
    }
}
