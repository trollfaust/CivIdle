using UnityEngine;
using trollschmiede.CivIdle.Science;

namespace trollschmiede.CivIdle.GameEvents
{
    [CreateAssetMenu(fileName = "New Requirement Technology", menuName = "Scriptable Objects/Requierments/Requierment Technology")]
    public class RequiermentTechnology : Requierment
    {
        [SerializeField] Technology technology = null;

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