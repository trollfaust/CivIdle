using UnityEngine;
using trollschmiede.CivIdle.Resources;

namespace trollschmiede.CivIdle.GameEvents
{
    [System.Serializable]
    public class Requierment
    {
        [SerializeField] Resource[] resources = new Resource[0];
        [SerializeField] int[] resourcesAmounts = new int[0];
        [SerializeField] ResoureRequierment resoureRequierment = ResoureRequierment.Start;

        public bool CheckRequierment()
        {
            bool b = true;
            if (resources.Length == resourcesAmounts.Length)
            {
                if (resources.Length > 0)
                {
                    for (int i = 0; i < resources.Length; i++)
                    {
                        if (resources[i].amount < resourcesAmounts[i])
                        {
                            b = false;
                        }
                    }
                }
            } else
            {
                b = false;
                Debug.LogWarning("Error: Requirement " + this + " - Resources and ResourcesAmounts out of Sync");
            }

            if (!ResourceManager.instance.CheckRequirement(resoureRequierment))
            {
                b = false;
            }

            return b;
        }

    }
}