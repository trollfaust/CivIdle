using UnityEngine;
using trollschmiede.CivIdle.Resources;

namespace trollschmiede.CivIdle.GameEvents
{
    [CreateAssetMenu(fileName = "New Action Resource Amount", menuName = "Scriptable Objects/Actions/Action Resource Amount")]
    public class ActionResourceAmount : Action
    {
        public Resource resourcesToChange;
        public int resourceChangeAmountMin;
        public int resourceChangeAmountMax;

        public override int EvokeAction()
        {
            int tempResourceChangeAmountMin = resourceChangeAmountMin;
            int tempResourceChangeAmountMax = resourceChangeAmountMax;
            if (overrideValue != 0)
            {
                tempResourceChangeAmountMin = overrideValue;
                tempResourceChangeAmountMax = overrideValue;
            }
            int i = Random.Range(tempResourceChangeAmountMin, tempResourceChangeAmountMax + 1);
            if (resourcesToChange.maxAmount > 0 && resourcesToChange.amount + i > resourcesToChange.GetTempMaxAmount())
            {
                i = resourcesToChange.GetTempMaxAmount() - resourcesToChange.amount;
            }
            if (resourcesToChange.AmountChange(i))
                return i;
            return 0;
        }
        public override string GetActionString()
        {
            return "Changes " + resourcesToChange.name + " Amount by min " + resourceChangeAmountMin.ToString() + " and max " + resourceChangeAmountMax.ToString();
        }
    }
}

