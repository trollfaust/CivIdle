using System.Collections.Generic;
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
        public ScriptableObject[] tooltipValueElementObject;
        public string[] triggerWords;

        public ITooltipValueElement[] GetTooltipValueElement()
        {
            if (tooltipValueElementObject == null)
            {
                return null;
            }
            List<ITooltipValueElement> tooltipValues = new List<ITooltipValueElement>();
            foreach (var item in tooltipValueElementObject)
            {
                if (item is ITooltipValueElement)
                {
                    tooltipValues.Add((ITooltipValueElement)item);
                }
            }
            if (tooltipValues.Count > 0)
            {
                return tooltipValues.ToArray();
            }
            
            return null;
        }
    }
}
