using UnityEngine;
using TMPro;
using UnityEngine.UI;
using trollschmiede.CivIdle.ResourceSys;
using trollschmiede.Generic.Tooltip;
using trollschmiede.CivIdle.MapSys;

namespace trollschmiede.CivIdle.UI
{
    public class GatheringObjectDisplay : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI CountText = null;
        [SerializeField] Button substructButton = null;
        [SerializeField] Button addButton = null;
        [SerializeField] GatheringButton gatheringButton = null;
        [SerializeField] Resource resourcePeople = null;
        [SerializeField] TooltipHoverElement hoverElement = null;

        private GatheringObject gatheringObject;

        float timeStamp = 0f;
        int mapValue = 0;

        private void Update()
        {
            if (SubstractButtonChange() == false)
                return;
            
            AddButtonChange();
        }

        #region Buttons On/Off
        private bool SubstractButtonChange()
        {
            if (gatheringObject == null)
                return false;
            if (gatheringObject.peopleWishedWorking <= 0)
            {
                substructButton.interactable = false;
            }
            else
            {
                substructButton.interactable = true;
            }
            return true;
        }

        private void AddButtonChange()
        {
            if (gatheringObject.workBuilding == null)
            {
                return;
            }
            if (gatheringObject.workBuilding.buildingCount > gatheringObject.peopleWorking)
            {
                addButton.interactable = true;
            }
            else
            {
                addButton.interactable = false;
            }
        }
        #endregion

        bool isSetup = false;
        //TODO: Return false if failed
        public bool Setup(GatheringObject _gatheringObject)
        {
            gatheringObject = _gatheringObject;
            gatheringButton.SetGatheringObjectDisplay(this, gatheringObject);
            PeopleManager.instance.AddGatheringObject(this);
            hoverElement.TooltipInitialize(gatheringObject.name);
            UpdateCountText();

            timeStamp = Time.time;

            isSetup = true;
            return isSetup;
        }

        public void Tick()
        {
            if (isSetup == false)
                return;

            AutoGatheringTick();

            CheckPeopleWorking();
        }

        void AutoGatheringTick()
        {
            if (timeStamp + gatheringObject.timeBetweenAutoGathering > Time.time)
                return;

            timeStamp = Time.time;

            for (int i = 0; i < gatheringObject.peopleWorking; i++)
            {
                Gathering();
            }
        }

        void CheckPeopleWorking()
        {
            if (gatheringObject.peopleWishedWorking == gatheringObject.peopleWorking)
                return;

            int diff = gatheringObject.peopleWishedWorking - gatheringObject.peopleWorking;

            ChangePeopleWorking(diff);
            
        }

        void ChangePeopleWorking(int _differance)
        {
            for (int i = 0; i < Mathf.Abs(_differance); i++)
            {
                if (resourcePeople.openAmount < gatheringObject.peopleNeededToWork && _differance > 0)
                    return;
                gatheringObject.peopleWorking += Mathf.RoundToInt(Mathf.Sign((float)_differance) * 1);
                resourcePeople.AmountOpenChange(Mathf.RoundToInt(Mathf.Sign((float)_differance) * gatheringObject.peopleNeededToWork * -1));
                UpdateCountText();
            }
        }

        public void OnPeopleGained()
        {
            if (gatheringObject == null)
                return;

            CheckPeopleWorking();
        }

        public void OnAddButtonPressed()
        {
            if (gatheringObject == null)
                return;
            
            gatheringObject.peopleWishedWorking++;
            UpdateCountText();
        }

        public void OnSubstructButtonPressed()
        {
            if (gatheringObject == null)
                return;

            gatheringObject.peopleWishedWorking--;
            UpdateCountText();
        }
        
        public void OnPeopleLost()
        {
            if (gatheringObject == null)
                return;

            ChangePeopleWorking(-1);

            CheckPeopleWorking();
        }

        public int GetPriorityValue()
        {
            return gatheringObject.priorityValueSubstract;
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
                    if (pair.resource.GetMaxAmount() > 0 && pair.resource.amount + value > pair.resource.GetTempMaxAmount())
                    {
                        value = pair.resource.GetTempMaxAmount() - pair.resource.amount;
                    }
                    pair.resource.AmountChange(value);
                }
            }
            if (gatheringObject.mapDiscovery == true)
            {
                int rng = Random.Range(0, 100);
                if (gatheringObject.mapResource.chance < rng)
                    return;
                
                MainMap mainMap = FindObjectOfType<MainMap>();
                mainMap.OnTick(gatheringObject.mapResource.minValue, gatheringObject.mapResource.maxValue);
                mapValue += mainMap.GetLastValue();

                if (mapValue >= 10)
                {
                    int amount = Mathf.RoundToInt(Mathf.Floor(mapValue / 10));
                    gatheringObject.mapResource.resource?.AmountChange(amount);
                    mapValue -= (amount * 10);
                }
            }
        }

        void UpdateCountText()
        {
            CountText.text = (gatheringObject.peopleWishedWorking != gatheringObject.peopleWorking) ? gatheringObject.peopleWorking.ToString() + " / " + gatheringObject.peopleWishedWorking.ToString() : gatheringObject.peopleWorking.ToString();
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
