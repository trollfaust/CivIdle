using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using trollschmiede.CivIdle.Resources;
using trollschmiede.CivIdle.Events;
using trollschmiede.Generic.Tooltip;
using UnityEngine.UI;

namespace trollschmiede.CivIdle.UI {
    public class ResourceDisplay : MonoBehaviour, IResourceEventListener
    {
        [SerializeField] TextMeshProUGUI resourceText = null;
        [SerializeField] TooltipHoverElement hoverElement = null;
        [SerializeField] Image iconImage = null;

        public Resource resource { get; private set; }

        public void Evoke() => resourceText.text = resource.name + ": " + resource.amount.ToString();
        public void Evoke(Resource resource)
        {
            resourceText.text = resource.name + ": " + ((resource.hasAmountOpen) ? resource.amountOpen.ToString() + "/" : "") + resource.amount.ToString();
        }

        public void SetResource(Resource resource)
        {
            this.resource = resource;
            resourceText.text = resource.name + ": " + ((resource.hasAmountOpen) ? resource.amountOpen.ToString() + "/" : "") + resource.amount.ToString();
            iconImage.sprite = resource.iconSprite;
            hoverElement.TooltipInitialize(resource.name);
            hoverElement.SetTooltipValueElement(resource);
            StartCoroutine(AddListener(resource));
        }

        IEnumerator AddListener(Resource resource)
        {
            yield return new WaitForEndOfFrame();
            resource.RegisterListener(this);
        }

        void OnEnable()
        {
            if (resource != null)
            {
                SetResource(resource);
            }
        }

        void OnDisable() => resource.UnregisterListener(this);
    }
}