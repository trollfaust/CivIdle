using UnityEngine;

namespace trollschmiede.CivIdle.Util
{
    public static class GameTime
    {
        static bool isInPause = false;
        static float lastTimeScale = 1f;
        static bool isFastTime = false;

        public static void TogglePause()
        {
            if (isInPause)
            {
                Time.timeScale = lastTimeScale;
                isInPause = false;
            }
            else
            {
                lastTimeScale = Time.timeScale;
                Time.timeScale = 0f;
                isInPause = true;
            }
        }

        public static void TogglePause(bool setPause)
        {
            if (setPause != isInPause)
                TogglePause();
        }

        public static void ToggleFast()
        {
            if (isFastTime)
            {
                Time.timeScale = 1f;
                isFastTime = false;
            }
            else
            {
                Time.timeScale = 2f;
                isFastTime = true;
            }

        }

        public static void ToggleFast(bool setFast)
        {
            if (setFast != isFastTime)
            {
                ToggleFast();
            }
        }
    }
}
