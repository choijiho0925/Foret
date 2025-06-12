using System.Collections;
using UnityEngine;

public class ForestGuardian : BossBase
{
    [Header("���� ���� ����")]
    [SerializeField] private float meleeRange = 2f;
    [SerializeField] private float teleportDistance = 2f;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override IEnumerator GoPattern()
    {
        // ���� ���� �ִٸ� ����, ���ٸ� �����̵�
        float distance = Vector3.Distance(transform.position, Player.transform.position);

        // ������ ����
        if(distance <= meleeRange)
        {
            yield return MeleeAttack();
        }

        // �ָ� �ڷ���Ʈ
        else
        {
            yield return Teleport();
        }

        yield return new WaitForSeconds(patternDelay);
        EndPattern();
    }

    private IEnumerator MeleeAttack()
    {
        Debug.Log("���� ���� �õ�");

        Attack();
        yield return new WaitForSeconds(1f);
    }

    private IEnumerator Teleport()
    {
        Debug.Log("�����̵�");

        // �÷��̾� ���� ���� ���
        Vector3 toPlayer = (transform.position - Player.transform.position).normalized;
        Vector3 behindPosition = Player.transform.position + toPlayer * teleportDistance;

        // �����̵�
        transform.position = behindPosition;

        yield return new WaitForSeconds(1f);
    }
}
