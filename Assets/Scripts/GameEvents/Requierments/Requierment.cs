using UnityEngine;
using trollschmiede.CivIdle.ResourceSys;

namespace trollschmiede.CivIdle.GameEventSys
{
    public class Requierment : ScriptableObject
    {
        public virtual bool CheckRequierment()
        {
            return false;
        }
        public virtual string GetRequiermentString()
        {
            return "";
        }
    }
}