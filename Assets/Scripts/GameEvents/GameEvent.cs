using UnityEngine;
using trollschmiede.CivIdle.Events;
using System.Collections.Generic;
using System.Collections;

namespace trollschmiede.CivIdle.GameEvents
{
    [CreateAssetMenu(fileName = "New GameEvent", menuName = "Scriptable Objects/Game Events/Game Event")]
    public class GameEvent : ScriptableObject
    {
        [Header("Requirements")]
        public Requierment[] requierments;
        [Tooltip("0 = infinite, 1 = one time Event...")]
        public int repeatCountMax;
        [Range(0,100)]
        public int chanceToPass = 50;
        public float timeBetweenChecks = 1f;
        [Header("Actions")]
        public GameEventAction[] gameEventActions = new GameEventAction[0];
        [Header("User Feedback")]
        public string gameEventText = "";
        public char valueSeperator;

        [HideInInspector]
        public bool isDone;
        private List<IGameEventListener> listeners;
        private bool isOnHold = false;

        private List<int> returnActionValues;

        private int repeatCount = 0;
        public void Reset()
        {
            repeatCount = 0;
            isDone = false;
            isOnHold = false;
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
                cut[i] = value.ToString() + cut[i].Substring(1);
            }

            string back = string.Join("", cut);

            return back;
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

        public bool Evoke()
        {
            //Debug.Log("Gameevent " + name + " is Evoked - isDone:" + isDone.ToString() + " - isOnHold: "+ isOnHold.ToString());
            if (isDone || isOnHold)
                return false;
            if (Random.Range(0,100) > chanceToPass)
                return false;

            foreach (var requierment in requierments)
            {
                if (!requierment.CheckRequierment())
                {
                    return false;
                }
            }

            repeatCount++;
            if (repeatCountMax <= repeatCount && repeatCountMax != 0)
                isDone = true;

            returnActionValues = new List<int>();
            foreach (var action in gameEventActions)
            {
                int i = action.EvokeAction();
                returnActionValues.Add(i);
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
