using UnityEngine;
using trollschmiede.CivIdle.Resources;
using trollschmiede.CivIdle.UI;

namespace trollschmiede.CivIdle.GameEvents
{
    [CreateAssetMenu(fileName = "New Action Resource Type Amount", menuName = "Scriptable Objects/Actions/Action Resource Type Amount")]
    public class ActionResourceTypeAmount : Action
    {
        [SerializeField] ResourceCategory resourceCategory = ResourceCategory.Other;
        [SerializeField] int amountMin = 0;
        [SerializeField] int amountMax = 0;
        [SerializeField] GameEvent faildGameEvent = null;

        public override int EvokeAction()
        {
            if (LogDisplay.instance != null)
            {
                LogDisplay.instance.RegisterForGameEvent(faildGameEvent);
            }

            int amount = Random.Range(amountMin, amountMax + 1);
            int count = amount;

            int i = 0;
            while (count > 0 && i <= amount + 1)
            {
                i++;
                foreach (Resource resource in ResourceManager.instance.allResources)
                {
                    if (count <= 0)
                        break;
                    if (resource.resourceCategory == resourceCategory && resource.amount > 0)
                    {
                        resource.AmountChange(-1);
                        count--;
                    }
                }
            }
            if (i > amount && faildGameEvent != null)
            {
                faildGameEvent.Evoke();
            }
            return amount;
        }
    }
}

