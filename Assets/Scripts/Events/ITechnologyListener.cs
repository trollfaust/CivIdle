using trollschmiede.CivIdle.Science;

namespace trollschmiede.CivIdle.Events
{
    public interface ITechnologyListener : IEventListener
    {
        void Evoke(Technology technology);
    }
}

