namespace UnityUtils.StateMachine
{
    /// <summary>
    /// Represents a state in the state machine.
    /// </summary>
    public interface IState
    {
        /// <summary>
        /// Called when the state is entered.
        /// </summary>
        public void Enter();

        /// <summary>
        /// Called every frame to update the state.
        /// </summary>
        public void Update();

        /// <summary>
        /// Called at fixed intervals to update the state.
        /// </summary>
        public void FixedUpdate();

        /// <summary>
        /// Called when the state is exited.
        /// </summary>
        public void Exit();
    }
}