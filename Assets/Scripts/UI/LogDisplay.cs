using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using trollschmiede.CivIdle.Events;
using trollschmiede.CivIdle.GameEvents;
using TMPro;
using UnityEngine.UI;
using trollschmiede.CivIdle.Science;

namespace trollschmiede.CivIdle.UI
{

    public class LogDisplay : MonoBehaviour, IGameEventListener, ITechnologyListener
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
        [SerializeField] Scrollbar scrollBar = null;
        [SerializeField] int maxLogs = 10;

        private List<GameObject> logDisplayItems;

        void Start()
        {
            GameEventManager.instance.RegisterToAllGameEvents(this);
            ScienceManager.instance.RegisterToAll(this);
            logDisplayItems = new List<GameObject>();
        }

        void OnDestroy()
        {
            GameEventManager.instance.UnregisterFromAllGameEvents(this);
            ScienceManager.instance.UnregisterFromAll(this);

        }

        public void Evoke(GameEvent gameEvent)
        {
            string text = gameEvent.GetGameEventText();
            if (text == string.Empty)
                return;

            SetDisplayItem(text);
        }

        public void Evoke() { }

        public void Reset()
        {
            foreach (var item in logDisplayContentTransform.GetComponentsInChildren<LogDisplayItem>())
            {
                Destroy(item.gameObject);
            }
            logDisplayItems = new List<GameObject>();
        }

        public void Evoke(Technology technology)
        {
            string text = "Unlocked " + technology.name;
            if (technology.name == string.Empty)
                return;

            SetDisplayItem(text);
        }

        void SetDisplayItem(string text)
        {
            if (logDisplayItems.Count >= maxLogs)
            {
                GameObject item = logDisplayItems[0];
                logDisplayItems.Remove(item);
                logDisplayItems.Add(item);
                item.GetComponentInChildren<TextMeshProUGUI>().text = text;
                item.transform.SetAsFirstSibling();
                logDisplayContentTransform.GetComponent<VerticalLayoutGroup>().enabled = false;
                logDisplayContentTransform.GetComponent<VerticalLayoutGroup>().enabled = true;
                Canvas.ForceUpdateCanvases();
            }
            else
            {
                GameObject newItem = Instantiate(logDisplayItemPrefab, logDisplayContentTransform, false) as GameObject;
                logDisplayItems.Add(newItem);
                newItem.GetComponentInChildren<TextMeshProUGUI>().text = text;
                newItem.transform.SetAsFirstSibling();
            }
            scrollBar.value = 1f;
        }
    }
}