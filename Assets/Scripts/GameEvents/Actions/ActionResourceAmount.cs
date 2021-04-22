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
            int i = Random.Range(resourceChangeAmountMin, resourceChangeAmountMax + 1);
            if (resourcesToChange.maxAmount > 0 && resourcesToChange.amount + i > resourcesToChange.maxAmount)
            {
                i = resourcesToChange.maxAmount - resourcesToChange.amount;
            }
            resourcesToChange.AmountChange(i);
            return i;
        }
        public override string GetActionString()
        {
            return "Changes " + resourcesToChange.name + " Amount by min " + resourceChangeAmountMin.ToString() + " and max " + resourceChangeAmountMax.ToString();
        }
    }
}

