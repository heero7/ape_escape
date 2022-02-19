﻿namespace ApeEscape.FSM
{
    public interface IState
    {
        void OnEnter();
        void OnUpdate();
        void OnExit();
    }
}