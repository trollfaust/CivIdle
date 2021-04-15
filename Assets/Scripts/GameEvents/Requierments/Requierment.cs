using UnityEngine;
using trollschmiede.CivIdle.Resources;

namespace trollschmiede.CivIdle.GameEvents
{
    public class Requierment : ScriptableObject
    {
        public virtual bool CheckRequierment()
        {
            return false;
        }
    }
}