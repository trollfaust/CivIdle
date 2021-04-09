using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using trollschmiede.CivIdle.Events;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

namespace trollschmiede.Generic.Tooltip
{
    public class TooltipHoverElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("Setup")]
        [SerializeField] RectTransform tooltipParent = null;
        [SerializeField] TextMeshProUGUI text = null;

        private string tooltipName;
        private bool tooltipFixed = false;
        private bool hasTooltipOn = false;
        private GameObject currentTooltip;
        private Coroutine coroutine;
        private TooltipManager manager;
        [HideInInspector]
        public bool isOverUI = false;

        #region Event Managment
        private List<IEventListener> listeners;
        public void RegisterListener(IEventListener listener)
        {
            if (listeners == null)
            {
                listeners = new List<IEventListener>();
            }
            listeners.Add(listener);
        }

        public void UnregisterListener(IEventListener listener)
        {
            listeners.Remove(listener);
        }

        private void EvokeAll()
        {
            foreach (var listener in listeners)
            {
                listener.Evoke();
            }
        }
        #endregion

        void Start()
        {
            manager = TooltipManager.Instance;
        }

        void Update()
        {
            if (isOverUI && text != null && !hasTooltipOn) // If you have Hovertext and no Tooltip, set Tooltip if available
            {
                var wordIndex = TMP_TextUtilities.FindIntersectingWord(text, Input.mousePosition, null);
                if (wordIndex != -1)
                {
                    string LastHoveredWord = text.textInfo.wordInfo[wordIndex].GetWord();

                    Tooltip tooltip = manager.GetTooltipDataByString(LastHoveredWord);
                    if (tooltip != null)
                    {
                        SetTooltip(tooltip);
                    }
                }
            }

            if (currentTooltip != null) // If Tooltip on, check if it needs to be locked
            {
                if (Input.GetKeyDown(manager.settings.lockKey) && isOverUI /*&& !currentTooltip.GetComponent<TooltipDisplay>().hoverElement.isOverUI*/) // TODO test
                {
                    tooltipFixed = !tooltipFixed;
                    currentTooltip.GetComponent<TooltipDisplay>().isFixed = tooltipFixed;
                }
            }

            if (!tooltipFixed && !isOverUI)
            {
                if (coroutine != null) // Stop Coroutine for Locking by Exit
                {
                    StopCoroutine(coroutine);
                    coroutine = null;
                }
                if (currentTooltip != null) // Remove Tooltip if not needed anymore
                {
                    manager.RemoveTooltip(currentTooltip);
                    currentTooltip = null;
                    hasTooltipOn = false;
                }
            }
        }

        IEnumerator StartTooltipLocking(float timeToWait) //Coroutine for Locking a Tooltip
        {
            yield return new WaitForSeconds(timeToWait);

            if (isOverUI)
            {
                tooltipFixed = true;
                currentTooltip.GetComponent<TooltipDisplay>().isFixed = true;
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            isOverUI = true;
            if (!hasTooltipOn && text == null) // Setup the Tooltip if you have none and you are not a Tooltip by yourself
            {
                SetTooltip(manager.GetTooltipDataByString(tooltipName));
                manager.SetActiveBaseHoverElement(this);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            isOverUI = false;
        }

        public bool RemoveTooltip()
        {
            tooltipFixed = false;
            hasTooltipOn = false;
            currentTooltip = null;
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
                coroutine = null;
            }
            return true;
        }

        /// <summary>
        /// Sets the Tooltip Name for Initialization
        /// </summary>
        /// <param name="tooltipName"></param>
        public void SetTooltipName(string tooltipName)
        {
            this.tooltipName = tooltipName;
        }

        void SetTooltip(Tooltip tooltip)
        {
            if (tooltip == null || currentTooltip != null)
            {
                return;
            }
            
            hasTooltipOn = true;

            currentTooltip = manager.GetNewTooltip();
            currentTooltip.transform.SetParent(tooltipParent, false);
            TooltipDisplay tooltipDisplay = currentTooltip.GetComponent<TooltipDisplay>();
            tooltipDisplay.AlignPosition();
            tooltipDisplay.SetTooltip(tooltip);

            Canvas.ForceUpdateCanvases(); // Update Canvas and VerticalLayoutGroup for correct Tooltip size
            tooltipDisplay.GetComponent<VerticalLayoutGroup>().enabled = false;
            tooltipDisplay.GetComponent<VerticalLayoutGroup>().enabled = true;

            if (manager.settings.useHoverTimerToLock)
            {
                coroutine = StartCoroutine(StartTooltipLocking(manager.settings.hoverTime));
            }
        }
    }
}
