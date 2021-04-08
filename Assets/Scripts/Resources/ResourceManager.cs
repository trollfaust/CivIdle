using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using trollschmiede.CivIdle.UI;
using trollschmiede.CivIdle.Events;

namespace trollschmiede.CivIdle.Resources {
    public class ResourceManager : MonoBehaviour, IEventListener
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

        [SerializeField] Resource[] allResources;
        [SerializeField] GameObject resourceDisplayPrefab;
        [SerializeField] Transform resourceDisplayParent;

        public void Evoke() { }
        public void Evoke(Resource resource) => ActivateResource(resource);

        private List<ResoureRequierment> requiermentsMeet;

        private void Start()
        {
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
        }

        void OnDisable()
        {
            foreach (var item in allResources)
            {
                item.UnregisterListener(this);
            }
        }

        void ActivateResource(Resource resource)
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
