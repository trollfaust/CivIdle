using trollschmiede.CivIdle.Util;
using UnityEngine;

namespace trollschmiede.CivIdle.UI {
    public class TimeControlInput : MonoBehaviour
    {
        public TimeButton[] timeButtons = new TimeButton[0];
        public KeySettings keySettings = null;

        private void Update()
        {
            if (keySettings == null)
            {
                return;
            }

            KeyCode pauseKey;
            keySettings.GetKeys().TryGetValue(KeyCodeNames.pauseKey, out pauseKey);
            if (Input.GetKeyDown(pauseKey))
            {
                foreach (TimeButton button in timeButtons)
                {
                    if (button.buttonType == TimeButtonType.Pause)
                    {
                        button.PauseButton();
                    }
                }
            }

            KeyCode playKey;
            keySettings.GetKeys().TryGetValue(KeyCodeNames.playKey, out playKey);
            if (Input.GetKeyDown(playKey))
            {
                foreach (TimeButton button in timeButtons)
                {
                    if (button.buttonType == TimeButtonType.Play)
                    {
                        button.PlayButton();
                    }
                }
            }

            KeyCode fastKey;
            keySettings.GetKeys().TryGetValue(KeyCodeNames.fastKey, out fastKey);
            if (Input.GetKeyDown(fastKey))
            {
                foreach (TimeButton button in timeButtons)
                {
                    if (button.buttonType == TimeButtonType.Fast)
                    {
                        button.FastButton();
                    }
                }
            }
        }
    }
}
