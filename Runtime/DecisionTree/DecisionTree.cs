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
            Children[CurrentChildIndex].Process(deltaTime);
            CurrentChildIndex = (CurrentChildIndex + 1) % Children.Count;

            return Status.Running;
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