using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

namespace trollschmiede.Generic.Tooltip
{
    public class TooltipDisplay : MonoBehaviour
    {
        [Header("Editor Setup")]
        [SerializeField] TextMeshProUGUI titleText = null;
        [SerializeField] TextMeshProUGUI tooltipText = null;
        [SerializeField] Image tooltipImage = null;
        [SerializeField] LayoutElement layoutElement = null;
        public TooltipHoverElement hoverElement = null;
        [SerializeField] Animator animator = null;
        [Header("Event Setup for Fixed Tooltip")]
        [SerializeField] UnityEvent onTooltipFixed = null;
        [SerializeField] UnityEvent onTooltipUnFixed = null;

        [HideInInspector] public bool isInUse = false;
        [HideInInspector] public bool isFixed = false;

        public Tooltip tooltip { get; private set; }
        private Vector3 oldPos;
        private float timeStamp;
        private bool isInvokedFixed = false;
        private bool isInvokedUnFixed = false;

        /// <summary>
        /// Setup the Tooltip for Display
        /// </summary>
        /// <param name="_tooltip"></param>
        public void SetTooltip(Tooltip _tooltip)
        {
            this.tooltip = _tooltip;

            titleText.text = _tooltip.tooltipName;
            tooltipImage.sprite = _tooltip.tooltipImage;
            tooltipText.text = TextEdit(_tooltip.tooltipText);
            hoverElement.SetSelfTooltip(_tooltip);
            SetLayoutElement();
        }

        public void SetDisplayOff()
        {
            isInUse = false;
            animator.SetBool("FadeIn", false);
        }

        void SetLayoutElement()
        {
            if (layoutElement == null)
                return;
            int titleLenght = titleText.text.Length;
            int contentLenght = tooltipText.text.Length;

            layoutElement.enabled = (titleLenght > TooltipManager.Instance.settings.characterWrapLimit || contentLenght > TooltipManager.Instance.settings.characterWrapLimit) ? true : false;
        }

        string TextEdit(string text)
        {
            if (tooltip.GetTooltipValueElement() != null)
            {
                foreach (var dic in tooltip.GetTooltipValueElement())
                {
                    Dictionary<string, string> dict = dic.GetTooltipValues();
                    foreach (KeyValuePair<string, string> item in dict)
                    {
                        try
                        {
                            text = text.Replace("{" + item.Key + "}", item.Value);
                        }
                        catch (System.Exception)
                        {
                            continue;
                        }
                    }
                }
            }

            char[] delimiterChars = { ' ', ',', '.', ':', '\t' };
            string colorCode = ColorUtility.ToHtmlStringRGB(TooltipManager.Instance.settings.highlightColor);

            string[] words = text.Split(delimiterChars);
            for (int i = 0; i < words.Length; i++)
            {
                string word = words[i];
                bool selfCheck = false;
                foreach (var item in tooltip.triggerWords)
                {
                    if (item == word)
                    {
                        selfCheck = true;
                        break;
                    }
                }
                if (tooltip.tooltipName == word)
                    selfCheck = true;
                if (TooltipDatabase.instance.allTriggerWords.Contains(word) && !selfCheck && word != string.Empty)
                {
                    string newWord = "<color=#" + colorCode + ">" + word + "</color>";
                    text = text.Replace(word, newWord);
                }
            }

            return text;
        }

        void Update()
        {
            if (isFixed && !isInvokedFixed)
            {
                isInvokedFixed = true;
                isInvokedUnFixed = false;
                onTooltipFixed.Invoke();
            } else if (!isFixed && !isInvokedUnFixed)
            {
                isInvokedUnFixed = true;
                isInvokedFixed = false;
                onTooltipUnFixed.Invoke();
            }

            if (!isInUse)
                return;

            if (timeStamp + TooltipManager.Instance.settings.updateTextTime >= Time.time)
                return;

            timeStamp = Time.time;
            string text = tooltip.tooltipText;
            text = TextEdit(text);
            tooltipText.text = text;
        }

        private void Start()
        {
            timeStamp = Time.time;
        }

        /// <summary>
        /// Starts the Coroutine to Align the Tooltip
        /// </summary>
        public void AlignPosition()
        {
            StartCoroutine(AlignPositionCo());
        }

        IEnumerator AlignPositionCo()
        {
            yield return new WaitForSeconds(0.01f);
            RectTransform rect = this.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0, 1);
            rect.anchorMax = new Vector2(0, 1);
            rect.anchoredPosition = new Vector2(0, 0);

            if (rect.rect.height + 30 > Screen.height - rect.position.y)
            {
                rect.anchorMin = new Vector2(rect.anchorMin.x, 0);
                rect.anchorMax = new Vector2(rect.anchorMax.x, 0);
                rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, -rect.rect.height);
            }
            if (rect.rect.width + 30 > Screen.width - rect.position.x)
            {
                rect.anchorMin = new Vector2(1, rect.anchorMin.y);
                rect.anchorMax = new Vector2(1, rect.anchorMax.y);
                rect.anchoredPosition = new Vector2(-rect.rect.width, rect.anchoredPosition.y);
            }
            yield return new WaitForEndOfFrame();
            animator.SetBool("FadeIn", true);
        }
    }
}
