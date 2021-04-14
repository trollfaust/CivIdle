using UnityEngine;
using trollschmiede.CivIdle.Resources;

namespace trollschmiede.CivIdle.GameEvents
{
    [CreateAssetMenu(fileName = "New Requirement Resource Category", menuName = "Scriptable Objects/Game Events/Requierment Resource Category")]
    public class RequiermentResourceCategory : Requierment
    {
        [SerializeField] ResourceCategory resourceCategory;
        [SerializeField] int resourceAmount = 0;

        public override bool CheckRequierment()
        {
            int amount = 0;
            foreach (var resource in ResourceManager.instance.allResources)
            {
                amount += resource.amount;
            }
            if (amount >= resourceAmount)
            {
                return true;
            }
            return false;
        }
    } 
}