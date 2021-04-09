using UnityEngine;
using trollschmiede.CivIdle.Resources;

namespace trollschmiede.CivIdle.GameEvents
{
    [System.Serializable]
    public class GameEventAction
    {
        public Resource[] resourcesToChange;
        public int[] resourceChangeAmountMin;
        public int[] resourceChangeAmountMax;

        public void EvokeAction()
        {
            if (resourcesToChange.Length != resourceChangeAmountMin.Length || resourcesToChange.Length != resourceChangeAmountMax.Length)
            {
                Debug.LogWarning("Error: Action " + this + " - Resources and ResourcesAmounts out of Sync");
                return;
            }
            for (int i = 0; i < resourcesToChange.Length; i++)
            {
                resourcesToChange[i].AmountChange(Random.Range(resourceChangeAmountMin[i], resourceChangeAmountMax[i] + 1));
            }
        }
    }
}
