using UnityEngine;

namespace trollschmiede.Generic.Tooltip
{
    [CreateAssetMenu(fileName ="New Tooltip Settings",menuName = "Tooltips/Settings")]
    public class TooltipSettings : ScriptableObject
    {
        public bool useHoverTimerToLock;
        public float hoverTime;
        public bool useKeyToLock;
        public KeyCode lockKey;
        public KeyCode unlockKey;
    }
}
