using System;
using System.Collections.Generic;

namespace UnityUtils.DecisionTree
{
    /// <summary>
    /// Represents a node in a behavior tree.
    /// </summary>
    public class Node
    {
        /// <summary>
        /// Gets the name of the node.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the priority of the node.
        /// </summary>
        public int Priority { get; }

        /// <summary>
        /// Gets the list of child nodes.
        /// </summary>
        public List<Node> Children { get; } = new();

        /// <summary>
        /// Gets or sets the last execution status of the node.
        /// </summary>
        public Status LastExecutionStatus { get; protected set; } = Status.None;

        /// <summary>
        /// The index of the current child node being processed.
        /// </summary>
        protected int CurrentChildIndex;

        /// <summary>
        /// Initializes a new instance of the <see cref="Node"/> class.
        /// </summary>
        /// <param name="nodeName">The name of the node.</param>
        /// <param name="nodePriority">The priority of the node.</param>
        protected Node(string nodeName = "Node", int nodePriority = 0)
        {
            Name = nodeName;
            Priority = nodePriority;
        }

        /// <summary>
        /// Adds a child node to this node.
        /// </summary>
        /// <param name="child">The child node to add.</param>
        /// <returns>The current node instance.</returns>
        public Node AddChild(Node child)
        {
            Children.Add(child);
            return this;
        }

        /// <summary>
        /// Processes the node.
        /// </summary>
        /// <param name="deltaTime">The time elapsed since the last process call.</param>
        /// <returns>The status of the process.</returns>
        public virtual Status Process(float deltaTime)
        {
            Status status = Children[CurrentChildIndex].Process(deltaTime);

            LastExecutionStatus = status;
            return status;
        }

        /// <summary>
        /// Resets the node and all its children.
        /// </summary>
        public virtual void Reset()
        {
            CurrentChildIndex = 0;

            foreach (Node child in Children)
            {
                child.Reset();
            }
        }

        /// <summary>
        /// Gets the active children of the node.
        /// </summary>
        /// <param name="start">The starting node to search from.</param>
        /// <returns>A list of active child nodes.</returns>
        public List<Node> GetActiveChildren(Node start = null)
        {
            start ??= this;

            List<Node> activeChildren = new List<Node>();

            if (start.LastExecutionStatus == Status.Running)
            {
                activeChildren.Add(start);
            }

            foreach (Node child in start.Children)
            {
                if (child.LastExecutionStatus == Status.Running)
                {
                    activeChildren.Add(child);
                }

                activeChildren.AddRange(GetActiveChildren(child));
            }

            return activeChildren;
        }

        /// <summary>
        /// Builder class for constructing nodes.
        /// </summary>
        /// <typeparam name="T">The type of node to build.</typeparam>
        public class Builder<T> where T : Node
        {
            private readonly T node;

            /// <summary>
            /// Initializes a new instance of the <see cref="Builder{T}"/> class.
            /// </summary>
            /// <param name="nodeInstance">The node instance to build.</param>
            public Builder(T nodeInstance)
            {
                node = nodeInstance;
            }

            /// <summary>
            /// Adds a child node to the builder.
            /// </summary>
            /// <param name="child">The child node to add.</param>
            /// <returns>The current builder instance.</returns>
            public Builder<T> AddChild(Node child)
            {
                node.AddChild(child);
                return this;
            }

            /// <summary>
            /// Builds the node.
            /// </summary>
            /// <returns>The built node.</returns>
            public T Build() => node;
        }

        /// <summary>
        /// Represents the status of a node.
        /// </summary>
        [Serializable]
        public enum Status
        {
            None,
            Success,
            Failure,
            Running,
        }
    }
}