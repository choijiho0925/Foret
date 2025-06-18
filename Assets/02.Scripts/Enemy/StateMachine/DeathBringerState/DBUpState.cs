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
