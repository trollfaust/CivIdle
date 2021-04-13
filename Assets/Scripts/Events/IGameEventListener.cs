using trollschmiede.CivIdle.GameEvents;

namespace trollschmiede.CivIdle.Events
{
    public interface IGameEventListener : IEventListener
    {
        void Evoke(GameEvent gameEvent);
    }
}

