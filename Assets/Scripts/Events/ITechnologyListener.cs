using trollschmiede.CivIdle.ScienceSys;

namespace trollschmiede.CivIdle.EventSys
{
    public interface ITechnologyListener : IEventListener
    {
        void Evoke(Technology technology);
    }
}

