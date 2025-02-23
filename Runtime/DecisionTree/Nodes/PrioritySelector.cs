using System.Collections.Generic;
using System.Linq;
using UnityUtils.Extensions;

namespace UnityUtils.DecisionTree
{
    /// <summary>
    /// Represents a priority selector in a behavior tree.
    /// </summary>
    public class PrioritySelector : Selector
    {
        /// <summary>
        /// The list of sorted child nodes.
        /// </summary>
        private List<Node> sortedChildren;

        /// <summary>
        /// Gets the sorted child nodes, sorting them if necessary.
        /// </summary>
        private List<Node> SortedChildren => sortedChildren ??= SortChildren();

        /// <summary>
        /// Sorts the child nodes by their priority in descending order.
        /// </summary>
        /// <returns>A list of sorted child nodes.</returns>
        protected virtual List<Node> SortChildren() =>
            Children.Clone().OrderByDescending(child => child.Priority).ToList();

        /// <summary>
        /// Initializes a new instance of the <see cref="PrioritySelector"/> class.
        /// </summary>
        /// <param name="name">The name of the selector.</param>
        public PrioritySelector(string name) : base(name)
        {
        }

        /// <summary>
        /// Processes the priority selector.
        /// </summary>
        /// <param name="deltaTime">The time elapsed since the last process call.</param>
        /// <returns>The status of the process.</returns>
        public override Status Process(float deltaTime)
        {
            foreach (Node child in SortedChildren)
            {
                Status status = child.Process(deltaTime);

                switch (status)
                {
                    case Status.Success:
                        return Status.Success;
                    case Status.Running:
                        return Status.Running;
                    case Status.Failure:
                    default:
                        continue;
                }
            }

            return Status.Success;
        }

        /// <summary>
        /// Resets the priority selector.
        /// </summary>
        public override void Reset()
        {
            base.Reset();
            sortedChildren = null;
        }
    }
}