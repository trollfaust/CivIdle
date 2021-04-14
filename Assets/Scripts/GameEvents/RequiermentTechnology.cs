using UnityEngine;
using trollschmiede.CivIdle.Science;

namespace trollschmiede.CivIdle.GameEvents
{
    [CreateAssetMenu(fileName = "New Requirement Technology", menuName = "Scriptable Objects/Game Events/Requierment Technology")]
    public class RequiermentTechnology : Requierment
    {
        [SerializeField] Technology technology;

        public override bool CheckRequierment()
        {
            if (ScienceManager.instance.CheckTechnology(technology))
            {
                return true;
            }
            return false;
        }
    }
}