using trollschmiede.CivIdle.Util;
using UnityEngine;
using UnityEngine.UI;

namespace trollschmiede.CivIdle.UI
{
    public enum TimeButtonType { Pause, Play, Fast }

    public class TimeButton : MonoBehaviour
    {
        public Image buttonImage;
        public Color offColor;
        public Color onColor;
        public TimeButtonType buttonType;

        public void OnTabButton()
        {
            foreach (var item in GameObject.FindObjectsOfType<TimeButton>())
            {
                if (item != this)
                {
                    item.OnOtherTabButton();
                }
            }

            if (buttonType == TimeButtonType.Pause && Time.timeScale != 0f)
            {
                buttonImage.color = offColor;
                return;
            }

            buttonImage.color = onColor;
        }

        public void OnOtherTabButton()
        {
            if (buttonType == TimeButtonType.Play && Time.timeScale == 1f)
            {
                buttonImage.color = onColor;
                return;
            }
            if (buttonType == TimeButtonType.Fast && Time.timeScale == 2f)
            {
                buttonImage.color = onColor;
                return;
            }

            buttonImage.color = offColor;
        }

        public void PauseButton()
        {
            GameTime.TogglePause();
            OnTabButton();
        }

        public void PlayButton()
        {
            GameTime.TogglePause(false);
            GameTime.ToggleFast(false);
            OnTabButton();
        }

        public void FastButton()
        {
            GameTime.TogglePause(false);
            GameTime.ToggleFast(true);
            OnTabButton();
        }
    }
}