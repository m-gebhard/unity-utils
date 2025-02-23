using System;

namespace UnityUtils.DecisionTree
{
    /// <summary>
    /// Represents a condition in a behavior tree.
    /// </summary>
    public class Condition : IStrategy
    {
        /// <summary>
        /// The predicate function that determines the condition.
        /// </summary>
        private readonly Func<bool> predicate;

        /// <summary>
        /// Initializes a new instance of the <see cref="Condition"/> class.
        /// </summary>
        /// <param name="strategyPredicate">The predicate function to evaluate.</param>
        public Condition(Func<bool> strategyPredicate)
        {
            predicate = strategyPredicate;
        }

        /// <summary>
        /// Processes the condition.
        /// </summary>
        /// <param name="deltaTime">The time elapsed since the last process call.</param>
        /// <returns>The status of the process.</returns>
        public Node.Status Process(float deltaTime) => predicate()
            ? Node.Status.Success
            : Node.Status.Failure;
    }
}