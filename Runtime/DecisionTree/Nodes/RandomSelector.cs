using System.Collections.Generic;
using UnityUtils.Extensions;

namespace UnityUtils.DecisionTree
{
    /// <summary>
    /// Represents a random selector in a behavior tree.
    /// </summary>
    public class RandomSelector : PrioritySelector
    {
        /// <summary>
        /// Sorts the child nodes by shuffling them randomly.
        /// </summary>
        /// <returns>A list of shuffled child nodes.</returns>
        protected override List<Node> SortChildren() => Children.Clone().Shuffle();

        /// <summary>
        /// Initializes a new instance of the <see cref="RandomSelector"/> class.
        /// </summary>
        /// <param name="name">The name of the selector.</param>
        public RandomSelector(string name) : base(name)
        {
        }
    }
}