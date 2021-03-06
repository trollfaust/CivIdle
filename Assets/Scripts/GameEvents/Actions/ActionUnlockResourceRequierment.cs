using UnityEngine;
using trollschmiede.CivIdle.Resources;

namespace trollschmiede.CivIdle.GameEvents
{
    [CreateAssetMenu(fileName = "New Action Unlock Resource Requierment", menuName = "Scriptable Objects/Actions/Action Unlock Resource Requierment")]
    public class ActionUnlockResourceRequierment : Action
    {
        [SerializeField] ResoureRequierment resoureRequierment = ResoureRequierment.Start;

        public override int EvokeAction()
        {
            ResourceManager.instance.AddRequierment(resoureRequierment);
            return 0;
        }
        public override string GetActionString()
        {
            //return "Unlocks Resource Requierment " + resoureRequierment.ToString().Replace("_", "-");
            return string.Empty;
        }
    }
}

