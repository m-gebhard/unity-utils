using System.Collections.Generic;
using UnityEngine;

namespace UnityUtils.UI
{
    /// <summary>
    /// Abstract class for feeding dynamic list items.
    /// </summary>
    public abstract class DynamicListFeeder : MonoBehaviour
    {
        /// <summary>
        /// Gets the list of dynamic list items.
        /// </summary>
        /// <returns>A list of dynamic list items.</returns>
        public abstract List<DynamicListItem> GetItems();
    }
}