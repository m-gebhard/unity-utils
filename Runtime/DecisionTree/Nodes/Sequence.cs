namespace UnityUtils.DecisionTree
{
    /// <summary>
    /// Represents a sequence node in a behavior tree.
    /// </summary>
    public class Sequence : Node
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Sequence"/> class.
        /// </summary>
        /// <param name="name">The name of the sequence.</param>
        /// <param name="nodePriority">The priority of the node.</param>
        public Sequence(string name, int nodePriority = 0) : base(name, nodePriority)
        {
        }

        /// <summary>
        /// Processes the sequence node.
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
                    case Status.Failure:
                        Reset();
                        return Status.Failure;
                    case Status.Running:
                        return Status.Running;
                    default:
                        CurrentChildIndex++;
                        return CurrentChildIndex == Children.Count
                            ? Status.Success
                            : Status.Running;
                }
            }

            Reset();
            return Status.Success;
        }
    }
}