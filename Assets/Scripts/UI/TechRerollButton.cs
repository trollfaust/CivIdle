using UnityEngine;
using trollschmiede.CivIdle.Science;
using trollschmiede.CivIdle.Resources;
using trollschmiede.Generic.Tooltip;

namespace trollschmiede.CivIdle.UI
{
    public class TechRerollButton : MonoBehaviour
    {
        [SerializeField] Resource scienceResource = null;
        [SerializeField] TooltipHoverElement hoverElement = null;

        private void Start()
        {
            hoverElement.TooltipInitialize("Reroll Cards");
        }

        public void OnButtonPressed()
        {
            if (scienceResource.amount < 2)
            {
                return;
            }
            scienceResource.AmountChange(-2);
            ScienceManager.instance.Reroll();
        }
    }
}