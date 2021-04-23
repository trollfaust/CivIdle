using UnityEngine;
using trollschmiede.CivIdle.GameEvents;
using System.Collections.Generic;

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

        public Resource peopleResource = null;
        [SerializeField] int peopleNeedsUpdateTime = 5;
        [SerializeField] Action[] peopleNeeds = new Action[0];
        [SerializeField] GameEvent failGameEvent = null;

        private float timeStamp;

        void Start()
        {
            if (peopleResource == null)
            {
                Debug.LogWarning("No People Resource");
            }
            timeStamp = Time.time;
        }

        void Update()
        {
            if (timeStamp + peopleNeedsUpdateTime <= Time.time)
            {
                timeStamp = Time.time;
                List<int> output = new List<int>();
                for (int i = 0; i < peopleResource.amount; i++)
                {
                    foreach (var item in peopleNeeds)
                    {
                        output.Add(item.EvokeAction());
                    }
                }
                foreach (int value in output)
                {
                    if (value == 0)
                    {
                        failGameEvent.Evoke();
                        break;
                    }
                }
            }
        }
    }
}
