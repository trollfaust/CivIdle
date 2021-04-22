using UnityEngine;

namespace trollschmiede.CivIdle.GameEvents
{
    [CreateAssetMenu(fileName = "New Requirement GameEvent Count", menuName = "Scriptable Objects/Requierments/Requierment GameEvent Count")]
    public class RequiermentGameEventCount : Requierment
    {
        [SerializeField] GameEvent gameEvent = null;
        [SerializeField] int countDone = 0;

        public override bool CheckRequierment()
        {
            if (gameEvent.GetCountDone() >= countDone)
            {
                return true;
            }
            return false;
        }
        public override string GetRequiermentString()
        {
            return gameEvent.name + " " + countDone.ToString() + " times happend";
        }
    }
}