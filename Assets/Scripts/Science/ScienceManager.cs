using UnityEngine;
using System.Collections.Generic;
using trollschmiede.CivIdle.UI;
using trollschmiede.CivIdle.GameEvents;
using trollschmiede.Generic.Tooltip;
using trollschmiede.CivIdle.Events;

namespace trollschmiede.CivIdle.Science
{
    public class ScienceManager : MonoBehaviour
    {
        #region Singleton
        public static ScienceManager instance;
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

        private string currentTechKey = "CURRENTTECH";
        private string techKey = "TECH";
        [SerializeField] Technology[] allTechnologies = new Technology[0];
        [SerializeField] GameObject techCardPrefab = null;
        [SerializeField] Transform techCardContainer = null;
        [SerializeField] int baseTechCardsToChoose = 0;
        private List<Technology> doneTechnologies;
        private int techCardsToChoose;

        char techSeperator = '$';

        private void Start()
        {
            techCardsToChoose = baseTechCardsToChoose;
            string safeString = PlayerPrefs.GetString(currentTechKey);
            foreach (var item in allTechnologies)
            {
                item.isDone = (PlayerPrefs.GetInt(techKey + item.name + "ISDONE") != 1) ? false : true; 
            }

            if (safeString == string.Empty)
            {
                SetTechCards(techCardsToChoose);
            } else
            {
                string[] cut = safeString.Split(techSeperator);
                for (int i = 0; i < cut.Length; i++)
                {
                    foreach (Technology technology in allTechnologies)
                    {
                        if (technology.name == cut[i])
                        {
                            AddTechCard(technology);
                        }
                    }
                }
            }
        }

        public void SetTechCards(int count)
        {
            string safeString = "";

            List<Technology> availableTechs = new List<Technology>();
            foreach (var item in allTechnologies)
            {
                if (item.CheckShowRequierments() && !item.isDone)
                {
                    availableTechs.Add(item);
                }
            }
            if (availableTechs.Count == 0)
                return;
            HashSet<int> indexes = new HashSet<int>();
            for (int i = 0; i < count; i++)
            {
                int index = Random.Range(0, availableTechs.Count);
                int check = (index != 0) ? index - 1 : availableTechs.Count - 1;
                while (indexes.Contains(index) && index != check)
                {
                    index++;
                    if (index >= availableTechs.Count)
                    {
                        index = 0;
                    }
                }

                indexes.Add(index);
            }
            foreach (var index in indexes)
            {
                AddTechCard(availableTechs[index]);
                safeString = safeString + availableTechs[index].name + techSeperator;
            }
            PlayerPrefs.SetString(currentTechKey, safeString);
        }

        void AddTechCard(Technology technology)
        {
            GameObject newGO = Instantiate(techCardPrefab, techCardContainer, false) as GameObject;
            newGO.GetComponent<TechnologyCardDisplay>().Setup(technology);
        }

        public bool CheckTechnology(Technology tech)
        {
            if (doneTechnologies == null)
            {
                doneTechnologies = new List<Technology>();
            }
            if (doneTechnologies.Contains(tech))
            {
                return true;
            }
            return false;
        }

        public void ChangeTechCardsToChoose(int amount)
        {
            techCardsToChoose += amount;
        }

        public void ResearchTechnology(Technology tech)
        {
            if (tech.Research())
            {
                if (doneTechnologies == null)
                {
                    doneTechnologies = new List<Technology>();
                }
                doneTechnologies.Add(tech);

                foreach (Requierment item in tech.GetRequierments())
                {
                    if (item is RequiermentResource)
                    {
                        RequiermentResource rr = (RequiermentResource)item;
                        rr.GetResource().AmountChange(-rr.GetAmount());
                    }
                }

                foreach (var item in techCardContainer.GetComponentsInChildren<TechnologyCardDisplay>())
                {
                    TooltipManager.Instance.RemoveTooltip(item.GetComponent<TooltipHoverElement>().currentTooltip);
                    Destroy(item.gameObject);
                }

                PlayerPrefs.SetInt(techKey + tech.name + "ISDONE", 1);

                SetTechCards(techCardsToChoose);
            }
        }

        public void Reset()
        {
            foreach (var item in techCardContainer.GetComponentsInChildren<TechnologyCardDisplay>())
            {
                Destroy(item.gameObject);
            }
            foreach (var item in allTechnologies)
            {
                item.isDone = false;
                PlayerPrefs.SetInt(techKey + item.name + "ISDONE", 0);
            }
            SetTechCards(techCardsToChoose);
        }

        public void Reroll()
        {
            foreach (var item in techCardContainer.GetComponentsInChildren<TechnologyCardDisplay>())
            {
                TooltipManager.Instance.RemoveTooltip(item.GetComponent<TooltipHoverElement>().currentTooltip);
                Destroy(item.gameObject);
            }
            SetTechCards(techCardsToChoose);
        }

        public void RegisterToAll(ITechnologyListener _listener)
        {
            foreach (var item in allTechnologies)
            {
                item.RegisterListener(_listener);
            }
        }

        public void UnregisterFromAll(ITechnologyListener _listener)
        {
            foreach (var item in allTechnologies)
            {
                item.UnregisterListener(_listener);
            }
        }
    }
}