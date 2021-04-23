using UnityEngine;
using trollschmiede.CivIdle.GameEvents;
using trollschmiede.Generic.Tooltip;
using System.Collections.Generic;

namespace trollschmiede.CivIdle.Science
{
    [CreateAssetMenu(fileName ="New Technology", menuName = "Scriptable Objects/Science/Technology")]
    public class Technology : ScriptableObject, ITooltipValueElement
    {
        [SerializeField] new string name = "";
        [SerializeField] Requierment[] showRequierments = new Requierment[0];
        [SerializeField] Requierment[] unlockRequierments = new Requierment[0];
        [SerializeField] Action[] unlocks = new Action[0];
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
            foreach (Action item in unlocks)
            {
                item.EvokeAction();
            }
            isDone = true;
            return true;
        }

        public Requierment[] GetRequierments() => unlockRequierments;

        public string GetName() => name;

        public Sprite GetSprite() => sprite;

        public Dictionary<string, string> GetTooltipValues()
        {
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();

            keyValuePairs.Add("name", name);

            string unlockRequ = "";
            for (int i = 0; i < unlockRequierments.Length; i++)
            {
                Requierment item = (Requierment)unlockRequierments[i];
                if (item.GetRequiermentString() == string.Empty)
                    continue;
                unlockRequ = unlockRequ + ((i == 0) ? "" : ", ") + item.GetRequiermentString();
            }
            keyValuePairs.Add("unlockRequierments", unlockRequ);

            string showRequ = "";
            for (int i = 0; i < showRequierments.Length; i++)
            {
                Requierment item = (Requierment)showRequierments[i];
                if (item.GetRequiermentString() == string.Empty)
                    continue;
                showRequ = showRequ + ((i == 0) ? "" : ", ") + item.GetRequiermentString();
            }
            keyValuePairs.Add("showRequierments", showRequ);

            string unlocksString = "";
            for (int i = 0; i < unlocks.Length; i++)
            {
                Action item = (Action)unlocks[i];
                if (item.GetActionString() == string.Empty)
                    continue;
                unlocksString = unlocksString + ((i == 0) ? "" : ", ") + item.GetActionString();
            }
            keyValuePairs.Add("unlocks", unlocksString);

            return keyValuePairs;
        }
    }
}