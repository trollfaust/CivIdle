using UnityEngine;
using System.Collections.Generic;
using trollschmiede.CivIdle.UI;

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
        [SerializeField] int techCardsToChoose = 0;
        private List<Technology> doneTechnologies;

        private void Start()
        {
            SetTechCards(techCardsToChoose);
        }

        public void SetTechCards(int count)
        {
            List<Technology> availableTechs = new List<Technology>();
            foreach (var item in allTechnologies)
            {
                if (item.CheckShowRequierments())
                {
                    availableTechs.Add(item);
                }
            }
            HashSet<int> indexes = new HashSet<int>();
            for (int i = 0; i < count; i++)
            {
                int index = Random.Range(0, availableTechs.Count);
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

        public void ResearchTechnology(Technology tech)
        {
            if (tech.Research())
            {
                if (doneTechnologies == null)
                {
                    doneTechnologies = new List<Technology>();
                }
                doneTechnologies.Add(tech);

                foreach (var item in techCardContainer.GetComponentsInChildren<TechnologyCardDisplay>())
                {
                    Destroy(item.gameObject);
                }

                SetTechCards(techCardsToChoose);
            }
        }
    }
}