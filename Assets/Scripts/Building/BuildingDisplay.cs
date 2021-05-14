using TMPro;
using trollschmiede.CivIdle.Resources;
using trollschmiede.Generic.Tooltip;
using UnityEngine;
using UnityEngine.UI;

namespace trollschmiede.CivIdle.Building
{
    public class BuildingDisplay : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI CountText = null;
        [SerializeField] Button substructButton = null;
        [SerializeField] Resource resourceLand = null;
        [SerializeField] TooltipHoverElement hoverElement = null;

        private Building building;

        bool isSetup = false;
        //TODO: Return false if failed
        public bool Setup(Building _building)
        {
            building = _building;
            hoverElement.TooltipInitialize(building.name);
            UpdateCountText();

            isSetup = true;
            return isSetup;
        }

        public void Tick()
        {
            if (isSetup == false)
                return;

            CheckBuildingCount();
        }

        void CheckBuildingCount()
        {
            if (building.buildingWishedCount == building.buildingCount)
                return;

            int diff = building.buildingWishedCount - building.buildingCount;

            ChangeBuildingCount(diff);
        }

        void ChangeBuildingCount(int _differance)
        {
            for (int i = 0; i < Mathf.Abs(_differance); i++)
            {
                if ((resourceLand.openAmount < building.landNeeded && _differance > 0) || CheckResources() == false)
                    return;
                building.buildingCount += Mathf.RoundToInt(Mathf.Sign((float)_differance) * 1);
                resourceLand.AmountOpenChange(Mathf.RoundToInt(Mathf.Sign((float)_differance) * building.landNeeded * -1));
                ChangeResources();
                UpdateCountText();
            }
        }

        bool CheckResources()
        {
            foreach (ResourceChancePair pair in building.buildingMaterials)
            {
                if (pair.resource.amount < pair.maxValue)
                {
                    return false;
                }
            }
            return true;
        }

        void ChangeResources()
        {
            foreach (ResourceChancePair pair in building.buildingMaterials)
            {
                int rng = Random.Range(pair.minValue, pair.maxValue + 1);
                pair.resource.AmountChange(rng);
            }
        }

        private void Update()
        {
            if (building == null)
                return;
            if (building.buildingWishedCount <= 0)
            {
                substructButton.interactable = false;
            }
            else
            {
                substructButton.interactable = true;
            }
        }

        public void OnAddButton()
        {
            if (building == null)
                return;

            building.buildingWishedCount++;
            UpdateCountText();
        }

        public void OnSubstractButton()
        {
            if (building == null)
                return;

            building.buildingWishedCount--;
            UpdateCountText();
        }

        void UpdateCountText()
        {
            CountText.text = (building.buildingWishedCount != building.buildingCount) ? building.buildingCount.ToString() + " / " + building.buildingWishedCount.ToString() : building.buildingCount.ToString();
        }
    }
}