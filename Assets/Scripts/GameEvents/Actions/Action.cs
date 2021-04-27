using UnityEngine;

namespace trollschmiede.CivIdle.GameEvents
{
    public class Action : ScriptableObject
    {
        [HideInInspector]
        public int overrideValue = 0;

        public virtual int EvokeAction()
        {
            return 0;
        }
        public virtual string GetActionString()
        {
            return "";
        }
    }
}
