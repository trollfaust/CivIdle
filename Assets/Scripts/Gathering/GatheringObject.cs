using UnityEngine;
using trollschmiede.Generic.Tooltip;
using System.Collections.Generic;

namespace trollschmiede.CivIdle.Resources
{
    [CreateAssetMenu(fileName = "New Gathering Object", menuName = "Scriptable Objects/Gathering/Gathering Object")]
    public class GatheringObject : ScriptableObject, ITooltipValueElement
    {
        public string buttonName = "";
        [Tooltip("Values should be Negative for Substraction")]
        public ResourceChancePair[] craftingMaterials = new ResourceChancePair[0];
        public ResourceChancePair[] gatheringMaterials = new ResourceChancePair[0];
        public float timeBetweenAutoGathering = 5f;
        public int peopleNeededToWork = 1;
        public bool isManuelGatherable = true;
        public float buttonCooldownTime = 5f;
        public int priorityValueSubstract = 0;
        public bool isEnabled = false;
        [HideInInspector]
        public int peopleWorking = 0;
        [HideInInspector]
        public int peopleWishedWorking = 0;

        public Dictionary<string, string> GetTooltipValues()
        {
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
            keyValuePairs.Add("name", name);
            keyValuePairs.Add("timeBetweenAutoGathering", timeBetweenAutoGathering.ToString() + " Secounds");
            keyValuePairs.Add("peopleNeededToWork", peopleNeededToWork.ToString() + ((peopleNeededToWork > 1) ? " People" : " Person"));
            keyValuePairs.Add("isManuelGatherable", (isManuelGatherable) ? "You can gather this yourself by using the Button." : "");
            keyValuePairs.Add("buttonCooldownTime", buttonCooldownTime.ToString() + " Secounds");
            string craftingMats = "";
            for (int i = 0; i < craftingMaterials.Length; i++)
            {
                ResourceChancePair item = craftingMaterials[i];
                string itemName = item.resource.name;
                string minAmount = item.minValue.ToString();
                string maxAmount = item.maxValue.ToString();
                string chance = item.chance.ToString();

                craftingMats = craftingMats + minAmount + ((maxAmount == minAmount) ? "" : "-" + maxAmount) + " " + itemName + " at a " + chance + "% Chance" + ((i == craftingMaterials.Length - 1) ? "" : ", ");
            }
            keyValuePairs.Add("craftingMaterials", craftingMats);

            string gatheringMats = "";
            for (int i = 0; i < gatheringMaterials.Length; i++)
            {
                ResourceChancePair item = gatheringMaterials[i];
                string itemName = item.resource.name;
                string minAmount = item.minValue.ToString();
                string maxAmount = item.maxValue.ToString();
                string chance = item.chance.ToString();

                gatheringMats = gatheringMats + minAmount + ((maxAmount == minAmount) ? "": "-" + maxAmount) + " " + itemName + " at a " + chance + "% Chance" + ((i == gatheringMaterials.Length - 1) ? "" : ", ");
            }
            keyValuePairs.Add("gatheringMaterials", gatheringMats);

            return keyValuePairs;
        }

        public void Reset()
        {
            isEnabled = false;
            peopleWorking = 0;
            peopleWishedWorking = 0;
        }
    }
}