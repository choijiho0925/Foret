public class MonsterStateMachine : StateMachine
{
    public MonsterBase Monster { get; }

    public BossBase Boss { get; }

    public MonsterStateMachine(MonsterBase monster)
    {
        this.Monster = monster;
    }

    public MonsterStateMachine(BossBase boss)
    {
        this.Boss = boss;
    }
}
