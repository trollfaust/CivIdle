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

        [HideInInspector]
        public List<string> allTriggerWords;
        public Tooltip[] database;
        public string searchString;

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
        }
    }
}
