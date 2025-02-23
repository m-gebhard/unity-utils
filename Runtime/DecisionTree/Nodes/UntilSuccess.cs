namespace UnityUtils.DecisionTree
{
    /// <summary>
    /// Represents a node that continues processing until a child node succeeds.
    /// </summary>
    public class UntilSuccess : Node
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UntilSuccess"/> class.
        /// </summary>
        /// <param name="name">The name of the node.</param>
        /// <param name="nodePriority">The priority of the node.</param>
        public UntilSuccess(string name, int nodePriority = 0) : base(name, nodePriority)
        {
        }

        /// <summary>
        /// Processes the node, continuing until a child node succeeds.
        /// </summary>
        /// <param name="deltaTime">The time elapsed since the last process call.</param>
        /// <returns>The status of the process.</returns>
        public override Status Process(float deltaTime)
        {
            if (Children[0].Process(deltaTime) == Status.Success)
            {
                Reset();
                return Status.Success;
            }

            return Status.Running;
        }
    }
}