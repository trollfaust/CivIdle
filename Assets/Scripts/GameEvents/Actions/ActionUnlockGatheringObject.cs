using UnityEngine;
using trollschmiede.CivIdle.ResourceSys;

namespace trollschmiede.CivIdle.GameEventSys
{
    [CreateAssetMenu(fileName = "New Action Unlock Gathering Object", menuName = "Scriptable Objects/Actions/Action Unlock Gathering Object")]
    public class ActionUnlockGatheringObject : Action
    {
        [SerializeField] GatheringObject gatheringObject = null;

        public override bool EvokeAction()
        {
            GatheringManager.instance.EnableGatheringObject(gatheringObject);
            return true;
        }
        public override string GetActionString()
        {
            return "Unlocks Work Task " + gatheringObject.name;
        }
        public override int GetLastValue()
        {
            return base.GetLastValue();
        }
    }
}

