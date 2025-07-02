using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityUtils.GameObjects;

namespace UnityUtils.DependencyInjection
{
    /// <summary>
    /// Manages dependency injection for MonoBehaviours in the Unity scene.
    /// </summary>
    [DefaultExecutionOrder(-1000)]
    public sealed class InjectionManager : EphemeralSingleton<InjectionManager>
    {
        /// <summary>
        /// Called when the script instance is being loaded.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();

            IEnumerable<ProviderManager> providerManager = FindMonoBehaviours().OfType<ProviderManager>();
            bool hasProviderManager = providerManager.Any();

            if (!hasProviderManager)
            {
                throw new Exception("[DependencyInjection] ProviderManager not found in the scene.");
            }

            // Find all MonoBehaviours that have fields or methods with the Inject attribute.
            IEnumerable<MonoBehaviour> injectables = FindMonoBehaviours().Where(IsInjectable);

            // Iterate through all injectable behaviours and inject dependencies.
            foreach (MonoBehaviour injectable in injectables)
            {
                Inject(injectable);
            }
        }

        /// <summary>
        /// Injects dependencies into the specified class instance.
        /// </summary>
        /// <param name="classInstance">The class instance to inject dependencies into.</param>
        private void Inject(object classInstance)
        {
            // Get the class type.
            Type type = classInstance.GetType();

            // Inject fields and methods.
            InjectFields(classInstance, type);
            InjectMethods(classInstance, type);
        }

        /// <summary>
        /// Injects dependencies into the fields of the specified class instance.
        /// </summary>
        /// <param name="classInstance">The class instance to inject dependencies into.</param>
        /// <param name="type">The type of the class instance.</param>
        private void InjectFields(object classInstance, Type type)
        {
            // Find all fields that have the Inject attribute.
            IEnumerable<FieldInfo> injectableFields =
                type.GetFields(ProviderManager.BindingFlags).Where(HasInjectableAttribute);

            foreach (FieldInfo field in injectableFields)
            {
                // Get field type, try get resolved instance from ProviderManager.
                Type fieldType = field.FieldType;
                object result = ProviderManager.Instance.GetFromRegistry(fieldType);

                if (result == null)
                {
                    throw new Exception(
                        $"[DependencyInjection] Failed to inject Field {fieldType.Name} into {type.Name}.");
                }

                // Updates the field value in the instance.
                field.SetValue(classInstance, result);
            }
        }

        /// <summary>
        /// Injects dependencies into the methods of the specified class instance.
        /// </summary>
        /// <param name="className">The class instance to inject dependencies into.</param>
        /// <param name="type">The type of the class instance.</param>
        private void InjectMethods(object className, Type type)
        {
            // Find all methods that have the Inject attribute.
            IEnumerable<MethodInfo> injectableMethods =
                type.GetMethods(ProviderManager.BindingFlags)
                    .Where(HasInjectableAttribute);

            foreach (MethodInfo method in injectableMethods)
            {
                Type[] requiredParameters = method
                    .GetParameters()
                    .Select(parameter => parameter.ParameterType)
                    .ToArray();

                object[] result = requiredParameters
                    .Select(ProviderManager.Instance.GetFromRegistry)
                    .ToArray();

                if (result.Any(r => r == null))
                {
                    throw new Exception($"[DependencyInjection] Failed to inject Method {type.Name}.{method.Name}.");
                }
            }
        }

        /// <summary>
        /// Determines if the specified MonoBehaviour has injectable members.
        /// </summary>
        /// <param name="obj">The MonoBehaviour to check.</param>
        /// <returns>True if the MonoBehaviour has injectable members; otherwise, false.</returns>
        public bool IsInjectable(MonoBehaviour obj) =>
            obj.GetType()
                .GetMembers(ProviderManager.BindingFlags)
                .Any(HasInjectableAttribute);

        /// <summary>
        /// Determines if the specified member has the Inject attribute.
        /// </summary>
        /// <param name="member">The member to check.</param>
        /// <returns>True if the member has the Inject attribute; otherwise, false.</returns>
        private static bool HasInjectableAttribute(MemberInfo member) =>
            Attribute.IsDefined(member, typeof(InjectAttribute));

        /// <summary>
        /// Finds all MonoBehaviours in the scene.
        /// </summary>
        /// <returns>An array of MonoBehaviours in the scene.</returns>
        private static MonoBehaviour[] FindMonoBehaviours() =>
            FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);
    }
}