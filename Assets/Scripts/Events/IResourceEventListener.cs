using trollschmiede.CivIdle.Resources;

namespace trollschmiede.CivIdle.Events
{
    public interface IResourceEventListener : IEventListener
    {
        void Evoke(Resource resource);
    }
}

