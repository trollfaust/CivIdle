using UnityEngine;
using UnityEngine.UI;
using TMPro;
using trollschmiede.CivIdle.Resources;
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
            button.interactable = false;
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
                    button.interactable = true;
                }
            }
            if (!isOnCooldown)
            {
                button.interactable = true;
                foreach (var item in gatheringObject.craftingMaterials)
                {
                    if (item.maxValue > item.resource.amount)
                    {
                        button.interactable = false;
                    }
                }
            }
        }

        public void SetGatheringObjectDisplay(GatheringObjectDisplay _gatheringObjectDisplay, GatheringObject _gatheringObject)
        {
            gatheringObjectDisplay = _gatheringObjectDisplay;
            gatheringObject = _gatheringObject;

            if (!gatheringObject.isManuelGatherable)
            {
                slider.value = 0;
                button.interactable = false;
            }
            buttonText.text = gatheringObject.buttonName;
        }

    }
}
