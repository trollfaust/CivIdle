using UnityEngine;
using trollschmiede.CivIdle.GameEvents;

namespace trollschmiede.CivIdle.Science
{
    [CreateAssetMenu(fileName ="New Technology", menuName = "Scriptable Objects/Science/Technology")]
    public class Technology : ScriptableObject
    {
        [SerializeField] Requierment[] requierments;
        public bool isDone = false;

        bool CheckRequierments()
        {
            bool b = true;
            foreach (var item in requierments)
            {
                if (!item.CheckRequierment())
                {
                    b = false;
                }
            }
            return b;
        }

        public bool Research()
        {
            if (!CheckRequierments())
            {
                return false;
            }
            isDone = true;
            return true;
        }
    }
}