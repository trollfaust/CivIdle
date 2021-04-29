using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using trollschmiede.CivIdle.Resources;
using trollschmiede.Generic.Tooltip;

namespace trollschmiede.CivIdle.UI
{
    public class GatheringObjectDisplay : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI CountText = null;
        [SerializeField] Button addButton = null;
        [SerializeField] Button substructButton = null;
        [SerializeField] GatheringButton gatheringButton = null;
        [SerializeField] Resource resourcePeople = null;
        [SerializeField] TooltipHoverElement hoverElement = null;

        private GatheringObject gatheringObject;

        Coroutine autoGathering;

        private void Update()
        {
            if (gatheringObject == null)
                return;
            if (gatheringObject.peopleWorking <= 0)
            {
                substructButton.interactable = false;
            } else
            {
                substructButton.interactable = true;
            }
            if (resourcePeople.openAmount < gatheringObject.peopleNeededToWork)
            {
                addButton.interactable = false;
            } else
            {
                addButton.interactable = true;
            }
        }

        public void Setup(GatheringObject _gatheringObject)
        {
            gatheringObject = _gatheringObject;
            gatheringButton.SetGatheringObjectDisplay(this, gatheringObject);
            PeopleManager.instance.NewGatheringObj(this);
            hoverElement.TooltipInitialize(gatheringObject.name);
            CountText.text = (gatheringObject.peopleWishedWorking != gatheringObject.peopleWorking) ? gatheringObject.peopleWorking.ToString() + " / " + gatheringObject.peopleWishedWorking.ToString() : gatheringObject.peopleWorking.ToString();
            if (gatheringObject.peopleWorking >= 1 && autoGathering == null)
            {
                autoGathering = StartCoroutine(AutoGatheringCo());
            }
        }

        public void OnPeopleGained()
        {
            if (gatheringObject == null)
                return;
            if (gatheringObject.peopleWishedWorking == gatheringObject.peopleWorking)
                return;

            int tempCount = gatheringObject.peopleWorking;
            for (int i = 0; i < (gatheringObject.peopleWishedWorking - tempCount); i++)
            {
                if (resourcePeople.openAmount < gatheringObject.peopleNeededToWork)
                    return;
                gatheringObject.peopleWorking++;
                CountText.text = (gatheringObject.peopleWishedWorking != gatheringObject.peopleWorking) ? gatheringObject.peopleWorking.ToString() + " / " + gatheringObject.peopleWishedWorking.ToString() : gatheringObject.peopleWorking.ToString();
                resourcePeople.AmountOpenChange(-gatheringObject.peopleNeededToWork);
                if (gatheringObject.peopleWorking == 1 && autoGathering == null)
                {
                    autoGathering = StartCoroutine(AutoGatheringCo());
                }
            }
        }

        public void OnAddButtonPressed()
        {
            if (gatheringObject == null)
                return;
            if (resourcePeople.openAmount < gatheringObject.peopleNeededToWork)
                return;

            gatheringObject.peopleWorking++;
            gatheringObject.peopleWishedWorking = gatheringObject.peopleWorking;
            CountText.text = gatheringObject.peopleWorking.ToString();
            resourcePeople.AmountOpenChange(-gatheringObject.peopleNeededToWork);
            if (gatheringObject.peopleWorking == 1 && autoGathering == null)
            {
                autoGathering = StartCoroutine(AutoGatheringCo());
            }
        }

        public void OnSubstructButtonPressed()
        {
            if (gatheringObject == null)
                return;
            if (gatheringObject.peopleWorking <= 0)
                return;

            gatheringObject.peopleWorking--;
            gatheringObject.peopleWishedWorking = gatheringObject.peopleWorking;
            CountText.text = gatheringObject.peopleWorking.ToString();
            resourcePeople.AmountOpenChange(gatheringObject.peopleNeededToWork);
        }
        
        public void OnPeopleLost()
        {
            if (gatheringObject == null)
                return;
            if (gatheringObject.peopleWorking <= 0)
                return;

            gatheringObject.peopleWorking--;
            CountText.text = (gatheringObject.peopleWishedWorking != gatheringObject.peopleWorking) ? gatheringObject.peopleWorking.ToString() + " / " + gatheringObject.peopleWishedWorking.ToString() : gatheringObject.peopleWorking.ToString();
            resourcePeople.AmountOpenChange(gatheringObject.peopleNeededToWork);
        }

        public int GetPriorityValue()
        {
            return gatheringObject.priorityValueSubstract;
        }

        IEnumerator AutoGatheringCo()
        {
            while (gatheringObject.peopleWorking > 0)
            {
                for (int i = 0; i < gatheringObject.peopleWorking; i++)
                {
                    Gathering();
                }
                yield return new WaitForSeconds(gatheringObject.timeBetweenAutoGathering);
            }
        }

        public void Gathering()
        {
            if (gatheringObject == null)
                return;
            foreach (ResourceChancePair pair in gatheringObject.craftingMaterials)
            {
                if (pair.resource.amount < Mathf.Abs(pair.maxValue))
                {
                    return;
                }
            }
            foreach (ResourceChancePair pair in gatheringObject.craftingMaterials)
            {
                if (Random.Range(0, 100) > pair.chance)
                {
                    continue;
                }
                int value = Random.Range(pair.minValue, pair.maxValue + 1);
                pair.resource.AmountChange(-value);
            }
            foreach (ResourceChancePair pair in gatheringObject.gatheringMaterials)
            {
                if (Random.Range(0, 100) > pair.chance)
                {
                    continue;
                }
                if (ResourceManager.instance.CheckRequirement(pair.resource.resoureRequierment))
                {
                    int value = Random.Range(pair.minValue, pair.maxValue + 1);
                    if (pair.resource.maxAmount > 0 && pair.resource.amount + value > pair.resource.maxAmount)
                    {
                        value = pair.resource.maxAmount - pair.resource.amount;
                    }
                    pair.resource.AmountChange(value);
                }
            }
        }

        public int GetCount()
        {
            return gatheringObject.peopleWorking;
        }
        public int GetWishCount()
        {
            return gatheringObject.peopleWishedWorking;
        }
    }
}
