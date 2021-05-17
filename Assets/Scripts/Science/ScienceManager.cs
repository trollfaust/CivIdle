using UnityEngine;
using System.Collections.Generic;
using trollschmiede.CivIdle.UI;
using trollschmiede.CivIdle.GameEventSys;
using trollschmiede.Generic.Tooltip;
using trollschmiede.CivIdle.EventSys;
using trollschmiede.CivIdle.Generic;

namespace trollschmiede.CivIdle.ScienceSys
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

        #region Setup
        bool isSetup = false;
        public bool Setup()
        {
            foreach (Technology technology in allTechnologies)
            {
                technology.Reset();
            }

            techCardsToChoose = baseTechCardsToChoose;
            bool check = SetRandomTechCards(techCardsToChoose);
            if (check == false)
                return check;

            isSetup = true;
            return isSetup;
        }
        #endregion

        #region Update Tick
        public void Tick()
        {
            if (isSetup == false)
                return;
            
        }
        #endregion

        #region Cards Setup
        /// <summary>
        /// Sets [amount] random Technologies active that are available, returns false if no technologies are available
        /// </summary>
        /// <param name="_amount"></param>
        /// <returns></returns>
        public bool SetRandomTechCards(int _amount)
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
                return false;

            availableTechs.ShuffleList();

            for (int i = 0; i < _amount; i++)
            {
                if (availableTechs.Count <= 0)
                    break;
                
                Technology tech = availableTechs[0];
                if (tech != null)
                {
                    AddTechCard(tech);
                    availableTechs.RemoveAt(0);
                }
            }
            return true;
        }

        /// <summary>
        /// Adds the a Technology Card with [technology] to the techCardContainer
        /// </summary>
        /// <param name="technology"></param>
        void AddTechCard(Technology technology)
        {
            GameObject newGO = Instantiate(techCardPrefab, techCardContainer, false) as GameObject;
            newGO.GetComponent<TechnologyCardDisplay>().Setup(technology);
        }

        /// <summary>
        /// Reroll the Tech Cards showen to the Player
        /// </summary>
        public void Reroll()
        {
            DestroyOldAndNewTechCards(techCardsToChoose);
        }
        #endregion

        #region Research
        /// <summary>
        /// Research the Technology [_technology]
        /// </summary>
        /// <param name="_technology"></param>
        public void ResearchTechnology(Technology _technology)
        {
            if (_technology.Research())
            {
                if (doneTechnologies == null)
                {
                    doneTechnologies = new List<Technology>();
                }
                doneTechnologies.Add(_technology);

                // Change Resource Amount if the Requierments are Resource Requierments
                foreach (Requierment item in _technology.GetRequierments())
                {
                    if (item is RequiermentResource)
                    {
                        RequiermentResource rr = (RequiermentResource)item;
                        rr.GetResource().AmountChange(-rr.GetAmount());
                    }
                }

                DestroyOldAndNewTechCards(techCardsToChoose); 
            }
        }
        #endregion

        #region Public Helperfunctions
        /// <summary>
        /// Checks if [_technology] is Done
        /// </summary>
        /// <param name="_technology"></param>
        /// <returns></returns>
        public bool CheckTechnology(Technology _technology)
        {
            if (doneTechnologies == null)
            {
                doneTechnologies = new List<Technology>();
            }
            if (doneTechnologies.Contains(_technology))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Changes the Tech Cards to Choose Amount by [amount]
        /// </summary>
        /// <param name="amount"></param>
        public void ChangeTechCardsToChoose(int amount)
        {
            techCardsToChoose += amount;
        }

        /// <summary>
        /// Returns the Percentage of Done Technologies (0-100)
        /// </summary>
        /// <returns></returns>
        public float GetPercentDone()
        {
            float division = doneTechnologies.Count / allTechnologies.Length;
            float output = Mathf.Lerp(0f, 100f, division);
            return output;
        }
        #endregion

        #region Reset
        /// <summary>
        /// Resets all Technologies
        /// </summary>
        public void Reset()
        {
            foreach (Technology technology in allTechnologies)
            {
                technology.Reset();
            }

            techCardsToChoose = baseTechCardsToChoose;
            DestroyOldAndNewTechCards(techCardsToChoose);
        }
        #endregion

        #region Helperfunctions
        List<Technology> ShuffleTechs(List<Technology> _technologies)
        {
            System.Random rando = new System.Random();
            _technologies.Sort((x, y) => rando.Next());

            return _technologies;
        }

        void DestroyOldAndNewTechCards(int _amount)
        {
            foreach (var item in techCardContainer.GetComponentsInChildren<TechnologyCardDisplay>())
            {
                TooltipManager.instance.DeactivateTooltip(item.GetComponent<TooltipHoverElement>().currentTooltip);
                Destroy(item.gameObject);
            }

            SetRandomTechCards(_amount);
        }
        #endregion

        #region Event System
        /// <summary>
        /// Registers to all Technologies
        /// </summary>
        /// <param name="_listener"></param>
        public void RegisterToAll(ITechnologyListener _listener)
        {
            foreach (var item in allTechnologies)
            {
                item.RegisterListener(_listener);
            }
        }
        /// <summary>
        /// Unregisters from all Technologies
        /// </summary>
        /// <param name="_listener"></param>
        public void UnregisterFromAll(ITechnologyListener _listener)
        {
            foreach (var item in allTechnologies)
            {
                item.UnregisterListener(_listener);
            }
        }
        #endregion
    }
}