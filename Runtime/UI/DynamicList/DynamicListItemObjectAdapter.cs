using UnityEngine;

namespace UnityUtils.UI
{
    /// <summary>
    /// Abstract class for adapting dynamic list items in the UI.
    /// Has to be placed on the prefab of the list item.
    /// Is responsible for setting the UI of the list item.
    /// </summary>
    public abstract class DynamicListItemObjectAdapter : MonoBehaviour
    {
        /// <summary>
        /// Gets the RectTransform component of the list item.
        /// </summary>
        public RectTransform Rect { get; private set; }

        /// <summary>
        /// Sets the data for the dynamic list item.
        /// </summary>
        /// <param name="item">The data to set for the list item.</param>
        public abstract void SetData(DynamicListItem item);

        /// <summary>
        /// Called when the script instance is being loaded.
        /// </summary>
        private void Awake()
        {
            Rect = GetComponent<RectTransform>();
        }
    }
}