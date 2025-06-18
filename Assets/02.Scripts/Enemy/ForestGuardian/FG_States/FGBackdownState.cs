using System.Collections;
using UnityEngine;

/// <summary>
/// 플레이어와의 거리를 재고 회피하는 상태 클래스
/// </summary>
public class FGBackdownState : IState
{
    private ForestGuardian boss;
    private float backdownTotalDistance = 5f;    // 총 이동 거리
    private float backdownSpeedMultiplier = 2f; // 속도 배수
    private Coroutine backdownCoroutine;

    public FGBackdownState(ForestGuardian boss)
    {
        this.boss = boss;
    }

    public void Enter() 
    {
        if(!boss.hasPlayedFirstBGM)
        {
            boss.AudioChanger.PlayFirstBossBGM();
            boss.hasPlayedFirstBGM = true;
        }
        boss.FGSFX.PlayBackdownClip();
        boss.SetAllowLookAtPlayer(false);
        boss.ResetAllAnimation();
        boss.PlayRunAnimation();

        backdownCoroutine = boss.StartCoroutine(BackdownRoutine());
    }

    public void Exit() 
    {
        boss.SetAllowLookAtPlayer(true);
        boss.ResetAllAnimation();
    }

    public void Update()
    {
    }

    private IEnumerator BackdownRoutine()
    {
        Vector3 playerPos = boss.Player.transform.position;
        float moveSpeed = boss.MoveSpeed * backdownSpeedMultiplier;
        float movedDistance = 0f;

        float dirToPlayer = Mathf.Sign(boss.transform.position.x - playerPos.x);
        float primaryDirection = dirToPlayer;
        float secondaryDirection = -dirToPlayer;

        // 시작 전에 두 방향 검사
        bool primaryClear = boss.CanBackdown(primaryDirection, 4f);
        bool secondaryClear = boss.CanBackdown(secondaryDirection, 4f);

        if (!primaryClear && !secondaryClear)
        {
            boss.StateMachine.ChangeState(new FGDecisionState(boss));
            yield break;
        }

        float direction = primaryClear ? primaryDirection : secondaryDirection;
        bool triedOpposite = !primaryClear; // 이미 반대로 시작했으면 true

        while (movedDistance < backdownTotalDistance)
        {
            float step = moveSpeed * Time.deltaTime;
            if (movedDistance + step > backdownTotalDistance)
                step = backdownTotalDistance - movedDistance;

            bool moved = boss.TryBackdownMove(direction, step);
            if (moved)
            {
                movedDistance += step;
            }
            else if (!triedOpposite)
            {
                // 처음 이동 중 막혔을 경우, 반대 방향으로 한 번 시도
                direction = -direction;
                triedOpposite = true;
            }
            else
            {
                break;
            }

            yield return null;
        }

        boss.StateMachine.ChangeState(new FGDecisionState(boss));
    }
}
