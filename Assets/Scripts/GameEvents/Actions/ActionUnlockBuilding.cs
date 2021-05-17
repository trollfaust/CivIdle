using trollschmiede.CivIdle.BuildingSys;
using UnityEngine;

namespace trollschmiede.CivIdle.GameEventSys
{
    [CreateAssetMenu(fileName = "New Action Unlock Building", menuName = "Scriptable Objects/Actions/Action Unlock Building")]
    public class ActionUnlockBuilding : Action
    {
        [SerializeField] Building building = null;

        public override bool EvokeAction()
        {
            BuildingManager.instance.EnableBuilding(building);
            return true;
        }
        public override string GetActionString()
        {
            return "Unlocks Building " + building.name;
        }
        public override int GetLastValue()
        {
            return base.GetLastValue();
        }
    }
}

