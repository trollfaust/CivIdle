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
        BoxCollider2D ownCollider;
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
        [HideInInspector]
        public TooltipHoverElement parentHoverElement;

        private int slot = 0;

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
            ownCollider = gameObject.GetComponent<BoxCollider2D>();
        }

        /// <summary>
        /// Starts the Coroutine to Align the Tooltip
        /// </summary>
        public void AlignPosition()
        {
            StartCoroutine(AlignPositionCo());
        }

        bool inCalc = false;

        IEnumerator AlignPositionCo()
        {
            slot = 0;
            yield return new WaitForEndOfFrame();
            RectTransform rect = this.GetComponent<RectTransform>();
            ownCollider.offset = new Vector2(rect.rect.width / 2, rect.rect.height / 2);
            ownCollider.size = new Vector2(rect.rect.width - 1, rect.rect.height - 1);

            SetPositon(rect);
            
            for (int i = 0; i < 4; i++)
            {
                bool checkOne = false;
                for (int j = 0; j < 4; j++)
                {
                    yield return new WaitForSeconds(0.1f);
                    checkOne = CalcPositonOutOfScreen(rect);
                    inCalc = false;
                    if (checkOne)
                    {
                        break;
                    }
                }
                if (checkOne )
                {
                    break;
                }
            }

            yield return new WaitForEndOfFrame();
            animator.SetBool("FadeIn", true);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!isFixed && !inCalc)
            {
                if (collision.tag == "Tooltip")
                {
                    slot = (slot < 3) ? slot + 1 : 0;
                    inCalc = true;
                    SetPositon(this.GetComponent<RectTransform>());
                }
            }
        }

        void SetPositon(RectTransform rect)
        {
            switch (slot)
            {
                case 0:
                    rect.anchorMin = new Vector2(0, 1);
                    rect.anchorMax = new Vector2(0, 1);
                    rect.anchoredPosition = new Vector2(0, 0);
                    break;
                case 1:
                    rect.anchorMin = new Vector2(0, 0);
                    rect.anchorMax = new Vector2(0, 0);
                    rect.anchoredPosition = new Vector2(0, -rect.rect.height);
                    break;
                case 2:
                    rect.anchorMin = new Vector2(1, 1);
                    rect.anchorMax = new Vector2(1, 1);
                    rect.anchoredPosition = new Vector2(0, -rect.rect.height);
                    break;
                case 3:
                    rect.anchorMin = new Vector2(0, 1);
                    rect.anchorMax = new Vector2(0, 1);
                    rect.anchoredPosition = new Vector2(-rect.rect.width, -rect.rect.height);
                    break;
                default:
                    rect.anchorMin = new Vector2(0, 1);
                    rect.anchorMax = new Vector2(0, 1);
                    rect.anchoredPosition = new Vector2(0, rect.rect.height);
                    break;
            }
        }

        bool CalcPositonOutOfScreen(RectTransform rect)
        {
            if (rect.rect.height + 30 > Screen.height - rect.position.y && slot == 0)
            {
                slot = 1;
                SetPositon(rect);
                return false;
            }
            if (rect.position.y - rect.rect.height <= 30 && slot == 1)
            {
                slot = 2;
                SetPositon(rect);
                return false;
            }
            if (rect.rect.width + 30 > Screen.width - rect.position.x && slot == 2)
            {
                slot = 3;
                SetPositon(rect);
                return false;
            }
            if (rect.position.x - rect.rect.width <= 30 && slot == 3)
            {
                slot = 0;
                SetPositon(rect);
                return false;
            }
            return true;
        }
    }
}
