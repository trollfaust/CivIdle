using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace trollschmiede.CivIdle.UI
{
    public class TabSwitch : MonoBehaviour
    {
        public GameObject tab;
        public Image buttonImage;
        public Color offColor;
        public Color onColor;

        public void OnTabButton()
        {
            foreach (var item in GameObject.FindObjectsOfType<TabSwitch>())
            {
                item.OnOtherTabButton();
            }

            tab.SetActive(true);
            buttonImage.color = onColor;
        }

        public void OnOtherTabButton()
        {
            tab.SetActive(false);
            buttonImage.color = offColor;
        }
    }
}