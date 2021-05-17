using trollschmiede.CivIdle.ResourceSys;
using UnityEngine;

namespace trollschmiede.CivIdle.GameEventSys
{
    [CreateAssetMenu(fileName = "New Requirement Resource is below Max", menuName = "Scriptable Objects/Requierments/Requierment Resource is below Max")]
    public class RequiermentResourceBelowMax : Requierment
    {
        [SerializeField] Resource resource = null;
        [SerializeField] int amountBelow = 1;

        public override bool CheckRequierment()
        {
            if (resource.amount + amountBelow <= resource.GetTempMaxAmount())
            {
                return true;
            }
            return false;
        }

        public override string GetRequiermentString()
        {
            if (CheckRequierment())
            {
                return resource.name + " amount is below it's maximum.";
            }
            return resource.name + " amount is at it's maximum.";
        }

    }
}