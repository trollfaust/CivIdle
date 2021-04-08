using UnityEngine;
using UnityEngine.UI;
using TMPro;
using trollschmiede.CivIdle.Resources;
using System.Collections;

namespace trollschmiede.CivIdle.UI
{
    public class GatheringButton : MonoBehaviour
    {
        [SerializeField] Slider slider;
        [SerializeField] Button button;
        [SerializeField] TextMeshProUGUI buttonText;
        [SerializeField] float cooldownTime;
        [SerializeField] ResourceChancePair[] resourcesPairs;

        private bool isOnCooldown = false;
        private float startTimestamp;
        private float timePassed;

        public void OnButtonPressed()
        {
            isOnCooldown = true;
            startTimestamp = Time.time;
            timePassed = 0f;
            button.interactable = false;
            Gathering();
        }

        private void Update()
        {
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

        void Gathering()
        {
            foreach (ResourceChancePair pair in resourcesPairs)
            {
                if (ResourceManager.instance.CheckRequirement(pair.resource.resoureRequierment))
                {
                    pair.resource.AmountChange(Random.Range(pair.minValue, pair.maxValue));
                }
            }
        }

    }
}
