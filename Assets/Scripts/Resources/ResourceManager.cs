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

        public void Evoke() { }
        public void Evoke(Resource resource) => ActivateResource(resource);

        private List<ResoureRequierment> requiermentsMeet;
        private float timeStamp;

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
            requiermentsMeet.Add(ResoureRequierment.Flint_Tool);
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

        public void ActivateResource(Resource resource)
        {
            if (!resource.isEnabled)
            {
                resource.isEnabled = true;
                GameObject gO = Instantiate(resourceDisplayPrefab, resourceDisplayParent) as GameObject;
                gO.GetComponent<ResourceDisplay>().SetResource(resource);
            }
        }

        public bool CheckRequirement(ResoureRequierment requierment)
        {
            if (requiermentsMeet.Contains(requierment))
            {
                return true;
            }
            return false;
        }

        public void AddRequierment(ResoureRequierment requierment)
        {
            requiermentsMeet.Add(requierment);
        }
    }
}
