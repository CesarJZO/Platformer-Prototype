﻿namespace StatePattern
{
    public class StateMachine
    {
        public State CurrentState { get; private set; }

        public StateMachine(State state)
        {
            CurrentState = state;
            CurrentState.Start();
        }

        public void ChangeState(State state)
        {
            CurrentState.Exit();
            CurrentState = state;
            CurrentState.Start();
        }
    }
}