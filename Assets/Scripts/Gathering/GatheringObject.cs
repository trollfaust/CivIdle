using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using trollschmiede.CivIdle.Resources;

namespace trollschmiede.CivIdle.UI
{
    public class GatheringObject : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI CountText = null;
        [SerializeField] Button addButton = null;
        [SerializeField] Button substructButton = null;
        [SerializeField] GatheringButton gatheringButton = null;
        [SerializeField] ResourceChancePair[] resourcesPairs = null;
        [SerializeField] float timeBetweenAutoGathering = 5f;
        [SerializeField] Resource resourcePeople = null;
        [SerializeField] int peopleNeededToWork = 1;

        private int count = 0;

        private void Start() => gatheringButton.SetGatheringObject(this);

        private void Update()
        {
            if (count <= 0)
            {
                substructButton.interactable = false;
            } else
            {
                substructButton.interactable = true;
            }
            if (resourcePeople.amountOpen < peopleNeededToWork)
            {
                addButton.interactable = false;
            } else
            {
                addButton.interactable = true;
            }
        }

        public void OnAddButtonPressed()
        {
            count++;
            CountText.text = count.ToString();
            resourcePeople.AmountOpenChange(-peopleNeededToWork);
            if (count == 1)
            {
                StartCoroutine(AutoGatheringCo());
            }
        }

        public void OnSubstructButtonPressed()
        {
            count--;
            CountText.text = count.ToString();
            resourcePeople.AmountOpenChange(peopleNeededToWork);
        }
        
        IEnumerator AutoGatheringCo()
        {
            while (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    Gathering();
                }
                yield return new WaitForSeconds(timeBetweenAutoGathering);
            }
        }

        public void Gathering()
        {
            foreach (ResourceChancePair pair in resourcesPairs)
            {
                if (ResourceManager.instance.CheckRequirement(pair.resource.resoureRequierment))
                {
                    pair.resource.AmountChange(Random.Range(pair.minValue, pair.maxValue + 1));
                }
            }
        }
    }
}
