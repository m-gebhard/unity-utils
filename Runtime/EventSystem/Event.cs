namespace UnityUtils.EventSystem
{
    /// <summary>
    /// Interface for events with a specific event channel.
    /// </summary>
    /// <typeparam name="TChannel">The type of the event channel.</typeparam>
    public interface IEvent<TChannel> where TChannel : EventChannel<TChannel>
    {
    }
}