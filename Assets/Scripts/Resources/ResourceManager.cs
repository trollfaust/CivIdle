using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using trollschmiede.CivIdle.UI;
using trollschmiede.CivIdle.Events;
using trollschmiede.CivIdle.GameEvents;

namespace trollschmiede.CivIdle.Resources
{
    public class ResourceManager : MonoBehaviour, IResourceEventListener
    {
        #region Singleton
        public static ResourceManager instance;

        void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }
            instance = this;
        }
        #endregion

        public Resource[] allResources = null;
        [SerializeField] GameObject resourceDisplayPrefab = null;
        [SerializeField] Transform resourceDisplayParent = null;
        [Header("People Managment")]
        [SerializeField] Resource peopleResource = null;
        [SerializeField] float peopleNeedsUpdateTime = 5f;
        [SerializeField] Action[] peopleNeeds = new Action[0];

        private bool isPeopleAmountRunning = false;
        public void Evoke() { }
        public void Evoke(Resource _resource)
        {
            if (_resource == peopleResource && !isPeopleAmountRunning)
            {
                StartCoroutine(PeopleAmountCheck());
            }
            ActivateResource(_resource);
        }

        private List<ResoureRequierment> requiermentsMeet;
        private float timeStamp;
        private List<GatheringObjectDisplay> gatheringObjects;

        private void Start()
        {
            System.Array.Sort(allResources, delegate (Resource x, Resource y) { return x.resourceCategory.CompareTo(y.resourceCategory); });

            foreach (var item in allResources)
            {
                if (item.isEnabled)
                {
                    GameObject gO = Instantiate(resourceDisplayPrefab, resourceDisplayParent) as GameObject;
                    gO.GetComponent<ResourceDisplay>().SetResource(item);
                }
                item.RegisterListener(this);
            }
            requiermentsMeet = new List<ResoureRequierment>();
            requiermentsMeet.Add(ResoureRequierment.Start);
            timeStamp = Time.time;
        }

        void Update()
        {
            if (timeStamp + peopleNeedsUpdateTime <= Time.time)
            {
                timeStamp = Time.time;
                for (int i = 0; i < peopleResource.amount; i++)
                {
                    foreach (var item in peopleNeeds)
                    {
                        item.EvokeAction();
                    }
                }
            }
        }

        void OnDisable()
        {
            foreach (var item in allResources)
            {
                item.UnregisterListener(this);
            }
        }

        public void ActivateResource(Resource _resource)
        {
            if (!_resource.isEnabled)
            {
                _resource.isEnabled = true;
                GameObject gO = Instantiate(resourceDisplayPrefab, resourceDisplayParent) as GameObject;
                gO.GetComponent<ResourceDisplay>().SetResource(_resource);
            }
        }

        public bool CheckRequirement(ResoureRequierment _requierment)
        {
            if (requiermentsMeet.Contains(_requierment))
            {
                return true;
            }
            return false;
        }

        public void AddRequierment(ResoureRequierment _requierment)
        {
            requiermentsMeet.Add(_requierment);
        }

        public void NewGatheringObj(GatheringObjectDisplay _gatheringObjectDisplay)
        {
            if (gatheringObjects == null)
            {
                gatheringObjects = new List<GatheringObjectDisplay>();
            }
            gatheringObjects.Add(_gatheringObjectDisplay);
        }

        IEnumerator PeopleAmountCheck()
        {
            isPeopleAmountRunning = true;
            while (peopleResource.amount > 0 && gatheringObjects != null)
            {
                while (peopleResource.amountOpen < 0)
                {
                    foreach (var item in gatheringObjects)
                    {
                        if (item.GetCount() > 0)
                        {
                            item.OnSubstructButtonPressed();
                            break;
                        }
                    }
                }

                yield return new WaitForSeconds(1f);
            }
            isPeopleAmountRunning = false;
        }
    }
}
