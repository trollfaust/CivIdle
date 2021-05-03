using UnityEngine;
using trollschmiede.CivIdle.Resources;

namespace trollschmiede.CivIdle.GameEvents
{
    [CreateAssetMenu(fileName = "New Action Resource Max Amount", menuName = "Scriptable Objects/Actions/Action Resource Max Amount")]
    public class ActionResourceMaxAmount : Action
    {
        public Resource resourcesToChange;
        public int resourceChangeAmountMin;
        public int resourceChangeAmountMax;

        int lastValue = 0;

        public override bool EvokeAction()
        {
            int randomAmount = Random.Range(resourceChangeAmountMin, resourceChangeAmountMax + 1);
            resourcesToChange.MaxAmountChange(randomAmount);
            lastValue = randomAmount;
            return true;
        }
        public override string GetActionString()
        {
            return "Changes " + resourcesToChange.name + " max Amount by min " + resourceChangeAmountMin.ToString() + " and max " + resourceChangeAmountMax.ToString();
        }
        public override int GetLastValue()
        {
            return lastValue;
        }
    }
}

