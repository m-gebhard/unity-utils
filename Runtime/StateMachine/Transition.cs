namespace UnityUtils.StateMachine
{
    /// <summary>
    /// Represents a transition between states in a state machine.
    /// </summary>
    public interface ITransition
    {
        /// <summary>
        /// Gets the predicate that determines if the transition should occur.
        /// </summary>
        public IPredicate Predicate { get; }

        /// <summary>
        /// Gets the target state of the transition.
        /// </summary>
        public IState TargetState { get; }
    }

    /// <summary>
    /// Implements a transition between states in a state machine.
    /// </summary>
    public class Transition : ITransition
    {
        /// <summary>
        /// Gets the predicate that determines if the transition should occur.
        /// </summary>
        public IPredicate Predicate { get; }

        /// <summary>
        /// Gets the target state of the transition.
        /// </summary>
        public IState TargetState { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Transition"/> class.
        /// </summary>
        /// <param name="predicate">The predicate that determines if the transition should occur.</param>
        /// <param name="targetState">The target state of the transition.</param>
        public Transition(IPredicate predicate, IState targetState)
        {
            Predicate = predicate;
            TargetState = targetState;
        }
    }
}