using UnityEngine;
using trollschmiede.CivIdle.Resources;

namespace trollschmiede.CivIdle.GameEvents
{
    [CreateAssetMenu(fileName = "New Requirement Resource Requierment", menuName = "Scriptable Objects/Game Events/Requierment Resource Requierment")]
    public class RequiermentResourceRequierment : Requierment
    {
        [SerializeField] ResoureRequierment requierment;

        public override bool CheckRequierment()
        {
            if (ResourceManager.instance.CheckRequirement(requierment))
            {
                return true;
            }
            return false;
        }
    }
}