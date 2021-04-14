using trollschmiede.CivIdle.Resources;
using UnityEngine;

namespace trollschmiede.CivIdle.GameEvents
{
    [CreateAssetMenu(fileName = "New Requirement Resource", menuName = "Scriptable Objects/Game Events/Requierment Resource")]
    public class RequiermentResource : Requierment
    {
        [SerializeField] Resource resource = null;
        [SerializeField] int resourceAmount = 0;

        public override bool CheckRequierment()
        {
            if (resource.amount >= resourceAmount)
            {
                return true;
            }
            return false;
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