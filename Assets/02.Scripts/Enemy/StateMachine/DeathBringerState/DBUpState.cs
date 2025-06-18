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
        Debug.Log("보스 위로 이동 상태");
        boss.BossAnimationHandler.BossUp();
        boss.StartCoroutine(BossUpState());
        GameManager.Instance.isFirstPhaseEnd = true;
    }

    public void Exit()
    {

    }

    public void Update()
    {
        //boss.ReaperCameraMove.CameraMove();
    }

    private IEnumerator BossUpState()
    {
        yield return new WaitForSeconds(5f);
        boss.RemoveGameobject();
    }
}
