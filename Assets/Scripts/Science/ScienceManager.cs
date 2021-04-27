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

        [SerializeField] Technology[] allTechnologies = new Technology[0];
        [SerializeField] GameObject techCardPrefab = null;
        [SerializeField] Transform techCardContainer = null;
        [SerializeField] int baseTechCardsToChoose = 0;
        private List<Technology> doneTechnologies;
        private int techCardsToChoose;

        private void Start()
        {
            techCardsToChoose = baseTechCardsToChoose;
            SetTechCards(techCardsToChoose);
        }

        public void SetTechCards(int count)
        {
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
            }
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