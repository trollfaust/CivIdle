using UnityEngine;
using System.Collections.Generic;
using trollschmiede.CivIdle.Util;

namespace trollschmiede.Generic.Tooltip
{
    public class TooltipManager : MonoBehaviour
    {
        #region Singleton
        public static TooltipManager instance;
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

        [Header("Setup")]
        [Tooltip("Create a TooltipSetting Object and set it here")]
        public TooltipSettings settings;
        public KeySettings keySettings;
        [Tooltip("The Prefab for a Tooltip")]
        [SerializeField] GameObject tooltipPrefab = null;
        [Tooltip("The Transform where all unused Tooltips go")]
        [SerializeField] Transform tooltipStorage = null;

        private List<GameObject> tooltips;
        private List<GameObject> activeTooltips;
        private TooltipHoverElement baseActiveHoverElement;

        #region Setup
        bool isSetup = false;
        private void Start()
        {
            tooltips = new List<GameObject>();
            activeTooltips = new List<GameObject>();
            isSetup = true;
            return;
        }
        #endregion

        // Needs Update for Input of Player
        private void Update()
        {
            if (isSetup == false)
                return;
            
            if (Input.GetKeyDown(keySettings.unlockKey)) // Removes all Tooltips if the Unlock Key is pressed and not over any Tooltip
            {
                bool isOverAny = false;
                bool hasLockedTooltip = false;
                foreach (var tooltip in activeTooltips)
                {
                    if (tooltip.GetComponent<TooltipDisplay>().hoverElement.isOverUI)
                    {
                        isOverAny = true;
                    }
                    if (tooltip.GetComponent<TooltipDisplay>().hoverElement.tooltipFixed)
                    {
                        hasLockedTooltip = true;
                    }
                }
                if (baseActiveHoverElement != null && baseActiveHoverElement.tooltipFixed)
                {
                    hasLockedTooltip = true;
                }
                
                if (!isOverAny && hasLockedTooltip)
                {
                    DeactivateAllTooltips();
                }
            }
        }

        /// <summary>
        /// Returns a GameObject which is a Tooltip from the Pool or creates a new one
        /// </summary>
        /// <returns></returns>
        public GameObject GetNewTooltip()
        {
            foreach (GameObject tooltip in tooltips)
            {
                TooltipDisplay display = tooltip.GetComponent<TooltipDisplay>();
                if (!display.isInUse)
                {
                    display.isInUse = true;
                    activeTooltips.Add(tooltip);
                    tooltip.GetComponent<TooltipDisplay>().isInUse = true;
                    return tooltip;
                }
            }
            GameObject newTooltip = Instantiate(tooltipPrefab, tooltipStorage) as GameObject;
            tooltips.Add(newTooltip);
            activeTooltips.Add(newTooltip);
            newTooltip.GetComponent<TooltipDisplay>().isInUse = true;
            return newTooltip;
        }

        /// <summary>
        /// Sets the Base HoverElement for RemoveAllTooltips if needed
        /// </summary>
        /// <param name="_hoverElement"></param>
        public void SetActiveBaseHoverElement(TooltipHoverElement _hoverElement)
        {
            baseActiveHoverElement = _hoverElement;
        }

        /// <summary>
        /// Deactivates one specific Tooltip
        /// </summary>
        /// <param name="_tooltip"></param>
        public void DeactivateTooltip(GameObject _tooltip)
        {
            if (_tooltip == null)
                return;
            
            _tooltip.transform.SetParent(tooltipStorage, false);
            TooltipDisplay tooltipDisplay = _tooltip.GetComponent<TooltipDisplay>();
            tooltipDisplay.SetDisplayOff();
            activeTooltips.Remove(_tooltip);
        }

        /// <summary>
        /// Deactivates all active Tooltips
        /// </summary>
        private void DeactivateAllTooltips()
        {
            GameObject[] activeTooltipArray = activeTooltips.ToArray();

            for (int i = activeTooltipArray.Length - 1; i >= 0; i--)
            {
                GameObject tooltip = activeTooltipArray[i];
                DeactivateTooltip(tooltip);
            }

            if (baseActiveHoverElement != null)
            {
                baseActiveHoverElement.RemoveTooltip();
                baseActiveHoverElement = null;
            }

            activeTooltips = new List<GameObject>();
        }

        /// <summary>
        /// Returns the Tooltipdata (Tooltip) for a given String (Name or Triggerword)
        /// </summary>
        /// <param name="_textToTest"></param>
        /// <returns></returns>
        public Tooltip GetTooltipDataByString (string _textToTest)
        {
            foreach (var tooltip in TooltipDatabase.instance.database)
            {
                if (tooltip.tooltipName == _textToTest)
                {
                    return tooltip;
                }

                foreach (var triggerWord in tooltip.triggerWords)
                {
                    if (triggerWord == _textToTest)
                    {
                        return tooltip;
                    }
                }
            }

            return null;
        }
    }
}
