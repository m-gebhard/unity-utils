using System.Linq;

namespace UnityUtils.StateMachine
{
    /// <summary>
    /// Represents a state machine that manages state transitions and updates.
    /// </summary>
    public class StateMachine
    {
        public StateNode CurrentState { get; private set; }

        private readonly StateMachineNodeManager nodeManager = new();

        /// <summary>
        /// Updates the state machine, checking for transitions and updating the current state.
        /// </summary>
        public void Update()
        {
            ITransition transition = GetNextStateTransition();

            if (transition != null)
            {
                ChangeState(transition.TargetState);
            }

            CurrentState?.State?.Update();
        }

        /// <summary>
        /// Calls the FixedUpdate method on the current state.
        /// </summary>
        public void FixedUpdate()
        {
            CurrentState?.State?.FixedUpdate();
        }

        /// <summary>
        /// Sets the current state of the state machine.
        /// </summary>
        /// <param name="state">The new state to set.</param>
        public void SetState(IState state)
        {
            CurrentState = nodeManager.Nodes[state.GetType()];
            CurrentState.State?.Enter();
        }

        /// <summary>
        /// Changes the current state to a new state.
        /// </summary>
        /// <param name="state">The new state to change to.</param>
        private void ChangeState(IState state)
        {
            if (state == CurrentState?.State)
            {
                return;
            }

            IState previousState = CurrentState?.State;
            StateNode newStateNode = nodeManager.Nodes[state.GetType()];
            IState newState = newStateNode.State;

            previousState?.Exit();
            newState?.Enter();

            CurrentState = newStateNode;
        }

        /// <summary>
        /// Adds a transition from one state to another with a specified predicate.
        /// </summary>
        /// <param name="from">The state to transition from.</param>
        /// <param name="to">The state to transition to.</param>
        /// <param name="predicate">The predicate that determines if the transition should occur.</param>
        public void AddTransition(IState from, IState to, IPredicate predicate) =>
            nodeManager.AddTransition(from, to, predicate);

        /// <summary>
        /// Adds a transition from any state to a specified state with a specified predicate.
        /// </summary>
        /// <param name="to">The state to transition to.</param>
        /// <param name="predicate">The predicate that determines if the transition should occur.</param>
        public void AddAnyTransition(IState to, IPredicate predicate) =>
            nodeManager.AddAnyTransition(to, predicate);

        /// <summary>
        /// Gets the next state transition based on the current state and any transitions.
        /// </summary>
        /// <returns>The next state transition if one is found; otherwise, null.</returns>
        private ITransition GetNextStateTransition()
        {
            ITransition possibleAnyTransition =
                nodeManager.AnyTransitions.FirstOrDefault(t => t.Predicate.Evaluate());

            return possibleAnyTransition ?? CurrentState?.Transitions?.FirstOrDefault(t => t.Predicate.Evaluate());
        }
    }
}