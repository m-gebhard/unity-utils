using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityUtils.DecisionTree;
using GUI = UnityEngine.GUI;
using Handles = UnityEditor.Handles;

namespace UnityUtils.Editor
{
    /// <summary>
    /// Represents a window for viewing decision trees in the Unity editor.
    /// </summary>
    public class DecisionTreeWindow : EditorWindow
    {
        /// <summary>
        /// The object representing the decision tree.
        /// </summary>
        private object treeObject;

        /// <summary>
        /// A dictionary mapping nodes to their positions in the window.
        /// </summary>
        private readonly Dictionary<Node, Rect> nodePositions = new();

        /// <summary>
        /// The offset for positioning nodes.
        /// </summary>
        private readonly Vector2 offset = new(300, 100);

        /// <summary>
        /// The scroll position of the window.
        /// </summary>
        private Vector2 scrollPosition;

        /// <summary>
        /// Shows the decision tree viewer window.
        /// </summary>
        [MenuItem("Window/Decision Tree Viewer")]
        public static void ShowWindow()
        {
            GetWindow<DecisionTreeWindow>("Decision Tree Viewer").Show();
        }

        /// <summary>
        /// Draws the GUI for the window.
        /// </summary>
        private void OnGUI()
        {
            EditorGUILayout.LabelField(
                "Drag any object here that exposes a RootNode or DecisionTree property",
                EditorStyles.boldLabel);
            EditorGUILayout.TextArea("", EditorStyles.boldLabel);

            DragAndDropGUI();

            if (treeObject == null)
            {
                return;
            }

            Node rootNode = GetRootNode();
            if (rootNode == null)
            {
                EditorGUILayout.LabelField("No valid tree found!", EditorStyles.boldLabel);
                return;
            }

            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            BeginWindows();

            nodePositions.Clear();
            DrawNode(rootNode, position.width / 2, 50);

            EndWindows();
            EditorGUILayout.EndScrollView();
        }

        /// <summary>
        /// Updates the window.
        /// </summary>
        private void Update()
        {
            Repaint();
        }

        /// <summary>
        /// Handles the drag and drop GUI for the window.
        /// </summary>
        private void DragAndDropGUI()
        {
            Event evt = Event.current;
            Rect dropArea = GUILayoutUtility.GetRect(0, 50, GUILayout.ExpandWidth(true));
            GUI.Box(dropArea, "Drag a object here", EditorStyles.helpBox);

            if (evt.type != EventType.DragUpdated && evt.type != EventType.DragPerform) return;
            if (!dropArea.Contains(evt.mousePosition))
                return;

            DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

            if (evt.type == EventType.DragPerform)
            {
                DragAndDrop.AcceptDrag();
                treeObject = DragAndDrop.objectReferences.FirstOrDefault();
                Repaint();
            }

            evt.Use();
        }

        /// <summary>
        /// Gets the root node of the decision tree.
        /// </summary>
        /// <returns>The root node of the decision tree.</returns>
        private Node GetRootNode()
        {
            if (treeObject == null)
                return null;

            PropertyInfo rootNodeProperty = treeObject.GetType().GetProperty("RootNode");
            if (rootNodeProperty == null)
            {
                rootNodeProperty = treeObject.GetType().GetProperty("DecisionTree");
            }

            return rootNodeProperty?.GetValue(treeObject) as Node;
        }

        /// <summary>
        /// Draws a node in the window.
        /// </summary>
        /// <param name="node">The node to draw.</param>
        /// <param name="x">The x-coordinate of the node.</param>
        /// <param name="y">The y-coordinate of the node.</param>
        private void DrawNode(Node node, float x, float y)
        {
            if (node == null) return;

            Rect nodeRect = new(x, y, 150, 50);
            nodePositions[node] = nodeRect;

            Color originalColor = GUI.backgroundColor;
            GUI.backgroundColor = GetNodeColor(node);

            string statusIcon = node.LastExecutionStatus switch
            {
                Node.Status.Running => "\u23fa",
                Node.Status.Failure => "\u274c",
                Node.Status.Success => "\u2705",
                _ => "",
            };

            nodePositions[node] = GUI.Window(
                node.GetHashCode(),
                nodeRect,
                (_) =>
                {
                    GUI.Label(new Rect(10, 25, 130, 20), $"{node.Name} {statusIcon}", EditorStyles.boldLabel);
                    GUI.DragWindow();
                },
                GetNodeDisplayName(node)
            );

            GUI.backgroundColor = originalColor;

            if (IsNodeActive(node))
            {
                Handles.color = Color.green;
                Handles.DrawSolidRectangleWithOutline(nodePositions[node], new Color(1, 1, 0, 0.2f), Color.yellow);
                Handles.color = Color.white;
            }

            DrawChildNodes(node, x, y);
        }

        /// <summary>
        /// Draws the child nodes of a node.
        /// </summary>
        /// <param name="node">The parent node.</param>
        /// <param name="x">The x-coordinate of the parent node.</param>
        /// <param name="y">The y-coordinate of the parent node.</param>
        private void DrawChildNodes(Node node, float x, float y)
        {
            float childX, childY;

            if (node is Sequence)
            {
                childX = x;
                childY = y + offset.y;

                foreach (Node child in node.Children)
                {
                    DrawNode(child, childX, childY);
                    DrawLine(nodePositions[node].center, nodePositions[child].center);
                    childY += offset.y;
                }
            }
            else
            {
                childX = x - (node.Children.Count - 1) * offset.x / 2f;
                childY = y + offset.y;

                foreach (Node child in node.Children)
                {
                    DrawNode(child, childX, childY);
                    DrawLine(nodePositions[node].center, nodePositions[child].center);
                    childX += offset.x;
                }
            }
        }

        /// <summary>
        /// Determines if a node is active.
        /// </summary>
        /// <param name="node">The node to check.</param>
        /// <returns>True if the node is active, otherwise false.</returns>
        private bool IsNodeActive(Node node)
        {
            Node rootNode = GetRootNode();
            return rootNode != null && rootNode.GetActiveChildren().Contains(node);
        }

        /// <summary>
        /// Gets the display name of a node.
        /// </summary>
        /// <param name="node">The node to get the display name for.</param>
        /// <returns>The display name of the node.</returns>
        private static string GetNodeDisplayName(Node node) =>
            node is Leaf leaf ? leaf.StrategyType.Name : node.GetType().Name;

        /// <summary>
        /// Gets the color for a node.
        /// </summary>
        /// <param name="node">The node to get the color for.</param>
        /// <returns>The color of the node.</returns>
        private static Color GetNodeColor(Node node) => node switch
        {
            DecisionTree.DecisionTree => Color.grey,
            Leaf leaf when leaf.StrategyType == typeof(Condition) => Color.cyan,
            Leaf leaf when leaf.StrategyType == typeof(Execution) => Color.red,
            Leaf => Color.yellow,
            Selector => Color.green,
            _ => Color.gray,
        };

        /// <summary>
        /// Draws a line between two points.
        /// </summary>
        /// <param name="start">The start point of the line.</param>
        /// <param name="end">The end point of the line.</param>
        private static void DrawLine(Vector2 start, Vector2 end)
        {
            Handles.BeginGUI();
            Handles.DrawBezier(
                start,
                end,
                start + Vector2.down * 50,
                end + Vector2.up * 50,
                Color.white,
                null,
                2f);
            Handles.EndGUI();
        }
    }
}