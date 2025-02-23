namespace UnityUtils.DecisionTree
{
    /// <summary>
    /// Represents the root of the decision tree.
    /// </summary>
    public class DecisionTree : Node
    {
        /// <summary>
        /// Gets the name of the decision tree.
        /// </summary>
        public new string Name { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DecisionTree"/> class.
        /// </summary>
        /// <param name="treeName">The name of the decision tree.</param>
        public DecisionTree(string treeName) : base(treeName)
        {
            Name = treeName;
            CurrentChildIndex = 0;
        }

        /// <summary>
        /// Processes the decision tree node.
        /// </summary>
        /// <param name="deltaTime">The time elapsed since the last process call.</param>
        /// <returns>The status of the process.</returns>
        public override Status Process(float deltaTime)
        {
            if (Children.Count == 0)
                return Status.Failure;

            bool loopedOnce = false;

            while (true)
            {
                Status status = Children[CurrentChildIndex].Process(deltaTime);

                if (status == Status.Failure)
                {
                    CurrentChildIndex = 0;
                    return status;
                }

                if (status == Status.Running)
                {
                    return Status.Running;
                }

                CurrentChildIndex++;
                if (CurrentChildIndex >= Children.Count)
                {
                    CurrentChildIndex = 0;
                    loopedOnce = true;
                }

                if (loopedOnce)
                    break;
            }

            Reset();
            return Status.Success;
        }

        /// <summary>
        /// Resets the decision tree node.
        /// </summary>
        public override void Reset()
        {
            CurrentChildIndex = 0;

            foreach (Node child in Children)
            {
                child.Reset();
            }
        }
    }
}