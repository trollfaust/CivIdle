using UnityEngine;

namespace trollschmiede.CivIdle.GameEvents
{
    [CreateAssetMenu(fileName = "New Action GameEvent Chance Modifier", menuName = "Scriptable Objects/Actions/Action GameEvent Chance Modifier")]
    public class ActionGameEventChanceModifier : Action
    {
        [SerializeField] GameEvent gameEvent = null;
        [SerializeField] [Range(0f,5f)] float multiplier = 1f;

        public override int EvokeAction()
        {
            gameEvent.SetChanceMultiplier(multiplier);
            return 0;
        }
        public override string GetActionString()
        {
            return "Multiplies " + gameEvent.name + " Chance by " + multiplier.ToString();
        }
    }
}

