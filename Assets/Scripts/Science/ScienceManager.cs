using UnityEngine;
using System.Collections.Generic;

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

        [SerializeField] Technology[] allTechnologies;
        List<Technology> doneTechnologies;


        public bool CheckTechnology(Technology tech)
        {
            if (doneTechnologies.Contains(tech))
            {
                return true;
            }
            return false;
        }

        void ResearchTechnology(Technology tech)
        {
            if (tech.Research())
            {
                doneTechnologies.Add(tech);
            }
        }
    }
}