namespace UnityUtils.DecisionTree
{
    /// <summary>
    /// Represents an inverter node in a behavior tree.
    /// </summary>
    public class Inverter : Node
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Inverter"/> class.
        /// </summary>
        /// <param name="name">The name of the node.</param>
        /// <param name="nodePriority">The priority of the node.</param>
        public Inverter(string name, int nodePriority = 0) : base(name, nodePriority)
        {
        }

        /// <summary>
        /// Processes the inverter node.
        /// </summary>
        /// <param name="deltaTime">The time elapsed since the last process call.</param>
        /// <returns>The status of the process.</returns>
        public override Status Process(float deltaTime)
        {
            if (CurrentChildIndex < Children.Count)
            {
                Status status = Children[CurrentChildIndex].Process(deltaTime);

                switch (status)
                {
                    case Status.Success:
                        return Status.Failure;
                    case Status.Running:
                        return Status.Running;
                    case Status.Failure:
                        return Status.Success;
                    default:
                        return Status.Failure;
                }
            }

            Reset();
            return Status.Success;
        }
    }
}