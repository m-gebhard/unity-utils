namespace UnityUtils.DecisionTree
{
    /// <summary>
    /// Represents a selector node in a behavior tree.
    /// </summary>
    public class Selector : Node
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Selector"/> class.
        /// </summary>
        /// <param name="name">The name of the selector.</param>
        /// <param name="nodePriority">The priority of the node.</param>
        public Selector(string name, int nodePriority = 0) : base(name, nodePriority)
        {
        }

        /// <summary>
        /// Processes the selector node.
        /// </summary>
        /// <param name="deltaTime">The time elapsed since the last process call.</param>
        /// <returns>The status of the process.</returns>
        public override Status Process(float deltaTime)
        {
            if (CurrentChildIndex < Children.Count)
            {
                Status status = Children[CurrentChildIndex].Process(deltaTime);
                LastExecutionStatus = status;

                switch (status)
                {
                    case Status.Running:
                        return Status.Running;
                    case Status.Success:
                        Reset();
                        return Status.Success;
                    default:
                        CurrentChildIndex++;
                        return Status.Running;
                }
            }

            Reset();
            return Status.Success;
        }
    }
}