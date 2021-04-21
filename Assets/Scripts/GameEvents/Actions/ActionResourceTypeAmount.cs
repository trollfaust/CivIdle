using UnityEngine;
using trollschmiede.CivIdle.Resources;
using trollschmiede.CivIdle.UI;
using System.Collections.Generic;

namespace trollschmiede.CivIdle.GameEvents
{
    [CreateAssetMenu(fileName = "New Action Resource Type Amount", menuName = "Scriptable Objects/Actions/Action Resource Type Amount")]
    public class ActionResourceTypeAmount : Action
    {
        [SerializeField] ResourceCategory resourceCategory = ResourceCategory.Other;
        [SerializeField] int amountMin = 0;
        [SerializeField] int amountMax = 0;

        private List<Resource> resourceOfType;
        private float savedValue;

        public override int EvokeAction()
        {
            float i = 0;
            resourceOfType = new List<Resource>();
            foreach (Resource resource in ResourceManager.instance.allResources)
            {
                if (resource.resourceCategory == resourceCategory)
                {
                    resourceOfType.Add(resource);
                    i = i + resource.amount * resource.saturationValue;
                }
            }

            float amount = (float)Random.Range(amountMin, amountMax + 1);
            float count = Mathf.Abs(amount);

            if (i < Mathf.Abs(amount))
            {
                return 0;
            }

            count += savedValue;
            int safeCount = 0;
            while (count > 0 && safeCount < 20)
            {
                safeCount++;
                int index = Random.Range(0, resourceOfType.Count);
                if(resourceOfType[index].amount > 0)
                {
                    resourceOfType[index].AmountChange(Mathf.RoundToInt(Mathf.Sign(amount) * 1));
                    count = count - resourceOfType[index].saturationValue;
                    if (count < 0)
                    {
                        savedValue = count;
                    }
                }
            }

            if (safeCount >= 20)
            {
                while (count > 0)
                {
                    foreach (Resource resource in resourceOfType)
                    {
                        if (resource.amount > 0)
                        {
                            resource.AmountChange(Mathf.RoundToInt(Mathf.Sign(amount) * 1));
                            count = count - resource.saturationValue;
                            if (count < 0)
                            {
                                savedValue = count;
                                break;
                            }
                        }
                    }
                }
            }

            return Mathf.RoundToInt(amount);
        }
    }
}

