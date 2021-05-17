using trollschmiede.CivIdle.GameEventSys;

namespace trollschmiede.CivIdle.EventSys
{
    public interface IGameEventListener : IEventListener
    {
        void Evoke(GameEvent gameEvent);
    }
}

