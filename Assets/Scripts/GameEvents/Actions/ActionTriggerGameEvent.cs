using UnityEngine;

namespace trollschmiede.CivIdle.GameEvents
{
    [CreateAssetMenu(fileName = "New Action Trigger GameEvent", menuName = "Scriptable Objects/Actions/Action Trigger GameEvent")]
    public class ActionTriggerGameEvent : Action
    {
        [SerializeField] GameEvent gameEvent = null;

        public override bool EvokeAction()
        {
            GameEventManager.instance.TriggerGameEvent(gameEvent);
            return true;
        }
        public override string GetActionString()
        {
            return "Triggers " + gameEvent.name;
        }
        public override int GetLastValue()
        {
            return base.GetLastValue();
        }
    }
}


