namespace Player
{
    public class AttackState : PlayerState
    {
        public AttackState(PlayerController player) : base(player) { }

        
        
        public override string ToString() => nameof(AttackState);
    }
}
