using UnityEngine;
using TMPro;
using UnityEngine.UI;
using trollschmiede.CivIdle.Science;
using trollschmiede.CivIdle.GameEvents;
using trollschmiede.Generic.Tooltip;

namespace trollschmiede.CivIdle.UI
{
    public class TechnologyCardDisplay : MonoBehaviour
    {
        [SerializeField] GameObject techRequirementDisplayPrefab = null;
        [SerializeField] Transform techRequirementDisplayContainer = null;
        [SerializeField] TextMeshProUGUI nameText = null;
        [SerializeField] Image iconImage = null;
        [SerializeField] TooltipHoverElement hoverElement = null;

        public Technology tech;

        public void Setup(Technology technology)
        {
            tech = technology;
            foreach (Requierment requierment in technology.GetRequierments())
            {
                if (requierment is RequiermentResource)
                {
                    RequiermentResource requiermentResource = (RequiermentResource)requierment;
                    GameObject newGO = Instantiate(techRequirementDisplayPrefab, techRequirementDisplayContainer, false) as GameObject;
                    bool check = newGO.GetComponent<TechnologyRequiermentCostDisplay>().Setup(requiermentResource.GetResource(), requiermentResource.GetAmount());
                    if (!check)
                    {
                        Destroy(newGO);
                    }
                }
            }

            iconImage.sprite = technology.GetSprite();
            nameText.text = technology.GetName();

            //hoverElement.SetTooltipValueElement(null);
            hoverElement.TooltipInitialize(technology.GetName());
        }

        public void OnButtonPressed()
        {
            foreach (var requirement in tech.GetRequierments())
            {
                if (!requirement.CheckRequierment())
                {
                    return;
                }
            }
            ScienceManager.instance.ResearchTechnology(tech);
            Destroy(gameObject);
        }
    }
}