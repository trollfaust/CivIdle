using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

namespace trollschmiede.Generic.Tooltip
{
    public class TooltipHoverElement : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("Setup")]
        [SerializeField] RectTransform tooltipParent = null;
        [Tooltip("Tooltip Text Element for nested Tooltips with Word Hover - can be null")]
        [SerializeField] TextMeshProUGUI text = null;

        private string tooltipName;
        private bool tooltipFixed = false;
        private bool hasTooltipOn = false;
        public GameObject currentTooltip { private set; get; }
        private Coroutine coroutine;
        [HideInInspector]
        public bool isOverUI = false;

        private float timeStamp;

        private Tooltip selfTooltip;
        public void SetSelfTooltip(Tooltip _tooltip)
        {
            selfTooltip = _tooltip;
        }

        void Update()
        {
            if (isOverUI && !hasTooltipOn && timeStamp + TooltipManager.Instance.settings.hoverTimeToShow <= Time.time)
            {
                if (text == null)
                {
                    SetTooltip(TooltipManager.Instance.GetTooltipDataByString(tooltipName));
                    TooltipManager.Instance.SetActiveBaseHoverElement(this);
                }
                if (text != null) // If you have Hovertext and no Tooltip, set Tooltip if available
                {
                    var wordIndex = TMP_TextUtilities.FindIntersectingWord(text, Input.mousePosition, null);
                    if (wordIndex != -1)
                    {
                        string LastHoveredWord = text.textInfo.wordInfo[wordIndex].GetWord();

                        bool selfCheck = false;
                        foreach (var item in selfTooltip.triggerWords)
                        {
                            if (item == LastHoveredWord)
                            {
                                selfCheck = true;
                                break;
                            }
                        }
                        if (selfTooltip.tooltipName == LastHoveredWord)
                            selfCheck = true;


                        Tooltip tooltip = TooltipManager.Instance.GetTooltipDataByString(LastHoveredWord);
                        if (tooltip != null && !selfCheck)
                        {
                            if (currentTooltip != null && tooltip != currentTooltip.GetComponent<TooltipDisplay>().tooltip)
                            {
                                RemoveTooltipSelf();
                            }
                            SetTooltip(tooltip);
                        }
                    }
                }
            }

            if (currentTooltip != null) // If Tooltip on, check if it needs to be locked
            {
                if (Input.GetKeyDown(TooltipManager.Instance.settings.lockKey) && isOverUI)
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
                    RemoveTooltipSelf();
                }
            }
        }

        void RemoveTooltipSelf()
        {
            TooltipManager.Instance.RemoveTooltip(currentTooltip);
            currentTooltip = null;
            hasTooltipOn = false;
        }

        IEnumerator StartTooltipLocking(float _timeToWait) //Coroutine for Locking a Tooltip
        {
            yield return new WaitForSeconds(_timeToWait);

            if (isOverUI)
            {
                tooltipFixed = true;
                currentTooltip.GetComponent<TooltipDisplay>().isFixed = true;
            }
        }

        public void OnPointerEnter(PointerEventData _eventData)
        {
            isOverUI = true;
            timeStamp = Time.time;
        }

        public void OnPointerExit(PointerEventData _eventData)
        {
            isOverUI = false;
        }

        /// <summary>
        /// Remove the Tooltip Gameobject from this Hover Element for Cleanup
        /// </summary>
        /// <returns></returns>
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
        /// <param name="_tooltipName"></param>
        public void TooltipInitialize(string _tooltipName)
        {
            this.tooltipName = _tooltipName;
        }

        void SetTooltip(Tooltip _tooltip)
        {
            if (_tooltip == null || currentTooltip != null)
            {
                return;
            }
            
            hasTooltipOn = true;

            currentTooltip = TooltipManager.Instance.GetNewTooltip();
            currentTooltip.transform.SetParent(tooltipParent, false);
            TooltipDisplay tooltipDisplay = currentTooltip.GetComponent<TooltipDisplay>();
            tooltipDisplay.parentHoverElement = this;
            tooltipDisplay.SetTooltip(_tooltip);
            tooltipDisplay.AlignPosition();

            Canvas.ForceUpdateCanvases(); // Update Canvas and VerticalLayoutGroup for correct Tooltip size
            tooltipDisplay.GetComponent<VerticalLayoutGroup>().enabled = false;
            tooltipDisplay.GetComponent<VerticalLayoutGroup>().enabled = true;

            if (TooltipManager.Instance.settings.useHoverTimerToLock)
            {
                coroutine = StartCoroutine(StartTooltipLocking(TooltipManager.Instance.settings.hoverTimeToLock));
            }
        }
    }
}
