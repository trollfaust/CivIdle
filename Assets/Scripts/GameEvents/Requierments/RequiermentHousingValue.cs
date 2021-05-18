using trollschmiede.CivIdle.BuildingSys;
using UnityEngine;

namespace trollschmiede.CivIdle.GameEventSys
{
    [CreateAssetMenu(fileName = "New Requirement Housing Value", menuName = "Scriptable Objects/Requierments/Requierment Housing Value")]
    public class RequiermentHousingValue : Requierment
    {
        [SerializeField] int housingValue = 0;

        public override bool CheckRequierment()
        {
            if (BuildingManager.instance.GetHousingValues() >= housingValue)
            {
                return true;
            }
            return false;
        }

        public override string GetRequiermentString()
        {
            return housingValue.ToString() + " People have a House";
        }
    }
}