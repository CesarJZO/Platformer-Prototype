namespace StatePattern
{
    public class StateMachine
    {
        public State CurrentState { get; private set; }
        public State LastState { get; private set; }

        public StateMachine(State initialState)
        {
            LastState = CurrentState = initialState;
            CurrentState.Start();
        }

        public void ChangeState(State state)
        {
            if (state == null) return;

            CurrentState.Exit();
            LastState = CurrentState;

            CurrentState = state;
            CurrentState.Start();
        }
    }
}
