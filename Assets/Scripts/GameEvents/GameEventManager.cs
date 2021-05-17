using UnityEngine;
using trollschmiede.CivIdle.EventSys;
using trollschmiede.CivIdle.UI;
using trollschmiede.CivIdle.Util;

namespace trollschmiede.CivIdle.GameEventSys
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
        [SerializeField] GameObject popupEventPrefab = null;
        [SerializeField] Transform popupGameEventContainer = null;

        #region Setup
        bool isSetup = false;
        public bool Setup()
        {
            Reset();

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

            TriggerGameEvent(_gameEvent);
        }

        public void TriggerGameEvent(GameEvent _gameEvent)
        {
            bool check = _gameEvent.Evoke();
            if (_gameEvent is PopupGameEvent && check)
            {
                GameObject go = Instantiate(popupEventPrefab, popupGameEventContainer) as GameObject;
                go.GetComponent<PopupGameEventDisplay>().Setup((PopupGameEvent)_gameEvent);
                Pause.TogglePause(true);
            }

            if (_gameEvent.isOnHold == false && _gameEvent.isDone == false)
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

        public void RegisterToAllGameEvents(IGameEventListener _eventListener)
        {
            foreach (var item in gameEvents)
            {
                item.RegisterListener(_eventListener);
            }
        }

        public void UnregisterFromAllGameEvents(IGameEventListener _eventListener)
        {
            foreach (var item in gameEvents)
            {
                item.UnregisterListener(_eventListener);
            }
        }
    }
}
