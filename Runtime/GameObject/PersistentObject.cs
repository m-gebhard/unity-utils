using System;
using UnityEditor;
using UnityEngine;

namespace UnityUtils.GameObjects
{
    [ExecuteInEditMode]
    public class PersistentObject : MonoBehaviour
    {
        public string guid;

#if UNITY_EDITOR
        /// <summary>
        /// Create a new unique ID for this object when it's created
        /// </summary>
        private void Awake()
        {
            if (Application.platform != RuntimePlatform.WindowsEditor)
            {
                guid = Guid.NewGuid().ToString();
                PrefabUtility.RecordPrefabInstancePropertyModifications(this);
            }
        }

        /// <summary>
        /// Set the GUID if the object has none assigned
        /// </summary>
        private void Update()
        {
            if (String.IsNullOrEmpty(guid))
            {
                guid = Guid.NewGuid().ToString();
                PrefabUtility.RecordPrefabInstancePropertyModifications(this);
            }
        }
#endif
    }
}