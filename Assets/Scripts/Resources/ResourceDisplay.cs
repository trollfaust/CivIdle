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
        public void Evoke(Resource _resource)
        {
            string text = _resource.name + ": ";
            if (_resource.hasAmountOpen)
                text = text + _resource.amountOpen.ToString() + "/" + _resource.amount.ToString();
            else if (_resource.maxAmount > 0)
                text = text + _resource.amount.ToString() + "/" + _resource.maxAmount.ToString();
            else
                text = text + _resource.amount.ToString();

            resourceText.text = text;
        }

        public void SetResource(Resource _resource)
        {
            this.resource = _resource;
            Evoke(_resource);
            iconImage.sprite = _resource.iconSprite;
            hoverElement.TooltipInitialize(_resource.name);
            StartCoroutine(AddListener(_resource));
        }

        IEnumerator AddListener(Resource _resource)
        {
            yield return new WaitForEndOfFrame();
            _resource.RegisterListener(this);
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