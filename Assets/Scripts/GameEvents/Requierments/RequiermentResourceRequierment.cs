using UnityEngine;
using trollschmiede.CivIdle.Resources;

namespace trollschmiede.CivIdle.GameEvents
{
    [CreateAssetMenu(fileName = "New Requirement Resource Requierment", menuName = "Scriptable Objects/Requierments/Requierment Resource Requierment")]
    public class RequiermentResourceRequierment : Requierment
    {
        [SerializeField] ResoureRequierment requierment = ResoureRequierment.Start;

        public override bool CheckRequierment()
        {
            if (ResourceManager.instance.CheckRequirement(requierment))
            {
                return true;
            }
            return false;
        }
        public override string GetRequiermentString()
        {
            return "Resource Requierment " + requierment.ToString();
        }
    }
}