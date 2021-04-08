using UnityEngine;
using TMPro;
using trollschmiede.CivIdle.Events;
using trollschmiede.CivIdle.Resources;
using UnityEngine.UI;
using System.Collections;

namespace trollschmiede.Generic.Tooltip
{
    public class TooltipDisplay : MonoBehaviour, IEventListener
    {
        public TextMeshProUGUI titleText;
        public TextMeshProUGUI tooltipText;
        public Image image;
        public TooltipHoverElement hoverElement;
        public Tooltip tooltip;
        public bool isInUse = false;
        public bool isFixed = false;
        public Color32 highlightColor;
        [SerializeField] Image background;
        private Vector3 oldPos;

        public void SetTooltip(Tooltip tooltip)
        {
            this.tooltip = tooltip;

            titleText.text = tooltip.tooltipName;
            tooltipText.text = tooltip.tooltipText;
            image.sprite = tooltip.tooltipImage;
        }

        void Update()
        {
            if (isFixed)
            {
                background.color = Color.red;
            } else
            {
                background.color = Color.white;
            }
        }

        public void AlignPosition()
        {
            StartCoroutine(AlignPositionCo());
        }

        IEnumerator AlignPositionCo()
        {
            yield return new WaitForSeconds(.01f);
            RectTransform rect = this.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0, 1);
            rect.anchorMax = new Vector2(0, 1);
            rect.anchoredPosition = new Vector2(0, 0);

            if (rect.rect.height > Screen.height - rect.position.y)
            {
                rect.anchorMin = new Vector2(0, 0);
                rect.anchorMax = new Vector2(0, 0);
                rect.anchoredPosition = new Vector2(0, -rect.rect.height);
            }
        }

        #region Eventmanagment
        public void Evoke()
        {
            //TODO: i don't know LUL
        }
        public void Evoke(Resource resource) { }

        public void RegisterEventListener(TooltipHoverElement hoverElement)
        {
            hoverElement.RegisterListener(this);
        }
        #endregion
    }
}
