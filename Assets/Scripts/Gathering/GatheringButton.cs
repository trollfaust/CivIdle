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
        [SerializeField] float cooldownTime = 5f;
        [SerializeField] ResourceChancePair[] resourcesPairs = new ResourceChancePair[0];
        [SerializeField] bool isManuelGatherable = true;

        private bool isOnCooldown = false;
        private float startTimestamp;
        private float timePassed;
        private GatheringObject gatheringObject;

        public void OnButtonPressed()
        {
            isOnCooldown = true;
            startTimestamp = Time.time;
            timePassed = 0f;
            button.interactable = false;
            gatheringObject.Gathering();
        }

        private void Start()
        {
            if (!isManuelGatherable)
            {
                slider.value = 0;
                button.interactable = false;
            }
        }

        private void Update()
        {
            if (!isManuelGatherable)
                return;
            if (isOnCooldown)
            {
                timePassed = timePassed + Time.deltaTime;

                float v = (timePassed / cooldownTime);
                slider.value = (v > 1f) ? 1f : v;

                if (Time.time > startTimestamp + cooldownTime)
                {
                    isOnCooldown = false;
                    button.interactable = true;
                }
            }
        }

        public void SetGatheringObject(GatheringObject gathering)
        {
            gatheringObject = gathering;
        }

    }
}
