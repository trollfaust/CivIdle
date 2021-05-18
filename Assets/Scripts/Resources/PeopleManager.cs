﻿using UnityEngine;
using System.Collections.Generic;
using trollschmiede.CivIdle.UI;
using trollschmiede.CivIdle.Generic;
using trollschmiede.CivIdle.ScienceSys;
using trollschmiede.CivIdle.BuildingSys;

namespace trollschmiede.CivIdle.ResourceSys
{
    public class PeopleManager : MonoBehaviour
    {
        #region Singleton
        public static PeopleManager instance;
        private void Awake()
        {
            if (instance != null)
            {
                return;
            }
            instance = this;
        }
        #endregion

        [Header("Setup")]
        [SerializeField] Resource peopleResource = null;
        [SerializeField] Resource sickPeopleResource = null;
        [SerializeField] PeopleNeeds[] allPeopleNeeds = new PeopleNeeds[0];
        [SerializeField] HappinessNeed[] happinessNeeds = new HappinessNeed[0];

        public delegate void OnPeopleAmountChange();
        public event OnPeopleAmountChange onPeopleAmountChange;

        private float timeStamp;
        private List<GatheringObjectDisplay> gatheringObjects;
        private int oldPeopleAmount;
        private float happiness = 0f;

        #region Setup
        bool isSetup = false;
        public bool Setup()
        {
            if (peopleResource == null)
            {
                GameManager.instance.CheckLogWarning("People Manager without People Resource");
                return false;
            }
            timeStamp = Time.time;
            oldPeopleAmount = peopleResource.amount;

            foreach (PeopleNeeds peopleNeeds in allPeopleNeeds)
            {
                peopleNeeds.Setup();
            }

            isSetup = true;
            return isSetup;
        }
        #endregion

        #region Update Tick
        public void Tick()
        {
            if (isSetup == false)
                return;

            CalcHappiness();

            foreach (PeopleNeeds peopleNeeds in allPeopleNeeds)
            {
                peopleNeeds.Evoke();
            }
        }
        #endregion

        // TODO: Probablly Update needed for Amount change check (maybe on Event Resource change?)
        void Update()
        {
            if (isSetup == false)
                return;
            
            if (oldPeopleAmount != peopleResource.amount)
            {
                PeopleAmountCheck();
                oldPeopleAmount = peopleResource.amount;
                onPeopleAmountChange?.Invoke();
            }
        }

        /// <summary>
        /// Add a Gathering Object to check for People Count
        /// </summary>
        /// <param name="_gatheringObjectDisplay"></param>
        public void AddGatheringObject(GatheringObjectDisplay _gatheringObjectDisplay)
        {
            if (_gatheringObjectDisplay == null)
                return;
            
            if (gatheringObjects == null)
            {
                gatheringObjects = new List<GatheringObjectDisplay>();
            }
            gatheringObjects.Add(_gatheringObjectDisplay);
        }

        /// <summary>
        /// Add or Substract People from Gathering Objects automaticlly
        /// </summary>
        void PeopleAmountCheck()
        {
            if (gatheringObjects == null)
                return;

            int safeCount = 0;
            // Substract People from Gathering Objects if necesary
            gatheringObjects.Sort((x, y) => x.GetPriorityValue().CompareTo(y.GetPriorityValue()));
            while (peopleResource.openAmount < 0 && safeCount < 20)
            {
                safeCount++;
                if (gatheringObjects == null || gatheringObjects.Count == 0)
                    break;
                foreach (GatheringObjectDisplay gatheringObjectDisplay in gatheringObjects)
                {
                    if (gatheringObjectDisplay.GetCount() > 0)
                    {
                        gatheringObjectDisplay.OnPeopleLost();
                        break;
                    }
                }
            }

            // Add People to Gathering Objects if necesary
            if (peopleResource.openAmount > 0)
            {
                gatheringObjects.Sort((x, y) => y.GetPriorityValue().CompareTo(x.GetPriorityValue()));
                foreach (GatheringObjectDisplay gatheringObjectDisplay in gatheringObjects)
                {
                    if (gatheringObjectDisplay.GetWishCount() > gatheringObjectDisplay.GetCount())
                    {
                        gatheringObjectDisplay.OnPeopleGained();
                    }
                }
            }
        }

        float GetNeedsPercent()
        {
            float calc = 1 / happinessNeeds.Length;
            float output = Mathf.Lerp(0f, 100f, calc);
            return output;
        }

        float CalcHappinessNeedsResource()
        {
            if (happinessNeeds == null || happinessNeeds.Length == 0)
                return 0f;

            float calc = 0f;
            foreach (HappinessNeed need in happinessNeeds)
            {
                if (need.isForAll)
                {
                    if (need.resource.amount >= need.amount)
                    {
                        calc += GetNeedsPercent();
                    } else
                    {
                        calc -= GetNeedsPercent();
                    }
                }
                else
                {
                    if (need.resource.amount >= need.amount * peopleResource.amount)
                    {
                        calc += GetNeedsPercent();
                    } else
                    {
                        calc -= GetNeedsPercent();
                    }
                }
            }
            return calc;
        }

        float CalcHousingHappiness()
        {
            float calc = BuildingManager.instance.GetHousingValues() / ((peopleResource.amount == 0) ? 1 : peopleResource.amount);
            if (calc > 1f)
            {
                calc = 1f;
            }
            calc = Mathf.Lerp(0f, 30f, calc);

            return calc;
        }

        public void CalcHappiness()
        {
            float calc = 0f;

            calc -= ScienceManager.instance.GetPercentDone();
            calc += CalcHappinessNeedsResource();
            calc += CalcHousingHappiness();

            if (calc > 100f)
            {
                calc = 100f;
            }
            if (calc < -100f)
            {
                calc = -100f;
            }
            happiness = calc;
        }

        /// <summary>
        /// Returns the People Happiness in Percent (-100 to 100)
        /// </summary>
        /// <returns></returns>
        public float GetHappiness()
        {
            CalcHappiness();
            return happiness;
        }

        public Resource GetPeopleResource()
        {
            return peopleResource;
        }
    }
}
