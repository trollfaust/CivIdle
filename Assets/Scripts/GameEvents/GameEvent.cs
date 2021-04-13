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
        public string gameEventText = "";

        [HideInInspector]
        public bool isDone;
        private List<IGameEventListener> listeners;
        private bool isOnHold = false;

        private int repeatCount = 0;
        public void Reset()
        {
            repeatCount = 0;
            isDone = false;
        }

        public IEnumerator WaitTime()
        {
            isOnHold = true;
            yield return new WaitForSeconds(timeBetweenChecks);
            isOnHold = false;
        }

        public string GetGameEventText()
        {
            return gameEventText;
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

            foreach (var action in gameEventActions)
            {
                action.EvokeAction();
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
