using UnityEngine;

public class AirIdleState : IState
{
    private MonsterBase monster;

    public AirIdleState(MonsterBase monster)
    {
        this.monster = monster;
    }

    public void Enter()
    {
        // Idle ���� ����
    }

    public void Exit()
    {
        // Idle ���� ����
    }

    public void Update()
    {
        // �÷��̾� Ž�� �� ���� ��ȯ
        float distance = Vector3.Distance(monster.transform.position, monster.Player.transform.position);
        if (distance < monster.DetectionRange)
        {
            
        }
    }
}
