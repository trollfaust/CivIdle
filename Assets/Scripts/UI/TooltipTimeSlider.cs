using UnityEngine;
using TMPro;
using UnityEngine.UI;
using trollschmiede.Generic.Tooltip;
using System;

namespace trollschmiede.CivIdle.UI
{
    public class TooltipTimeSlider : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI text = null;
        [SerializeField] Slider slider = null;
        [SerializeField] TooltipSettings tooltipSettings = null;

        private void Start()
        {
            slider.value = tooltipSettings.hoverTimeToShow;
        }

        public void OnSliderChange()
        {
            
            text.text = Math.Round(slider.value, 1).ToString() + " sec";
            tooltipSettings.hoverTimeToShow = slider.value;
        }

    }
}