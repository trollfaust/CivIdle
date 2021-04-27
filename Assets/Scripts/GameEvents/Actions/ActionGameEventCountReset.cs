using UnityEngine;

namespace trollschmiede.CivIdle.GameEvents
{
    [CreateAssetMenu(fileName = "New Action GameEvent Count Reset", menuName = "Scriptable Objects/Actions/Action GameEvent Count Reset")]
    public class ActionGameEventCountReset : Action
    {
        [SerializeField] GameEvent gameEvent = null;

        public override int EvokeAction()
        {
            gameEvent.ResetCount();
            return 0;
        }
        public override string GetActionString()
        {
            return "Resets the count of " + gameEvent.name;
        }
    }
}

