using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using trollschmiede.CivIdle.Resources;

namespace trollschmiede.CivIdle.UI
{
    public class GatheringObjectDisplay : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI CountText = null;
        [SerializeField] Button addButton = null;
        [SerializeField] Button substructButton = null;
        [SerializeField] GatheringButton gatheringButton = null;
        [SerializeField] Resource resourcePeople = null;

        private GatheringObject gatheringObject;
        private int count = 0;

        private void Update()
        {
            if (gatheringObject == null)
                return;
            if (count <= 0)
            {
                substructButton.interactable = false;
            } else
            {
                substructButton.interactable = true;
            }
            if (resourcePeople.amountOpen < gatheringObject.peopleNeededToWork)
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
            ResourceManager.instance.NewGatheringObj(this);
        }

        public void OnAddButtonPressed()
        {
            if (gatheringObject == null)
                return;
            count++;
            CountText.text = count.ToString();
            resourcePeople.AmountOpenChange(-gatheringObject.peopleNeededToWork);
            if (count == 1)
            {
                StartCoroutine(AutoGatheringCo());
            }
        }

        public void OnSubstructButtonPressed()
        {
            if (gatheringObject == null)
                return;
            count--;
            CountText.text = count.ToString();
            resourcePeople.AmountOpenChange(gatheringObject.peopleNeededToWork);
        }
        
        IEnumerator AutoGatheringCo()
        {
            while (count > 0)
            {
                for (int i = 0; i < count; i++)
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
            foreach (ResourceChancePair pair in gatheringObject.resourcesPairs)
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
            return count;
        }
    }
}
