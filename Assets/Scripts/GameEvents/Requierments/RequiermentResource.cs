using trollschmiede.CivIdle.ResourceSys;
using UnityEngine;

namespace trollschmiede.CivIdle.GameEventSys
{
    [CreateAssetMenu(fileName = "New Requirement Resource", menuName = "Scriptable Objects/Requierments/Requierment Resource")]
    public class RequiermentResource : Requierment
    {
        [SerializeField] Resource resource = null;
        [SerializeField] int resourceAmount = 0;
        [SerializeField] bool isMaxAmount = false;

        public override bool CheckRequierment()
        {
            if (isMaxAmount)
            {
                if (resource.amount <= resourceAmount)
                {
                    return true;
                }
            }
            else
            {
                if (resource.amount >= resourceAmount)
                {
                    return true;
                }
            }
            return false;
        }

        public override string GetRequiermentString()
        {
            if (isMaxAmount)
            {
                return "max " + resourceAmount.ToString() + " of " + resource.name;
            }
            return resourceAmount.ToString() + " " + resource.name;
        }

        public Resource GetResource()
        {
            return resource;
        }

        public int GetAmount()
        {
            return resourceAmount;
        }
    }
}