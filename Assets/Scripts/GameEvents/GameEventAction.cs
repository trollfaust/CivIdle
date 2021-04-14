using UnityEngine;
using trollschmiede.CivIdle.Resources;

namespace trollschmiede.CivIdle.GameEvents
{
    [System.Serializable]
    public class GameEventAction
    {
        public Resource resourcesToChange;
        public int resourceChangeAmountMin;
        public int resourceChangeAmountMax;

        public int EvokeAction()
        {
            int i = Random.Range(resourceChangeAmountMin, resourceChangeAmountMax + 1);
            resourcesToChange.AmountChange(i);
            return i;
        }
    }
}
