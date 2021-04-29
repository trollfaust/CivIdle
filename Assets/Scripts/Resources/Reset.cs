using UnityEngine;
using trollschmiede.CivIdle.UI;
using trollschmiede.CivIdle.GameEvents;
using trollschmiede.CivIdle.Science;
using trollschmiede.CivIdle.Resources;

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
        }
    }
}
