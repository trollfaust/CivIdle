using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using trollschmiede.CivIdle.Events;
using trollschmiede.CivIdle.Resources;
using trollschmiede.CivIdle.GameEvents;
using TMPro;
using UnityEngine.UI;

namespace trollschmiede.CivIdle.UI
{

    public class LogDisplay : MonoBehaviour, IGameEventListener
    {
        #region Singleton
        public static LogDisplay instance;
        void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }
            instance = this;
        }
        #endregion

        [SerializeField] GameObject logDisplayItemPrefab = null;
        [SerializeField] Transform logDisplayContentTransform = null;
        [SerializeField] GameEventManager gameEventManager = null;
        [SerializeField] Scrollbar scrollBar = null;
        [SerializeField] int maxLogs = 10;

        private List<GameObject> logDisplayItems;

        void Start()
        {
            gameEventManager.RegisterToAllGameEvents(this);
            logDisplayItems = new List<GameObject>();
        }

        void OnDisable()
        {
            gameEventManager.UnregisterFromAllGameEvents(this);
        }

        public void Evoke(GameEvent gameEvent)
        {
            string text = gameEvent.GetGameEventText();

            if (logDisplayItems.Count >= maxLogs)
            {
                GameObject item = logDisplayItems[0];
                logDisplayItems.Remove(item);
                logDisplayItems.Add(item);
                item.transform.SetAsFirstSibling();
            } else
            {
                GameObject newItem = Instantiate(logDisplayItemPrefab, logDisplayContentTransform, false) as GameObject;
                logDisplayItems.Add(newItem);
                newItem.GetComponentInChildren<TextMeshProUGUI>().text = text;
                newItem.transform.SetAsFirstSibling();
            }
            scrollBar.value = 1f;
        }

        public void Evoke() { }

    }
}