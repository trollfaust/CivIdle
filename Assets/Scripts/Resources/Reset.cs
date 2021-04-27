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

            foreach (var resource in ResourceManager.instance.allResources)
            {
                resource.Reset();
            }

            foreach (var gameEvent in GameEventManager.instance.gameEvents)
            {
                gameEvent.Reset();
            }

            ResourceManager.instance.Reset();

            LogDisplay.instance.Reset();

            GatheringManager.instance.Reset();

            ScienceManager.instance.Reset();
        }
    }
}
