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
        private GameObject currentTooltip;
        private Coroutine coroutine;
        [HideInInspector]
        public bool isOverUI = false;

        private ITooltipValueElement tooltipValueElement = null;

        void Update()
        {
            if (isOverUI && text != null && !hasTooltipOn) // If you have Hovertext and no Tooltip, set Tooltip if available
            {
                var wordIndex = TMP_TextUtilities.FindIntersectingWord(text, Input.mousePosition, null);
                if (wordIndex != -1)
                {
                    string LastHoveredWord = text.textInfo.wordInfo[wordIndex].GetWord();

                    Tooltip tooltip = TooltipManager.Instance.GetTooltipDataByString(LastHoveredWord);
                    if (tooltip != null)
                    {
                        SetTooltip(tooltip);
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
                    TooltipManager.Instance.RemoveTooltip(currentTooltip);
                    currentTooltip = null;
                    hasTooltipOn = false;
                }
            }
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
            if (!hasTooltipOn && text == null) // Setup the Tooltip if you have none and you are not a Tooltip by yourself
            {
                SetTooltip(TooltipManager.Instance.GetTooltipDataByString(tooltipName));
                TooltipManager.Instance.SetActiveBaseHoverElement(this);
            }
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
        /// Sets the Tooltip Value Element
        /// </summary>
        /// <param name="_tooltipValueElement"></param>
        public void SetTooltipValueElement(ITooltipValueElement _tooltipValueElement)
        {
            this.tooltipValueElement = _tooltipValueElement;
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
            tooltipDisplay.AlignPosition();
            if (tooltipValueElement == null)
            {
                tooltipDisplay.SetTooltip(_tooltip);
            } else
            {
                tooltipDisplay.SetTooltip(_tooltip, tooltipValueElement);
            }

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
