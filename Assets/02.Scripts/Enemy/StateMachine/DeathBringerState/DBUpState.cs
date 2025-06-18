using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class DBUpState : IState
{
    private DeathBringer boss;
    private bool isOneTime = true;

    public DBUpState(DeathBringer boss)
    {
        this.boss = boss;
    }

    public void Enter()
    {
        boss.StartCoroutine(BossUpState());
        GameManager.Instance.isFirstPhaseEnd = true;
        EventBus.Raise(new BossClearEvent());
    }

    public void Exit()
    {

    }

    public void Update()
    {

    }

    private IEnumerator BossUpState()
    {
        // 보스 2페이즈 위로 이동
        boss.BossAnimationHandler.BossUp();

        yield return new WaitForSeconds(5f);
        boss.RemoveGameobject();
    }
}
