using UnityEngine;
using trollschmiede.CivIdle.Resources;

namespace trollschmiede.CivIdle.GameEvents
{
    [CreateAssetMenu(fileName = "New Action Unlock Resource", menuName = "Scriptable Objects/Actions/Action Unlock Resource")]
    public class ActionUnlockResource : Action
    {
        [SerializeField] Resource resource = null;

        public override int EvokeAction()
        {
            ResourceManager.instance.ActivateResource(resource);
            return 0;
        }
    }
}

