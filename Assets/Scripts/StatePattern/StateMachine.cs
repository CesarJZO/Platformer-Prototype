namespace StatePattern
{
    public class StateMachine
    {
        public State CurrentState { get; private set; }

        public StateMachine(State initialState)
        {
            CurrentState = initialState;
            CurrentState.Start();
        }

        public void ChangeState(State state)
        {
            if (state == null) return;
            CurrentState.Exit();
            CurrentState = state;
            CurrentState.Start();
        }
    }
}
