using UnityEngine;

namespace trollschmiede.Generic.Tooltip
{
    [System.Serializable]
    public class Tooltip
    {
        public string tooltipName;
        [TextArea(1, 30)][Tooltip("{valuename} for Values")]
        public string tooltipText;
        public Sprite tooltipImage;
        public string[] triggerWords;
    }
}
