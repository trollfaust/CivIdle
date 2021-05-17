using trollschmiede.CivIdle.ResourceSys;

namespace trollschmiede.CivIdle.EventSys
{
    public interface IResourceEventListener : IEventListener
    {
        void Evoke(Resource resource);
    }
}

