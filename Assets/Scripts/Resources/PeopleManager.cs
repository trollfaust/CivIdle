using UnityEngine;
using trollschmiede.CivIdle.GameEvents;
using System.Collections.Generic;
using trollschmiede.CivIdle.UI;
using trollschmiede.CivIdle.Generic;

namespace trollschmiede.CivIdle.Resources
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
        [SerializeField] int peopleNeedsUpdateTime = 5;
        [SerializeField] Action[] peopleNeedsEachPerson = new Action[0];
        [SerializeField] Action[] peopleNeedsAllPersons = new Action[0];
        [SerializeField] GameEvent[] failGameEvent = null;

        public delegate void OnPeopleAmountChange();
        public event OnPeopleAmountChange onPeopleAmountChange;

        private float timeStamp;
        private List<GatheringObjectDisplay> gatheringObjects;
        private int oldPeopleAmount;

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
            isSetup = true;
            return isSetup;
        }
        #endregion

        #region Update Tick
        public void Tick()
        {
            if (isSetup == false)
                return;

            if (timeStamp + peopleNeedsUpdateTime < Time.time)
            {
                timeStamp = Time.time;
                List<int> output = new List<int>();
                for (int i = 0; i < peopleResource.amount; i++)
                {
                    for (int j = 0; j < peopleNeedsEachPerson.Length; j++)
                    {
                        Action item = peopleNeedsEachPerson[j];
                        int value = item.EvokeAction();
                        if (value == 0)
                        {
                            failGameEvent[j]?.Evoke();
                            break;
                        }
                    }
                }

                for (int j = 0; j < peopleNeedsAllPersons.Length; j++)
                {
                    Action item = peopleNeedsAllPersons[j];
                    int value = item.EvokeAction();
                    if (value == 0)
                    {
                        failGameEvent[j + peopleNeedsEachPerson.Length]?.Evoke();
                        break;
                    }
                }
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

            // Substract People from Gathering Objects if necesary
            gatheringObjects.Sort((x, y) => x.GetPriorityValue().CompareTo(y.GetPriorityValue()));
            while (peopleResource.openAmount < 0)
            {
                if (gatheringObjects == null || gatheringObjects.Count == 0)
                    break;
                foreach (var item in gatheringObjects)
                {
                    if (item.GetCount() > 0)
                    {
                        item.OnPeopleLost();
                        break;
                    }
                }
            }

            // Add People to Gathering Objects if necesary
            if (peopleResource.openAmount > 0)
            {
                gatheringObjects.Sort((x, y) => y.GetPriorityValue().CompareTo(x.GetPriorityValue()));
                foreach (var item in gatheringObjects)
                {
                    if (item.GetWishCount() > item.GetCount())
                    {
                        item.OnPeopleGained();
                    }
                }
            }
        }
    }
}
