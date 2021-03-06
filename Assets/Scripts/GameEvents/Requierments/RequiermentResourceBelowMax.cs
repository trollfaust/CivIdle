using trollschmiede.CivIdle.Resources;
using UnityEngine;

namespace trollschmiede.CivIdle.GameEvents
{
    [CreateAssetMenu(fileName = "New Requirement Resource is below Max", menuName = "Scriptable Objects/Requierments/Requierment Resource is below Max")]
    public class RequiermentResourceBelowMax : Requierment
    {
        [SerializeField] Resource resource = null;

        public override bool CheckRequierment()
        {
            if (resource.amount < resource.GetTempMaxAmount())
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