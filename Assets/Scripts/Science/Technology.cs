using UnityEngine;
using trollschmiede.CivIdle.GameEvents;
using UnityEngine.UI;

namespace trollschmiede.CivIdle.Science
{
    [CreateAssetMenu(fileName ="New Technology", menuName = "Scriptable Objects/Science/Technology")]
    public class Technology : ScriptableObject
    {
        [SerializeField] new string name = "";
        [SerializeField] Requierment[] showRequierments = new Requierment[0];
        [SerializeField] Requierment[] unlockRequierments = new Requierment[0];
        [SerializeField] Sprite sprite = null;
        public bool isDone = false;

        bool CheckUnlockRequierments()
        {
            bool b = true;
            foreach (var item in unlockRequierments)
            {
                if (!item.CheckRequierment())
                {
                    b = false;
                }
            }
            return b;
        }

        public bool CheckShowRequierments()
        {
            bool b = true;
            foreach (var item in showRequierments)
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
            if (!CheckUnlockRequierments())
            {
                return false;
            }
            isDone = true;
            return true;
        }

        public Requierment[] GetRequierments() => unlockRequierments;

        public string GetName() => name;

        public Sprite GetSprite() => sprite;
    }
}