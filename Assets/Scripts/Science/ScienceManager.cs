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
        private List<Technology> doneTechnologies;

        private void Start()
        {
            foreach (var item in allTechnologies)
            {
                GameObject newGo = Instantiate(techCardPrefab, techCardContainer, false) as GameObject;
                newGo.GetComponent<TechnologyCardDisplay>().Setup(item);
            }
        }


        public bool CheckTechnology(Technology tech)
        {
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
            }
        }
    }
}