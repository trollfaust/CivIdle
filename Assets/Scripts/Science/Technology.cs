using UnityEngine;
using trollschmiede.CivIdle.GameEvents;
using UnityEngine.UI;

namespace trollschmiede.CivIdle.Science
{
    [CreateAssetMenu(fileName ="New Technology", menuName = "Scriptable Objects/Science/Technology")]
    public class Technology : ScriptableObject
    {
        [SerializeField] new string name = "";
        [SerializeField] Requierment[] requierments = new Requierment[0];
        [SerializeField] Sprite sprite = null;
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

        public Requierment[] GetRequierments() => requierments;

        public string GetName() => name;

        public Sprite GetSprite() => sprite;
    }
}