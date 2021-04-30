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

        private string gameEventKey = "GAMEEVENT";
        public GameEvent[] gameEvents;

        #region Setup
        bool isSetup = false;
        public bool Setup()
        {
            foreach (var item in gameEvents)
            {
                item.repeatCount = PlayerPrefs.GetInt(gameEventKey + item.name + "COUNT");
                item.chanceMultiplier = PlayerPrefs.GetFloat(gameEventKey + item.name + "MULTIPLIER");
                item.isDone = (PlayerPrefs.GetInt(gameEventKey + item.name + "ISDONE") == 0) ? false : true;
            }


            StartCoroutine(GameEventLoop());
            isSetup = true;
            return isSetup;
        }
        #endregion

        #region Update Tick
        public void Tick()
        {

        }
        #endregion


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
                            gameEvent.Evoke();
                            if (!gameEvent.isOnHold)
                            {
                                StartCoroutine(gameEvent.WaitTime());
                            }
                        }
                    }
                    Debug.Log("loop" + gameEvent.name);
                    PlayerPrefs.SetInt(gameEventKey + gameEvent.name + "COUNT", gameEvent.repeatCount);
                    PlayerPrefs.SetFloat(gameEventKey + gameEvent.name + "MULTIPLIER", gameEvent.chanceMultiplier);
                    PlayerPrefs.SetInt(gameEventKey + gameEvent.name + "ISDONE", (gameEvent.isDone) ? 1 : 0);
                }
                yield return new WaitForSeconds(1f);
            }
        }

        public void Reset()
        {
            foreach (var gameEvent in gameEvents)
            {
                gameEvent.Reset();

                PlayerPrefs.SetInt(gameEventKey + gameEvent.name + "COUNT", gameEvent.repeatCount);
                PlayerPrefs.SetFloat(gameEventKey + gameEvent.name + "MULTIPLIER", gameEvent.chanceMultiplier);
                PlayerPrefs.SetInt(gameEventKey + gameEvent.name + "ISDONE", (gameEvent.isDone) ? 1 : 0);
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
