using System.Collections;
using UnityEngine;

public class DBUpState : IState
{
    private DeathBringer boss;

    public DBUpState(DeathBringer boss)
    {
        this.boss = boss;
    }

    public void Enter()
    {
        boss.StartCoroutine(BossUpState());
        GameManager.Instance.isFirstPhaseEnd = true;
    }

    public void Exit()
    {

    }

    public void Update()
    {
        boss.ReaperCameraMove.CameraMove();
    }

    private IEnumerator BossUpState()
    {
        // 보스 2페이즈 위로 이동
        boss.BossAnimationHandler.BossUp();

        yield return new WaitForSeconds(5f);
        boss.RemoveGameobject();
    }
}
