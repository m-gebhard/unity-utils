using System;

namespace UnityUtils.DecisionTree
{
    /// <summary>
    /// Represents a leaf node in a tree structure that processes a strategy.
    /// </summary>
    public class Leaf : Node
    {
        /// <summary>
        /// The strategy associated with this leaf node.
        /// </summary>
        private readonly IStrategy strategy;

        /// <summary>
        /// Gets the type of the strategy.
        /// </summary>
        public Type StrategyType => strategy.GetType();

        /// <summary>
        /// Initializes a new instance of the <see cref="Leaf"/> class.
        /// </summary>
        /// <param name="name">The name of the node.</param>
        /// <param name="nodeStrategy">The strategy to be used by this node.</param>
        /// <param name="nodePriority">The priority of the node.</param>
        public Leaf(string name, IStrategy nodeStrategy, int nodePriority = 0) : base(name, nodePriority)
        {
            strategy = nodeStrategy;
        }

        /// <summary>
        /// Processes the strategy for this node.
        /// </summary>
        /// <param name="deltaTime">The time elapsed since the last process call.</param>
        /// <returns>The status of the process.</returns>
        public override Status Process(float deltaTime)
        {
            Status status = strategy.Process(deltaTime);
            LastExecutionStatus = status;
            return status;
        }

        /// <summary>
        /// Resets the strategy of this node.
        /// </summary>
        public override void Reset() => strategy.Reset();
    }
}