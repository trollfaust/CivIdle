using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using trollschmiede.CivIdle.Resources;
using trollschmiede.CivIdle.Events;
using trollschmiede.Generic.Tooltip;
using UnityEngine.UI;

namespace trollschmiede.CivIdle.UI {
    public class ResourceDisplay : MonoBehaviour, IEventListener
    {
        public TextMeshProUGUI resourceText;
        [SerializeField] TooltipHoverElement hoverElement;
        [SerializeField] Image iconImage;

        private Resource resource;

        public void Evoke() => resourceText.text = resource.name + ": " + resource.amount.ToString();
        public void Evoke(Resource resource) => resourceText.text = resource.name + ": " + resource.amount.ToString();

        public void SetResource(Resource resource)
        {
            this.resource = resource;
            resourceText.text = resource.name + ": " + resource.amount.ToString();
            iconImage.sprite = resource.iconSprite;
            hoverElement.SetTooltipName(resource.name);
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