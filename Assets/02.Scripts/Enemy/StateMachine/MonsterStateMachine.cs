public class MonsterStateMachine : StateMachine
{
    public MonsterBase Monster { get; }

    public MonsterStateMachine(MonsterBase monster)
    {
        this.Monster = monster;
    }
}
