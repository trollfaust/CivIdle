using UnityEngine;
using trollschmiede.CivIdle.ScienceSys;

namespace trollschmiede.CivIdle.GameEventSys
{
    [CreateAssetMenu(fileName = "New Requirement Technology", menuName = "Scriptable Objects/Requierments/Requierment Technology")]
    public class RequiermentTechnology : Requierment
    {
        [SerializeField] Technology technology = null;
        [SerializeField] bool isNotUnlocked = false;

        public override bool CheckRequierment()
        {
            if (ScienceManager.instance.CheckTechnology(technology) && !isNotUnlocked)
            {
                return true;
            }
            else if (!ScienceManager.instance.CheckTechnology(technology) && isNotUnlocked)
            {
                return true;
            }
            return false;
        }
        public override string GetRequiermentString()
        {
            return technology.name;
        }
    }
}