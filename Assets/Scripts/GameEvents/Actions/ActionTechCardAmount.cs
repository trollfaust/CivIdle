using UnityEngine;
using trollschmiede.CivIdle.Science;

namespace trollschmiede.CivIdle.GameEvents
{
    [CreateAssetMenu(fileName = "New Action Tech Card Amount", menuName = "Scriptable Objects/Actions/Action Tech Card Amount")]
    public class ActionTechCardAmount : Action
    {
        [SerializeField] int changeAmount = 0;

        int lastValue = 0;

        public override bool EvokeAction()
        {
            ScienceManager.instance.ChangeTechCardsToChoose(changeAmount);
            lastValue = changeAmount;
            return true;
        }
        public override string GetActionString()
        {
            return "Techcards to choose " + changeAmount.ToString();
        }
        public override int GetLastValue()
        {
            return lastValue;
        }
    }
}
