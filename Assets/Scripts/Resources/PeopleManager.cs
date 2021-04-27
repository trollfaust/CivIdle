using UnityEngine;
using trollschmiede.CivIdle.GameEvents;
using System.Collections.Generic;
using trollschmiede.CivIdle.UI;

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

        void Start()
        {
            if (peopleResource == null)
            {
                Debug.LogWarning("No People Resource");
            }
            timeStamp = Time.time;
            oldPeopleAmount = peopleResource.amount;
        }

        private int oldPeopleAmount;

        void Update()
        {

            if (oldPeopleAmount != peopleResource.amount)
            {
                PeopleAmountCheck();
                oldPeopleAmount = peopleResource.amount;
                onPeopleAmountChange?.Invoke();
            }

            if (timeStamp + peopleNeedsUpdateTime <= Time.time)
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

        public void NewGatheringObj(GatheringObjectDisplay _gatheringObjectDisplay)
        {
            if (gatheringObjects == null)
            {
                gatheringObjects = new List<GatheringObjectDisplay>();
            }
            gatheringObjects.Add(_gatheringObjectDisplay);
        }

        void PeopleAmountCheck()
        {
            if (gatheringObjects == null)
                return;

            gatheringObjects.Sort((x, y) => x.GetPriorityValue().CompareTo(y.GetPriorityValue()));

            while (peopleResource.amountOpen < 0)
            {
                foreach (var item in gatheringObjects)
                {
                    if (item.GetCount() > 0)
                    {
                        item.OnPeopleLost();
                        break;
                    }
                }
            }


            if (peopleResource.amountOpen > 0)
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
