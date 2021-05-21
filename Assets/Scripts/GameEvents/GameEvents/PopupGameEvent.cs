using System.Collections.Generic;
using UnityEngine;
using trollschmiede.CivIdle.Util;

namespace trollschmiede.CivIdle.GameEventSys
{
    [CreateAssetMenu(fileName = "New Popup GameEvent", menuName = "Scriptable Objects/Game Events/Popup Game Event")]
    public class PopupGameEvent : GameEvent
    {
        [Header("Popup Event Text")]
        public string title = null;
        [TextArea]
        public string description = null;
        public string[] answers = new string[0];

        [Header("Answer Requierment")]
        [Tooltip("can bo null for no Requierment")]
        public Requierment[] answerRequierments = new Requierment[0]; 

        public override bool Evoke()
        {
            if (isDone || isOnHold)
                return false;

            foreach (var requierment in requierments)
            {
                if (!requierment.CheckRequierment())
                {
                    return false;
                }
            }

            int rng = Random.Range(0, 100);
            if (rng > (float)GetCurrentChance())
                return false;

            return true;
        }

        public void EvokeAction(int buttonId)
        {
            returnActionValues = new List<int>();

            if (gameEventActions[buttonId] != null)
            {
                gameEventActions[buttonId].EvokeAction();
                int value = gameEventActions[buttonId].GetLastValue();
                returnActionValues.Add(value);

                repeatCount++;
                if (repeatCountMax <= repeatCount && repeatCountMax != 0)
                    isDone = true;
            }

            if (listeners != null)
            {
                foreach (var listener in listeners)
                {
                    listener.Evoke(this);
                }
            }
            GameTime.TogglePause(false);
        }
    }
}
