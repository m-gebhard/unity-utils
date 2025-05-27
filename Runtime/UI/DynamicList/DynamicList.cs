using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UnityUtils.UI
{
    /// <summary>
    /// Abstract class for creating a dynamic list of items in the UI.
    /// </summary>
    /// <typeparam name="T">Type of the list item, which must inherit from DynamicListItemObjectAdapter.</typeparam>
    public abstract class DynamicList<T> : MonoBehaviour where T : DynamicListItemObjectAdapter
    {
        /// <summary>
        /// Feeder that provides the list items.
        /// </summary>
        [Header("List Feeder")] [SerializeField]
        protected DynamicListFeeder feeder;

        /// <summary>
        /// Parent transform where the list items will be instantiated.
        /// </summary>
        [Header("UI")] [SerializeField] protected Transform listParent;

        /// <summary>
        /// Prefab for the list item.
        /// </summary>
        [SerializeField] protected T listItemPrefab;

        /// <summary>
        /// List of spawned list items.
        /// </summary>
        protected List<T> spawnedListItems = new();

        /// <summary>
        /// Called when the script instance is being loaded.
        /// </summary>
        protected virtual void Start()
        {
            BuildList();
        }

        /// <summary>
        /// Builds the list by spawning list items and adding them to the parent transform.
        /// </summary>
        protected virtual void BuildList()
        {
            List<DynamicListItem> items = feeder.GetItems();

            foreach (DynamicListItem item in items)
            {
                T spawnedItem = SpawnListItem(item);
                spawnedListItems.Add(spawnedItem);
            }

            foreach (T spawnedItem in spawnedListItems)
            {
                StartCoroutine(spawnedItem.Rect.RebuildLayout());
            }
        }

        /// <summary>
        /// Spawns a list item and sets its data.
        /// Can be overwritten to use pooling or other methods.
        /// </summary>
        /// <param name="item">The data for the list item.</param>
        /// <returns>The spawned list item.</returns>
        protected virtual T SpawnListItem(DynamicListItem item)
        {
            T listItem = Instantiate(listItemPrefab, listParent);
            listItem.SetData(item);

            return listItem;
        }

        /// <summary>
        /// Clears the list by destroying all spawned list items.
        /// </summary>
        protected virtual void ClearList()
        {
            foreach (T listItem in spawnedListItems)
            {
                Destroy(listItem.gameObject);
            }

            spawnedListItems.Clear();
        }

        /// <summary>
        /// Rebuilds the list by clearing and then building it again.
        /// </summary>
        public virtual void RebuildList()
        {
            ClearList();
            BuildList();
        }
    }

    /// <summary>
    /// Struct representing a dynamic list item.
    /// </summary>
    public struct DynamicListItem
    {
        /// <summary>
        /// Data associated with the list item.
        /// </summary>
        public Dictionary<string, object> Data;
    }
}