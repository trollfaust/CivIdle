using UnityEngine;
using trollschmiede.CivIdle.Resources;
using trollschmiede.CivIdle.UI;
using System.Collections.Generic;
using System.Linq;

namespace trollschmiede.CivIdle.GameEvents
{
    [CreateAssetMenu(fileName = "New Action Resource Type Amount", menuName = "Scriptable Objects/Actions/Action Resource Type Amount")]
    public class ActionResourceTypeAmount : Action
    {
        [SerializeField] ResourceCategory resourceCategory = ResourceCategory.Other;
        [SerializeField] int amountMin = 0;
        [SerializeField] int amountMax = 0;
        [SerializeField] bool isLowTypeAmountMultiplierFirst = false;

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
                    if (amountMax <= 0 && amountMin <= 0 && resource.amount <= 0)
                    {
                        continue;
                    }
                    resourceOfType.Add(resource);
                    i = i + resource.amount * resource.typeAmountMultiplier;
                }
            }

            if (isLowTypeAmountMultiplierFirst)
            {
                resourceOfType.Sort((x, y) => x.typeAmountMultiplier.CompareTo(y.typeAmountMultiplier));
            }
            else
            {
                resourceOfType.Sort((x, y) => y.typeAmountMultiplier.CompareTo(x.typeAmountMultiplier));
            }
            float amount = (float)Random.Range(amountMin, amountMax + 1);
            float count = Mathf.Abs(amount);

            if (i < Mathf.Abs(amount))
            {
                return 0;
            }

            int countSameMultiplier = 0;
            float currentMultiplier = resourceOfType[0].typeAmountMultiplier;
            while (currentMultiplier == resourceOfType[0].typeAmountMultiplier && countSameMultiplier < resourceOfType.Count)
            {
                countSameMultiplier++;
                if (countSameMultiplier < resourceOfType.Count && currentMultiplier != resourceOfType[countSameMultiplier].typeAmountMultiplier)
                {
                    currentMultiplier = resourceOfType[countSameMultiplier].typeAmountMultiplier;
                }
            }

            count += savedValue;
            int safeCount = 0;

            while (count > 0 && safeCount < 20)
            {
                safeCount++;
                
                int index = Random.Range(0, countSameMultiplier);
                if(resourceOfType[index].amount > 0)
                {
                    resourceOfType[index].AmountChange(Mathf.RoundToInt(Mathf.Sign(amount) * 1));
                    count = count - 1 * resourceOfType[index].typeAmountMultiplier;
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
                            count = count - resource.typeAmountMultiplier;
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
        public override string GetActionString()
        {
            return "Change Resource Amounts in Category " + resourceCategory.ToString() + " by min " + amountMin.ToString() + " and max " + amountMax.ToString();
        }
    }
}

