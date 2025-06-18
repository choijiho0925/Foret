using System.Collections;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        EventBus.Raise(new BossClearEvent());

        // 페이드 아웃 시작
        FadeController.Instance.FadeOut(() =>
        {
            // 페이드 인
            FadeController.Instance.FadeIn();
            boss.StartCoroutine(GameClearRoutine());
        });
    }

    public void Exit()
    {
        // Dead 상태 해제
    }

    public void Update()
    {
        
    }

    private IEnumerator GameClearRoutine()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("EndingCredit");
    }
}
