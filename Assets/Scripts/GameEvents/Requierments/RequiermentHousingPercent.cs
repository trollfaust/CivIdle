using trollschmiede.CivIdle.BuildingSys;
using trollschmiede.CivIdle.ResourceSys;
using UnityEngine;

namespace trollschmiede.CivIdle.GameEventSys
{
    [CreateAssetMenu(fileName = "New Requirement Housing Percent", menuName = "Scriptable Objects/Requierments/Requierment Housing Percent")]
    public class RequiermentHousingPercent : Requierment
    {
        [Range(0f, 100f)]
        [SerializeField] float percent = 0f;

        public override bool CheckRequierment()
        {
            if ((float)BuildingManager.instance.GetHousingValues() / (float)PeopleManager.instance.GetPeopleResource().amount >= 1f)
            {
                return true;
            }
            return false;
        }

        public override string GetRequiermentString()
        {
            return percent.ToString() + "% of People have a House";
        }
    }
}