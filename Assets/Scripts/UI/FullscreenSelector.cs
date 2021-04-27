using UnityEngine;
using TMPro;

namespace trollschmiede.CivIdle.UI
{
    public class FullscreenSelector : MonoBehaviour
    {
        public void OnDropdownChange(TMP_Dropdown dropdown)
        {
            switch (dropdown.value)
            {
                case 0:
                    Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                    break;
                case 1:
                    Screen.fullScreenMode = FullScreenMode.Windowed;
                    break;
                case 2:
                    Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                    break;
                default:
                    break;
            }
        }
    }
}