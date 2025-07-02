using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityUtils.GameObjects;

namespace UnityUtils.SceneManagement
{
    /// <summary>
    /// Manages scene loading operations in Unity.
    /// </summary>
    public class SceneLoader : PersistentSingleton<SceneLoader>
    {
        /// <summary>
        /// Event triggered when a scene change starts.
        /// </summary>
        public Action<int> OnSceneChangeStart;

        /// <summary>
        /// Event triggered to update the progress of a scene change.
        /// </summary>
        public Action<float> OnSceneChangeProgressUpdate;

        /// <summary>
        /// Event triggered when a scene change finishes.
        /// </summary>
        public Action<int> OnSceneChangeFinished;

        /// <summary>
        /// The asynchronous operation for loading a scene.
        /// </summary>
        private AsyncOperation operation;

        /// <summary>
        /// Loads a scene asynchronously.
        /// </summary>
        /// <param name="sceneIndex">The index of the scene to load.</param>
        /// <param name="callback">Optional callback to invoke after the scene is loaded.</param>
        /// <returns>An IEnumerator for the asynchronous operation.</returns>
        private IEnumerator LoadSceneAsync(int sceneIndex, Action callback = null)
        {
            operation = SceneManager.LoadSceneAsync(sceneIndex);
            OnSceneChangeStart?.Invoke(sceneIndex);

            while (operation is { isDone: false })
            {
                OnSceneChangeProgressUpdate?.Invoke(operation.progress);
                yield return null;
            }

            OnSceneChangeFinished?.Invoke(sceneIndex);
            callback?.Invoke();
        }

        /// <summary>
        /// Starts the asynchronous scene loading process.
        /// </summary>
        /// <param name="sceneIndex">The index of the scene to load.</param>
        /// <param name="callback">Optional callback to invoke after the scene is loaded.</param>
        /// <returns>A Coroutine for the asynchronous operation.</returns>
        public Coroutine LoadScene(int sceneIndex, Action callback = null)
        {
            return StartCoroutine(LoadSceneAsync(sceneIndex, callback));
        }
    }
}