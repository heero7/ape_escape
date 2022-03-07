using System;
using System.Collections.Generic;

namespace ApeEscape.FSM
{
    public class StateMachine
    {
        public IState CurrentState { get; private set; }
        
        private readonly List<StateTransition> _anyStateTransitions = new List<StateTransition>();
        private readonly List<StateTransition> _stateTransitions = new List<StateTransition>();

        public void Tick()
        {
            var transition = CheckForTransition();
            
            if (transition != null)
            {
                SetState(transition.To);
            }
            
            CurrentState.OnUpdate();
        }
        
        public void AddTransition(IState from, IState to, Func<bool> condition)
        {
            var stateTransition = new StateTransition(from, to , condition);
            _stateTransitions.Add(stateTransition);
        }

        public void AddAnyTransition(IState to, Func<bool> condition)
        {
            var stateTransition = new StateTransition(null, to , condition);
            _anyStateTransitions.Add(stateTransition);
        }
        
        public void SetState(IState state)
        {
            if (CurrentState == state) return;
            
            CurrentState?.OnExit();
            
            CurrentState = state;
            
            CurrentState.OnEnter();
        }

        private StateTransition CheckForTransition()
        {
            // Check for the anyTransitions
            foreach (var transition in _anyStateTransitions)
            {
                if (transition.Condition())
                {
                    return transition;
                }
            }
            
            foreach (var transition in _stateTransitions)
            {
                if (transition.From == CurrentState && transition.Condition())
                {
                    return transition;
                }
            }
            
            return null;
        }
    }
}