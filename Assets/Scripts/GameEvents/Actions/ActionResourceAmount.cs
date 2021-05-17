using UnityEngine;
using trollschmiede.CivIdle.ResourceSys;

namespace trollschmiede.CivIdle.GameEventSys
{
    [CreateAssetMenu(fileName = "New Action Resource Amount", menuName = "Scriptable Objects/Actions/Action Resource Amount")]
    public class ActionResourceAmount : Action
    {
        public Resource resourcesToChange;
        public int resourceChangeAmountMin;
        public int resourceChangeAmountMax;

        int lastValue = 0;

        public override bool EvokeAction()
        {
            int tempResourceChangeAmountMin = resourceChangeAmountMin;
            int tempResourceChangeAmountMax = resourceChangeAmountMax;
            if (overrideValue != 0)
            {
                tempResourceChangeAmountMin = overrideValue;
                tempResourceChangeAmountMax = overrideValue;
            }
            int randomAmount = Random.Range(tempResourceChangeAmountMin, tempResourceChangeAmountMax + 1);
            if (resourcesToChange.GetMaxAmount() > 0 && resourcesToChange.amount + randomAmount > resourcesToChange.GetTempMaxAmount())
            {
                randomAmount = resourcesToChange.GetTempMaxAmount() - resourcesToChange.amount;
            }
            if (resourcesToChange.AmountChange(randomAmount))
            {
                lastValue = randomAmount;
                return true;
            }
            lastValue = 0;
            return false;
        }
        public override string GetActionString()
        {
            return "Changes " + resourcesToChange.name + " Amount by min " + resourceChangeAmountMin.ToString() + " and max " + resourceChangeAmountMax.ToString();
        }
        public override int GetLastValue()
        {
            return lastValue;
        }
    }
}

