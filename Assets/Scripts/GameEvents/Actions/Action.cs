using UnityEngine;

namespace trollschmiede.CivIdle.GameEvents
{
    public class Action : ScriptableObject
    {
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
