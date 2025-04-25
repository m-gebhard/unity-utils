using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityUtils.Extensions;

namespace UnityUtils.UI
{
    /// <summary>
    /// A class that automatically scrolls a ScrollRect to ensure the selected UI element is visible.
    /// </summary>
    [RequireComponent(typeof(ScrollRect))]
    public class ScrollRectAutoScroll : MonoBehaviour
    {
        /// <summary>
        /// The RectTransform of the viewport.
        /// </summary>
        [SerializeField]
        private RectTransform viewportRectTransform;

        /// <summary>
        /// The RectTransform of the content.
        /// </summary>
        [SerializeField]
        private RectTransform contentRectTransform;

        /// <summary>
        /// The offset to apply when scrolling.
        /// </summary>
        [SerializeField]
        private float offset;

        /// <summary>
        /// The initial vertical scroll position of the ScrollRect,
        /// where 1 represents the top and 0 represents the bottom.
        /// </summary>
        [SerializeField]
        private float initialScroll = 1f;

        /// <summary>
        /// The ScrollRect component to control.
        /// </summary>
        private ScrollRect scrollRect;

        /// <summary>
        /// The event system used to get the currently selected UI element.
        /// </summary>
        private UnityEngine.EventSystems.EventSystem eventSystem;

        /// <summary>
        /// A cache for storing RectTransform of GameObjects.
        /// </summary>
        private readonly Dictionary<GameObject, RectTransform> objectRectCache = new();

        /// <summary>
        /// Initializes the ScrollRect component.
        /// </summary>
        private void Awake()
        {
            scrollRect = GetComponent<ScrollRect>();
            eventSystem = UnityEngine.EventSystems.EventSystem.current;
        }

        /// <summary>
        /// Sets the initial scroll position to the top of the ScrollRect.
        /// </summary>
        private void Start()
        {
            scrollRect.verticalNormalizedPosition = initialScroll;
        }

        /// <summary>
        /// Called every frame to update the scroll position if necessary.
        /// </summary>
        private void Update()
        {
            GameObject selected = eventSystem.currentSelectedGameObject;
            if (selected == null || !selected.transform.IsChildOf(contentRectTransform)) return;

            // Get the RectTransform of the selected object.
            if (!objectRectCache.TryGetValue(selected, out RectTransform selectedRectTransform))
            {
                selectedRectTransform = selected.GetComponent<RectTransform>();
                objectRectCache.Add(selected, selectedRectTransform);
            }

            // Convert the selected object's rect to the viewport's local space.
            Rect selectedRectViewport = selectedRectTransform.rect
                .ToWorldSpace(selectedRectTransform)
                .ToLocalSpace(viewportRectTransform);

            Rect viewportRect = viewportRectTransform.rect;

            // Calculate how much the selected object is outside the viewport.
            float outsideOnTop = Mathf.Max(0, selectedRectViewport.yMax - viewportRect.yMax);
            float outsideOnBottom = Mathf.Max(0, viewportRect.yMin - selectedRectViewport.yMin);

            // Determine how much we need to scroll to bring the object into view.
            float delta = outsideOnTop > 0 ? outsideOnTop : -outsideOnBottom;
            if (delta == 0) return;

            // Convert the content's rect to viewport local space.
            Rect contentRectViewport = contentRectTransform.rect
                .ToWorldSpace(contentRectTransform)
                .ToLocalSpace(viewportRectTransform);

            float overflow = contentRectViewport.height - viewportRect.height;
            float unitsToNormalized = 1 / overflow;

            // Adjust the scroll position.
            scrollRect.verticalNormalizedPosition += (delta + (delta > 0 ? offset : -offset)) * unitsToNormalized;
        }
    }
}