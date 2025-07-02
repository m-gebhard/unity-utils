using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace UnityUtils.Editor
{
    /// <summary>
    /// Abstract base class for creating custom property drawers in the Unity Editor.
    /// This class provides functionality for drawing custom properties and buttons in the inspector.
    /// </summary>
    /// <remarks>Make sure the deriving class has a [CustomEditor(typeof(T))] attribute.</remarks>
    /// <typeparam name="T">The type of the target component this editor is associated with.</typeparam>
    public abstract class CustomEditorDrawer<T> : UnityEditor.Editor where T : UnityEngine.Object
    {
        /// <summary>
        /// Gets the target component being edited.
        /// </summary>
        protected T Target => (T)target;

        /// <summary>
        /// Gets the label displayed in the custom editor section.
        /// </summary>
        protected virtual string Label { get; } = "Custom Editor";

        /// <summary>
        /// A list of custom buttons to display in the editor, each with a label and a callback action.
        /// </summary>
        protected virtual List<(string, Action)> EditorButtons { get; } = new();

        /// <summary>
        /// Called by Unity to draw the inspector GUI for the target component.
        /// </summary>
        public override void OnInspectorGUI()
        {
            // Update the serialized object to reflect any changes.
            serializedObject.Update();

            // Draw the default inspector for the target component.
            DrawDefaultInspector();

            // Add spacing and a custom label for the custom editor section.
            EditorGUILayout.Space();
            EditorGUILayout.LabelField(Label, EditorStyles.boldLabel);

            // Draw custom properties and buttons.
            DrawProperties();
            DrawButtons();

            // Apply any modified properties to the serialized object.
            serializedObject.ApplyModifiedProperties();
        }

        /// <summary>
        /// Draws the custom buttons defined in <see cref="EditorButtons"/>.
        /// </summary>
        private void DrawButtons()
        {
            foreach ((string text, Action callback) in EditorButtons)
            {
                // Create a button for each entry and invoke its callback when clicked.
                if (GUILayout.Button(text)) callback?.Invoke();
            }
        }

        /// <summary>
        /// Abstract method for drawing custom properties in the inspector.
        /// Must be implemented by derived classes.
        /// </summary>
        protected abstract void DrawProperties();
    }
}