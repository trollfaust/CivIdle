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
            resourcesToChange.AmountChange(i);
            return i;
        }
    }
}

