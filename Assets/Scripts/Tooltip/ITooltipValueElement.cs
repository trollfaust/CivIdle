using System.Collections.Generic;

namespace trollschmiede.Generic.Tooltip
{
    public interface ITooltipValueElement
    {
        Dictionary<string, string> GetTooltipValues();
    }
}
