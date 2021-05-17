using UnityEngine;

namespace trollschmiede.CivIdle.GameEventSys
{
    public class Action : ScriptableObject
    {
        [HideInInspector]
        public int overrideValue = 0;

        public virtual bool EvokeAction()
        {
            return false;
        }
        public virtual string GetActionString()
        {
            return "";
        }
        public virtual int GetLastValue()
        {
            return 0;
        }
    }
}
