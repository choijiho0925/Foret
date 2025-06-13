using UnityEngine.AI;

public class Bat : MonsterBase
{
    private NavMeshAgent agent;

    protected override void Awake()
    {
        base.Awake();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    public override void Move()
    {
        if (Player != null)
        {
            agent.SetDestination(Player.transform.position);
        }
    }

    public override void Attack()
    {
        
    }
}
