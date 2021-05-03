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

        #region Setup
        bool isSetup = false;
        public bool Setup()
        {
            isSetup = true;
            return isSetup;
        }
        #endregion

        #region Update Tick
        public void Tick()
        {
            if (isSetup == false)
                return;

            foreach (var gameEvent in gameEvents)
            {
                GameEventLoop(gameEvent);
            }
        }
        #endregion

        void GameEventLoop(GameEvent _gameEvent)
        {
            if (_gameEvent.isDone || _gameEvent.isSpecialTriggered)
                return;

            _gameEvent.Evoke();
            if (!_gameEvent.isOnHold)
            {
                StartCoroutine(_gameEvent.WaitTime());
            }
        }

        /// <summary>
        /// Restet All GameEvents
        /// </summary>
        public void Reset()
        {
            foreach (var gameEvent in gameEvents)
            {
                gameEvent.Reset();
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
