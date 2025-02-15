using System;
using System.Collections.Generic;

/// <summary>
/// Represents an observable field that notifies subscribers when its value changes.
/// </summary>
/// <typeparam name="T">The type of the value being observed.</typeparam>
public sealed class ObservableField<T>
{
    /// <summary>
    /// The current value of the observable field.
    /// </summary>
    private T observeValue;

    /// <summary>
    /// Action to be invoked when the value changes.
    /// </summary>
    public Action<T> OnChange;

    /// <summary>
    /// Initializes a new instance of the <see cref="ObservableField{T}"/> class with an optional initial value.
    /// </summary>
    /// <param name="initialValue">The initial value of the observable field.</param>
    public ObservableField(T initialValue = default)
    {
        observeValue = initialValue;
    }

    /// <summary>
    /// Gets or sets the value of the observable field.
    /// </summary>
    public T Value
    {
        get => observeValue;
        set
        {
            if (EqualityComparer<T>.Default.Equals(value, observeValue)) return;

            observeValue = value;
            OnChange?.Invoke(value);
        }
    }
}