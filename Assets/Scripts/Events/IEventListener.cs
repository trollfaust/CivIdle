using trollschmiede.CivIdle.Resources;

namespace trollschmiede.CivIdle.Events
{
    public interface IEventListener {
        void Evoke();
        void Evoke(Resource resource);
    }
}