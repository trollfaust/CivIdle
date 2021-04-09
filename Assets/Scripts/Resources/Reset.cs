using UnityEngine;
using trollschmiede.CivIdle.UI;
using trollschmiede.CivIdle.GameEvents;

namespace trollschmiede.CivIdle.Resources
{
    public class Reset
    {
        public static void ResetData()
        {
            foreach (var resource in ResourceManager.instance.allResources)
            {
                resource.amount = 0;
                resource.amountOpen = 0;
                if (resource.resoureRequierment != ResoureRequierment.Start)
                {
                    resource.isEnabled = false;
                }
                else
                {
                    resource.EvokeAll();
                }
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
        }
    }
}
