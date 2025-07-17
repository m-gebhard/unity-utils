using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

namespace UnityUtils.Extensions
{
    /// <summary>
    /// Provides extension methods for the Camera.
    /// </summary>
    public static class CameraExtension
    {
        /// <summary>
        /// Captures the current camera view and invokes a callback with the image data as a byte array.
        /// </summary>
        /// <param name="camera">The Camera instance to capture the view from.</param>
        /// <param name="callback">The callback to invoke with the captured image data.</param>
        /// <param name="excludedLayers">The layers to exclude from the camera capture. Defaults to none.</param>
        public static void Capture(this Camera camera, Action<byte[]> callback, LayerMask excludedLayers = default)
        {
            MonoBehaviour mono = camera.gameObject.GetComponent<MonoBehaviour>();
            if (!mono)
            {
                Debug.LogError(
                    "Camera must be attached to a GameObject with a MonoBehaviour component to use Capture.");
                return;
            }

            mono.StartCoroutine(RecordFrame(camera, callback, excludedLayers));
        }

        /// <summary>
        /// Coroutine that captures the current frame of the camera and invokes the callback with the image data.
        /// </summary>
        /// <param name="camera">The Camera instance to capture the view from.</param>
        /// <param name="callback">The callback to invoke with the captured image data.</param>
        /// <param name="excludedLayers">The layers to exclude from the camera capture.</param>
        /// <returns>An IEnumerator for the coroutine.</returns>
        private static IEnumerator RecordFrame(Camera camera, Action<byte[]> callback, LayerMask excludedLayers)
        {
            // Stores the original culling mask of the camera
            int originalMask = camera.cullingMask;
            camera.cullingMask = originalMask & ~excludedLayers;

            bool captured = false;
            byte[] pngData = null;

            // Subscribes to the endCameraRendering event to capture the frame
            RenderPipelineManager.endCameraRendering += OnEndCameraRendering;

            // Waits for the end of the frame before capturing
            yield return new WaitForEndOfFrame();

            // Restores the original culling mask of the camera
            camera.cullingMask = originalMask;

            // Invokes the callback with the captured image data
            callback(pngData ?? Array.Empty<byte>());
            yield break;

            void OnEndCameraRendering(ScriptableRenderContext ctx, Camera cam)
            {
                // Ensures the event is triggered only for the specified camera and only once
                if (cam != camera || captured) return;
                captured = true;

                // Captures the rendered frame as a texture and encodes it to PNG format
                RenderTexture activeRT = RenderTexture.active;
                Texture2D tex = new(Screen.width, Screen.height, TextureFormat.RGB24, false);
                tex.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
                tex.Apply();

                pngData = tex.EncodeToPNG();

                // Cleans up resources and unsubscribes from the event
                UnityEngine.Object.Destroy(tex);
                RenderPipelineManager.endCameraRendering -= OnEndCameraRendering;
                RenderTexture.active = activeRT;
            }
        }
    }
}