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

        public override int EvokeAction()
        {
            int i = Random.Range(resourceChangeAmountMin, resourceChangeAmountMax + 1);
            resourcesToChange.MaxAmountChange(i);
            return i;
        }
    }
}

