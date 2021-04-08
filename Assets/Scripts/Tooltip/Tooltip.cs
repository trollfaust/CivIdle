using UnityEngine;
using UnityEngine.UI;

namespace trollschmiede.Generic.Tooltip
{
    [System.Serializable]
    public class Tooltip
    {
        public string tooltipName;
        [TextArea(1, 30)]
        public string tooltipText;
        public Sprite tooltipImage;
        public string[] triggerWords;
    }
}
