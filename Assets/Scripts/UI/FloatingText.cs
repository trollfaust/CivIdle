using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace trollschmiede.CivIdle.UI
{
    public class FloatingText : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI text = null;
        [SerializeField] float timeOut = 0f;
        [SerializeField] CanvasGroup canvasGroup = null;

        bool isPositive = false;
        float timePassed = 0f;


        public void SetValue(int value)
        {
            text.text = value.ToString();
            if (value > 0)
            {
                isPositive = true;
            }
        }

        private void Update()
        {
            if (timePassed > timeOut / 2)
            {
                canvasGroup.alpha = Mathf.Lerp(1, 0, Time.deltaTime);
            }

            if (isPositive)
            {

            } else
            {

            }
        }
    }
}
