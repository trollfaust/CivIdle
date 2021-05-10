using UnityEngine;

namespace trollschmiede.CivIdle.Util
{
    public static class Pause
    {
        static bool isInPause = false;
        public static void TogglePause()
        {
            if (isInPause)
            {
                Time.timeScale = 1f;
                isInPause = false;
            }
            else
            {
                Time.timeScale = 0f;
                isInPause = true;
            }
        }
        public static void TogglePause(bool setPause)
        {
            if (setPause != isInPause)
                TogglePause();
        }
    }
}
