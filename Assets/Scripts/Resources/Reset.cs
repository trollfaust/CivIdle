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

            foreach (var resourceDisplay in GameObject.FindObjectsOfType<ResourceDisplay>())
            {
                if (!resourceDisplay.resource.isEnabled)
                {
                    MonoBehaviour.Destroy(resourceDisplay.gameObject);
                }
            }

            foreach (var gameEvent in GameEventManager.instance.gameEvents)
            {
                gameEvent.Reset();
            }

            LogDisplay.instance.Reset();

            GatheringManager.instance.Reset();

            ScienceManager.instance.Reset();
        }
    }
}
