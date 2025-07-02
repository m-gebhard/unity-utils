using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityUtils.GameObjects;

namespace UnityUtils.DependencyInjection
{
    /// <summary>
    /// Manages the registration and retrieval of dependency providers.
    /// </summary>
    [DefaultExecutionOrder(-1100)]
    public sealed class ProviderManager : EphemeralSingleton<ProviderManager>
    {
        /// <summary>
        /// Binding flags used to search for fields and methods.
        /// </summary>
        public static readonly BindingFlags BindingFlags =
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        /// <summary>
        /// Dictionary to store registered providers.
        /// </summary>
        private readonly Dictionary<Type, object> registry = new();

        /// <summary>
        /// Called when the script instance is being loaded.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            // Find all MonoBehaviours that implement IDependencyProvider.
            IEnumerable<IDependencyProvider> providers =
                FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<IDependencyProvider>();

            // Register them as providers.
            foreach (IDependencyProvider provider in providers)
            {
                RegisterProvider(provider);
            }
        }

        /// <summary>
        /// Registers a provider by invoking its methods marked with the Provider attribute.
        /// </summary>
        /// <param name="provider">The provider to register.</param>
        private void RegisterProvider(IDependencyProvider provider)
        {
            // Get all class methods of the provider.
            MethodInfo[] methods = provider.GetType().GetMethods(BindingFlags);

            foreach (MethodInfo method in methods)
            {
                // Skip the method if the Provider attribute is not set.
                if (!Attribute.IsDefined(method, typeof(ProviderAttribute))) continue;

                // Invoke method and store returned instance in the registry.
                Type returnType = method.ReturnType;
                object returnedInstance = method.Invoke(provider, null);

                if (returnedInstance != null)
                {
                    // Add a dictionary entry with the instance type and the returned instance.
                    registry.Add(returnType, returnedInstance);
                }
                else
                {
                    throw new Exception(
                        $"[DependencyInjection] Provider {provider.GetType().Name} returned null for {returnType.Name}.");
                }
            }
        }

        /// <summary>
        /// Retrieves an instance from the registry based on the specified type.
        /// </summary>
        /// <param name="type">The type of the instance to retrieve.</param>
        /// <returns>The instance of the specified type, or null if not found.</returns>
        public object GetFromRegistry(Type type)
        {
            registry.TryGetValue(type, out object result);
            return result;
        }
    }
}