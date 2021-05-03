using UnityEngine;

namespace trollschmiede.CivIdle.GameEvents
{
    [CreateAssetMenu(fileName = "New Action GameEvent Count Reset", menuName = "Scriptable Objects/Actions/Action GameEvent Count Reset")]
    public class ActionGameEventCountReset : Action
    {
        [SerializeField] GameEvent gameEvent = null;

        public override bool EvokeAction()
        {
            gameEvent.ResetCount();
            return true;
        }
        public override string GetActionString()
        {
            return "Resets the count of " + gameEvent.name;
        }
        public override int GetLastValue()
        {
            return base.GetLastValue();
        }
    }
}

