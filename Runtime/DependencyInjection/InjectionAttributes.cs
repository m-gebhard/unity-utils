using System;

namespace UnityUtils.DependencyInjection
{
    /// <summary>
    /// Attribute to mark fields or methods for dependency injection.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Method)]
    public sealed class InjectAttribute : Attribute
    {
    }

    /// <summary>
    /// Attribute to mark methods as providers for dependency injection.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class ProviderAttribute : Attribute
    {
    }

    /// <summary>
    /// Interface for dependency providers.
    /// </summary>
    public interface IDependencyProvider
    {
    }
}