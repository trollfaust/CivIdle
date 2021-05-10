using UnityEngine;

namespace trollschmiede.CivIdle.GameEvents
{
    [CreateAssetMenu(fileName = "New Action GameEvent Chance Modifier", menuName = "Scriptable Objects/Actions/Action GameEvent Chance Modifier")]
    public class ActionGameEventChanceModifier : Action
    {
        [SerializeField] GameEvent gameEvent = null;
        [SerializeField] [Range(0f,5f)] float multiplier = 1f;

        public override bool EvokeAction()
        {
            gameEvent.SetChanceMultiplier(multiplier);
            return true;
        }
        public override string GetActionString()
        {
            return "Multiplies " + gameEvent.name + " Chance by " + multiplier.ToString();
        }
        public override int GetLastValue()
        {
            return base.GetLastValue();
        }
    }
}


