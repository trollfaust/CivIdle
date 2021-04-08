using System.Collections.Generic;
using UnityEngine;

namespace trollschmiede.Generic.Tooltip
{
    public class TooltipDatabase : MonoBehaviour
    {
        public static TooltipDatabase instance;

        private void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }
            instance = this;
        }

        private List<string> allTriggerWords;
        public Color highlightColor;
        public Tooltip[] database;

        private void Start()
        {
            allTriggerWords = new List<string>();
            foreach (Tooltip tooltip in database)
            {
                allTriggerWords.Add(tooltip.tooltipName);
                foreach (string triggerWord in tooltip.triggerWords)
                {
                    allTriggerWords.Add(triggerWord);
                }
            }

            char[] delimiterChars = { ' ', ',', '.', ':', '\t' };
            string colorCode = ColorUtility.ToHtmlStringRGB(highlightColor);
            foreach (Tooltip tooltip in database)
            {
                string[] words = tooltip.tooltipText.Split(delimiterChars);
                for (int i = 0; i < words.Length; i++)
                {
                    string word = words[i];
                    if (allTriggerWords.Contains(word))
                    {
                        string newWord = "<color=#" + colorCode + ">" + word + "</color>";
                        tooltip.tooltipText = tooltip.tooltipText.Replace(word, newWord);
                    }
                }
            }
        }

    }
}
