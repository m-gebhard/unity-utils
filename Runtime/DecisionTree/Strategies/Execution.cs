using System;

namespace UnityUtils.DecisionTree
{
    /// <summary>
    /// Represents an action strategy in a behavior tree.
    /// </summary>
    public class Execution : IStrategy
    {
        /// <summary>
        /// The action to be executed by this strategy.
        /// </summary>
        private readonly Action action;

        /// <summary>
        /// Initializes a new instance of the <see cref="Execution"/> class.
        /// </summary>
        /// <param name="action">The action to be executed by this strategy.</param>
        public Execution(Action action)
        {
            this.action = action;
        }

        /// <summary>
        /// Processes the action strategy.
        /// </summary>
        /// <param name="deltaTime">The time elapsed since the last process call.</param>
        /// <returns>The status of the process.</returns>
        public Node.Status Process(float deltaTime)
        {
            action();
            return Node.Status.Success;
        }
    }
}