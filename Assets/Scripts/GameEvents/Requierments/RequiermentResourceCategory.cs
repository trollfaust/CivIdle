using UnityEngine;
using trollschmiede.CivIdle.Resources;

namespace trollschmiede.CivIdle.GameEvents
{
    [CreateAssetMenu(fileName = "New Requirement Resource Category", menuName = "Scriptable Objects/Requierments/Requierment Resource Category")]
    public class RequiermentResourceCategory : Requierment
    {
        [SerializeField] ResourceCategory resourceCategory = ResourceCategory.Other;
        [SerializeField] int resourceAmount = 0;

        public override bool CheckRequierment()
        {
            int amount = 0;
            foreach (var resource in ResourceManager.instance.allResources)
            {
                if (resource.resourceCategory == resourceCategory)
                {
                    amount += resource.amount;
                }
            }
            if (amount >= resourceAmount)
            {
                return true;
            }
            return false;
        }
        public override string GetRequiermentString()
        {
            return resourceAmount.ToString() + " in " + resourceCategory.ToString();
        }
    } 
}