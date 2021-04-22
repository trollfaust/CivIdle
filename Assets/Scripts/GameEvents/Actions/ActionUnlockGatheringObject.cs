using UnityEngine;
using trollschmiede.CivIdle.Resources;

namespace trollschmiede.CivIdle.GameEvents
{
    [CreateAssetMenu(fileName = "New Action Unlock Gathering Object", menuName = "Scriptable Objects/Actions/Action Unlock Gathering Object")]
    public class ActionUnlockGatheringObject : Action
    {
        [SerializeField] GatheringObject gatheringObject = null;

        public override int EvokeAction()
        {
            GatheringManager.instance.EnableGatheringObject(gatheringObject);
            return 0;
        }
        public override string GetActionString()
        {
            return "Unlocks Work Task " + gatheringObject.name;
        }
    }
}

