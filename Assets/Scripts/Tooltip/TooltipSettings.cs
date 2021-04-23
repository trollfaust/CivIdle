using UnityEngine;

namespace trollschmiede.Generic.Tooltip
{
    [CreateAssetMenu(fileName ="New Tooltip Settings",menuName = "Tooltips/Settings")]
    public class TooltipSettings : ScriptableObject
    {
        public bool useHoverTimerToLock;
        public float hoverTimeToLock;
        public bool useKeyToLock;
        public KeyCode lockKey;
        public KeyCode unlockKey;
        public float updateTextTime;
        public int characterWrapLimit;
        public float hoverTimeToShow;
        public Color highlightColor;
        public float fadeTime;
    }
}
