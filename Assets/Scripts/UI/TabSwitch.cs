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
                if (item != this)
                {
                    item.OnOtherTabButton();
                }
            }

            CanvasGroup group = tab.GetComponent<CanvasGroup>();
            group.alpha = 1;
            group.interactable = true;
            group.blocksRaycasts = true;
            buttonImage.color = onColor;
        }

        public void OnOtherTabButton()
        {
            CanvasGroup group = tab.GetComponent<CanvasGroup>();
            group.alpha = 0;
            group.interactable = false;
            group.blocksRaycasts = false;
            buttonImage.color = offColor;
        }
    }
}