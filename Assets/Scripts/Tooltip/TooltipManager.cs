using UnityEngine;
using System.Collections.Generic;

namespace trollschmiede.Generic.Tooltip
{
    public class TooltipManager : MonoBehaviour
    {
        #region Singleton
        public static TooltipManager Instance;
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }
        #endregion

        [Header("Setup")][Tooltip("Create a TooltipSetting Object and set it here")]
        public TooltipSettings settings;
        [Tooltip("The Prefab for a Tooltip")]
        [SerializeField] GameObject tooltipPrefab = null;
        [Tooltip("The Transform where all unused Tooltips go")]
        [SerializeField] Transform tooltipStorage = null;

        private List<GameObject> tooltips;
        private List<GameObject> activeTooltips;
        private TooltipHoverElement baseActiveHoverElement;

        private void Start()
        {
            tooltips = new List<GameObject>();
            activeTooltips = new List<GameObject>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(settings.unlockKey)) // Removes all Tooltips if the Unlock Key is pressed and not over any Tooltip
            {
                bool isOverAny = false;
                foreach (var tooltip in activeTooltips)
                {
                    if (tooltip.GetComponent<TooltipDisplay>().hoverElement.isOverUI)
                    {
                        isOverAny = true;
                        break;
                    }
                }
                if (!isOverAny)
                {
                    RemoveAllTooltips();
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

        public void SetActiveBaseHoverElement(TooltipHoverElement _hoverElement)
        {
            baseActiveHoverElement = _hoverElement;
        }

        public void RemoveTooltip(GameObject _tooltip)
        {
            if (_tooltip == null)
            {
                return;
            }
            _tooltip.transform.SetParent(tooltipStorage, false);
            TooltipDisplay tooltipDisplay = _tooltip.GetComponent<TooltipDisplay>();
            tooltipDisplay.SetDisplayOff();
            activeTooltips.Remove(_tooltip);
        }

        public void RemoveAllTooltips() // TODO Base Hoverelement reset (Done?)
        {
            GameObject[] activeTooltipArray = activeTooltips.ToArray();

            for (int i = activeTooltipArray.Length - 1; i >= 0; i--)
            {
                GameObject tooltip = activeTooltipArray[i];
                tooltip.transform.SetParent(tooltipStorage, false);
                TooltipDisplay tooltipDisplay = tooltip.GetComponent<TooltipDisplay>();
                tooltipDisplay.hoverElement.RemoveTooltip();
                tooltipDisplay.isFixed = false;
                tooltipDisplay.isInUse = false;
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
