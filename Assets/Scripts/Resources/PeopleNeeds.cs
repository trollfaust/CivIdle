using trollschmiede.CivIdle.GameEventSys;
using UnityEngine;

namespace trollschmiede.CivIdle.ResourceSys
{
    [System.Serializable]
    public class PeopleNeeds
    {
        public Action need = null;
        public bool isAllPeople = false;
        [Tooltip("Could be null")]
        public GameEvent failedGameEvent = null;
        [Tooltip("Could be null")]
        public GameEvent successGameEvent = null;
        public float timeToCheckInSecounds = 1f;

        float timeStamp = 0f;
        bool isSetup = false;

        public void Setup()
        {
            if (isSetup == true)
                return;
            
            timeStamp = Time.time;
            isSetup = true;
        }

        public void Evoke()
        {
            if (isSetup == false)
                return;

            if (timeStamp + timeToCheckInSecounds > Time.time)
                return;

            timeStamp = Time.time;

            if (isAllPeople)
            {
                EvokeActionNeed();
                return;
            }
            for (int i = 0; i < PeopleManager.instance.GetPeopleResource().amount; i++)
            {
                EvokeActionNeed();
            }
        }

        void EvokeActionNeed()
        {
            if (need?.EvokeAction() == false)
            {              
                failedGameEvent?.Evoke();
                return;
            }
            successGameEvent?.Evoke();
        }
    }
}
