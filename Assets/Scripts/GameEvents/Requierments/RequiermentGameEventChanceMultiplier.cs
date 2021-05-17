using UnityEngine;

namespace trollschmiede.CivIdle.GameEventSys
{
    [CreateAssetMenu(fileName = "New Requirement GameEvent Chance Multiplier", menuName = "Scriptable Objects/Requierments/Requierment GameEvent Chance Multiplier")]
    public class RequiermentGameEventChanceMultiplier : Requierment
    {
        [SerializeField] GameEvent gameEvent = null;
        [SerializeField] float multiplier = 1f;
        [SerializeField] bool isMax = false;

        public override bool CheckRequierment()
        {
            if (gameEvent.GetMultiplier() >= multiplier && !isMax)
            {
                return true;
            } else if (isMax && gameEvent.GetMultiplier() <= multiplier)
            {
                return true;
            }
            return false;
        }
        public override string GetRequiermentString()
        {
            return gameEvent.name + " Chance multiplier is at " + multiplier.ToString();
        }
    }
}