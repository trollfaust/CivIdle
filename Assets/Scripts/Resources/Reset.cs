using UnityEngine;
using trollschmiede.CivIdle.UI;
using trollschmiede.CivIdle.GameEventSys;
using trollschmiede.CivIdle.ScienceSys;
using trollschmiede.CivIdle.ResourceSys;
using trollschmiede.CivIdle.BuildingSys;

namespace trollschmiede.CivIdle.Generic
{
    public class Reset
    {
        public static void ResetData()
        {
            GameEventManager.instance.Reset();

            ResourceManager.instance.Reset();

            LogDisplay.instance.Reset();

            GatheringManager.instance.Reset();

            ScienceManager.instance.Reset();

            BuildingManager.instance.Reset();
        }
    }
}
