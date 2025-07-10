using System;

namespace UnityUtils.Data
{
    /// <summary>
    /// A generic class for managing and interpolating a value towards a target value over time.
    /// </summary>
    /// <typeparam name="T">The type of the value to interpolate.</typeparam>
    public sealed class LerpValue<T>
    {
        /// <summary>
        /// The current interpolated value.
        /// </summary>
        private T currentValue;

        /// <summary>
        /// The target value to interpolate towards.
        /// </summary>
        private T targetValue;

        /// <summary>
        /// The interpolation function used to calculate the interpolated value.
        /// </summary>
        private readonly Func<T, T, float, T> lerpFunc;

        /// <summary>
        /// The speed at which the value interpolates towards the target.
        /// </summary>
        private float lerpSpeed;

        /// <summary>
        /// Gets the current interpolated value.
        /// </summary>
        public T Value => currentValue;

        /// <summary>
        /// Gets the target value.
        /// </summary>
        public T Target => targetValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="LerpValue{T}"/> class.
        /// </summary>
        /// <param name="initialValue">The initial value of the interpolation.</param>
        /// <param name="lerpFunction">The interpolation function to use.</param>
        /// <param name="speed">The speed of interpolation (default is 5f).</param>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="lerpFunction"/> is null.</exception>
        public LerpValue(T initialValue, Func<T, T, float, T> lerpFunction, float speed = 5f)
        {
            currentValue = initialValue;
            targetValue = initialValue;
            lerpFunc = lerpFunction ?? throw new ArgumentNullException(nameof(lerpFunction));
            lerpSpeed = speed;
        }

        /// <summary>
        /// Sets a new target value for interpolation.
        /// </summary>
        /// <param name="newTarget">The new target value.</param>
        public void SetTarget(T newTarget)
        {
            targetValue = newTarget;
        }

        /// <summary>
        /// Immediately sets the current and target values to a specified value.
        /// </summary>
        /// <param name="newValue">The value to set.</param>
        public void ForceSet(T newValue)
        {
            currentValue = newValue;
            targetValue = newValue;
        }

        /// <summary>
        /// Sets the speed at which the value interpolates towards the target.
        /// </summary>
        /// <param name="newSpeed">The new interpolation speed.</param>
        public void SetLerpSpeed(float newSpeed)
        {
            lerpSpeed = newSpeed;
        }

        /// <summary>
        /// Updates the current value by interpolating towards the target value.
        /// </summary>
        /// <param name="deltaTime">The time elapsed since the last update.</param>
        public void Update(float deltaTime)
        {
            currentValue = lerpFunc(currentValue, targetValue, deltaTime * lerpSpeed);
        }
    }
}