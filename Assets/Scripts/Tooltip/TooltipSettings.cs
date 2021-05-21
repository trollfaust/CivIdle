using UnityEngine;

namespace trollschmiede.Generic.Tooltip
{
    [CreateAssetMenu(fileName ="New Tooltip Settings",menuName = "Settings/Tooltip Settings")]
    public class TooltipSettings : ScriptableObject
    {
        public bool useHoverTimerToLock;
        public float hoverTimeToLock;
        public bool useKeyToLock;
        public float updateTextTime;
        public int characterWrapLimit;
        public float hoverTimeToShow;
        public Color highlightColor;
        public float fadeTime;
    }
}
