using UnityEngine;
using UnityEngine.UI;
using TMPro;
using trollschmiede.CivIdle.ResourceSys;

namespace trollschmiede.CivIdle.UI
{
    public class TechnologyRequiermentCostDisplay : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI costText = null;
        [SerializeField] Image resourceImage = null;
        [SerializeField] Image backgroundImage = null;
        [SerializeField] Color[] resourceColors = new Color[0];
        [SerializeField] Resource[] resources = new Resource[0];

        public bool Setup(Resource resource, int amount)
        {
            bool check = false;
            costText.text = amount.ToString();
            for (int i = 0; i < resources.Length; i++)
            {
                if (resources[i] == resource)
                {
                    resourceImage.sprite = resources[i].iconSprite;
                    backgroundImage.color = resourceColors[i];
                    check = true;
                }
            }
            if (!check)
            {
                resourceImage.sprite = resource.iconSprite;
                backgroundImage.color = new Color(1, 1, 1, 1);
                check = true;
            }
            return check;
        }
    }
}