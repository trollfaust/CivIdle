using UnityEngine;
using trollschmiede.CivIdle.EventSys;
using System.Collections.Generic;
using System.Collections;

namespace trollschmiede.CivIdle.GameEventSys
{
    [CreateAssetMenu(fileName = "New GameEvent", menuName = "Scriptable Objects/Game Events/Game Event")]
    public class GameEvent : ScriptableObject
    {
        [Header("Requirements")]
        public Requierment[] requierments;
        [Header("Event Setup")]
        [Tooltip("0 = infinite, 1 = one time Event...")]
        public int repeatCountMax;
        [Range(0,100)]
        public int baseChanceToPass = 50;
        public float timeBetweenChecks = 1f;
        public bool isSpecialTriggered = false;
        [Header("Actions")]
        public Action[] gameEventActions = new Action[0];
        [Header("User Feedback")]
        public string gameEventText = "";
        char valueSeperator = '$';
        char cutSeperator = '§';
        public bool emptyIfZero = false;

        [HideInInspector]
        public bool isDone;
        [HideInInspector]
        public List<IGameEventListener> listeners;
        [HideInInspector]
        public bool isOnHold = false;

        [HideInInspector]
        public List<int> returnActionValues;

        [HideInInspector]
        public int repeatCount = 0;
        [HideInInspector]
        public float chanceMultiplier = 1f;

        public void Reset()
        {
            repeatCount = 0;
            isDone = false;
            isOnHold = false;
            chanceMultiplier = 1f;
        }

        public void ResetCount()
        {
            repeatCount = 0;
        }

        public IEnumerator WaitTime()
        {
            isOnHold = true;
            yield return new WaitForSeconds(timeBetweenChecks);
            isOnHold = false;
        }

        public string GetGameEventText()
        {
            string[] cut = gameEventText.Split(valueSeperator);
            for (int i = 1; i < cut.Length; i++)
            {
                string s = cut[i].Substring(0, 1);
                int x = 0;
                int.TryParse(s, out x);

                int value = GetValueById(x);
                value = Mathf.Abs(value);
                if (value == 0)
                {
                    string[] cutNew = cut[i].Split(cutSeperator);
                    if (cutNew.Length > 1)
                    {
                        cut[i] = string.Join("", cutNew, 1, cutNew.Length - 1);
                    }
                }
                else
                {
                    cut[i] = value.ToString() + cut[i].Substring(1);
                    if (cut[i].IndexOf(cutSeperator) >= 0)
                    {
                        cut[i] = cut[i].Remove(cut[i].IndexOf(cutSeperator), 1);
                    }
                }
            }

            string back = string.Join("", cut);
            if (emptyIfZero)
            {
                foreach (var item in returnActionValues)
                {
                    if (item != 0)
                    {
                        return back;
                    }
                }
                return "";
            }

            return back;
        }

        /// <summary>
        /// Modifies the Chance by the multiplier
        /// </summary>
        /// <param name="_multiplier"></param>
        public void SetChanceMultiplier(float _multiplier)
        {
            chanceMultiplier = _multiplier;
        }

        public int GetCurrentChance()
        {
            float f = baseChanceToPass * chanceMultiplier;
            f = Mathf.RoundToInt(f);
            if (f > 100f)
                f = 100f;
            if (f < 0f)
                f = 0f;
            return (int)f;
        }

        private int GetValueById(int i)
        {
            try
            {
                return returnActionValues[i];
            }
            catch (System.Exception)
            {
                return 0;
            }
        }

        public float GetMultiplier()
        {
            return chanceMultiplier;
        }

        public int GetCountDone()
        {
            return repeatCount;
        }

        #region Event Managment
        public void RegisterListener(IGameEventListener _listener)
        {
            if (listeners == null)
            {
                listeners = new List<IGameEventListener>();
            }
            listeners.Add(_listener);
        }

        public void UnregisterListener(IGameEventListener _listener)
        {
            listeners.Remove(_listener);
        }

        public virtual bool Evoke()
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

            repeatCount++;
            if (repeatCountMax <= repeatCount && repeatCountMax != 0)
                isDone = true;

            returnActionValues = new List<int>();
            foreach (var action in gameEventActions)
            {
                action.EvokeAction();
                int value = action.GetLastValue();
                returnActionValues.Add(value);
            }

            if (listeners != null)
            {
                foreach (var listener in listeners)
                {
                    listener.Evoke(this);
                }
            }
            return true;
        }
        #endregion
    }
}
