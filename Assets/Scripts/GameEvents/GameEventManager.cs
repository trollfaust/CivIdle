using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using trollschmiede.CivIdle.Events;

namespace trollschmiede.CivIdle.GameEvents
{
    public class GameEventManager : MonoBehaviour
    {
        #region singleton
        public static GameEventManager instance;
        private void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }
            instance = this;
        }
        #endregion

        public GameEvent[] gameEvents;

        private void Start()
        {
            StartCoroutine(GameEventLoop());
        }

        IEnumerator GameEventLoop()
        {
            yield return new WaitForEndOfFrame();
            bool isRunning = true;
            while (isRunning)
            {
                isRunning = false;
                foreach (var gameEvent in gameEvents)
                {
                    if (!gameEvent.isDone)
                    {
                        isRunning = true;
                        if (!gameEvent.isSpecialTriggered)
                        {
                            bool checkGameEvent = gameEvent.Evoke();
                            if (checkGameEvent)
                            {
                                StartCoroutine(gameEvent.WaitTime());
                            }
                        }
                    }
                }
                yield return new WaitForSeconds(1f);
            }
        }

        public void RegisterToAllGameEvents(IGameEventListener eventListener)
        {
            foreach (var item in gameEvents)
            {
                item.RegisterListener(eventListener);
            }
        }

        public void UnregisterFromAllGameEvents(IGameEventListener eventListener)
        {
            foreach (var item in gameEvents)
            {
                item.UnregisterListener(eventListener);
            }
        }
    }
}
