namespace UnityUtils.DecisionTree
{
    /// <summary>
    /// Defines the interface for a strategy in a behavior tree.
    /// </summary>
    public interface IStrategy
    {
        /// <summary>
        /// Processes the strategy.
        /// </summary>
        /// <param name="deltaTime">The time elapsed since the last process call.</param>
        /// <returns>The status of the process.</returns>
        public Node.Status Process(float deltaTime);

        /// <summary>
        /// Resets the strategy.
        /// </summary>
        public void Reset()
        {
        }
    }
}