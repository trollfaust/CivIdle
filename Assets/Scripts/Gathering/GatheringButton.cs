using UnityEngine;
using UnityEngine.UI;
using TMPro;
using trollschmiede.CivIdle.ResourceSys;
using System.Collections;

namespace trollschmiede.CivIdle.UI
{
    public class GatheringButton : MonoBehaviour
    {
        [SerializeField] Slider slider = null;
        [SerializeField] Button button = null;
        [SerializeField] TextMeshProUGUI buttonText = null;

        private bool isOnCooldown = false;
        private float startTimestamp;
        private float timePassed;
        private GatheringObjectDisplay gatheringObjectDisplay;
        private GatheringObject gatheringObject;

        public void OnButtonPressed()
        {
            if (gatheringObjectDisplay == null)
                return;
            isOnCooldown = true;
            startTimestamp = Time.time;
            timePassed = 0f;
            SetButtonInteractable(false);
            gatheringObjectDisplay.Gathering();
        }

        private void Update()
        {
            if (gatheringObject == null || gatheringObjectDisplay == null)
                return;
            if (!gatheringObject.isManuelGatherable)
                return;
            if (isOnCooldown)
            {
                timePassed = timePassed + Time.deltaTime;

                float v = (timePassed / gatheringObject.buttonCooldownTime);
                slider.value = (v > 1f) ? 1f : v;

                if (Time.time > startTimestamp + gatheringObject.buttonCooldownTime)
                {
                    isOnCooldown = false;
                    SetButtonInteractable(true);
                }
            }
            if (!isOnCooldown)
            {
                SetButtonInteractable(true);
                foreach (var item in gatheringObject.craftingMaterials)
                {
                    if (item.maxValue > item.resource.amount)
                    {
                        SetButtonInteractable(false);
                    }
                }
            }
        }

        void SetButtonInteractable(bool _interactable)
        {
            button.interactable = _interactable;
            Image buttonImage = button.GetComponent<Image>();
            if (_interactable)
            {
                buttonImage.color = new Color(buttonImage.color.r, buttonImage.color.g, buttonImage.color.b, 0);
            } else
            {
                buttonImage.color = new Color(buttonImage.color.r, buttonImage.color.g, buttonImage.color.b, 1);
            }
        }

        public void SetGatheringObjectDisplay(GatheringObjectDisplay _gatheringObjectDisplay, GatheringObject _gatheringObject)
        {
            gatheringObjectDisplay = _gatheringObjectDisplay;
            gatheringObject = _gatheringObject;

            if (!gatheringObject.isManuelGatherable)
            {
                slider.value = 0;
                SetButtonInteractable(false);
            }
            buttonText.text = gatheringObject.buttonName;
        }

    }
}
