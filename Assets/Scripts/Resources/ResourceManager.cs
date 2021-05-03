using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using trollschmiede.CivIdle.UI;
using trollschmiede.CivIdle.Events;
using System;

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

        [SerializeField] Resource[] allResources = null;
        [SerializeField] GameObject resourceDisplayGroupPrefab = null;
        [SerializeField] GameObject resourceDisplayPrefab = null;
        [SerializeField] Transform resourceDisplayParent = null;

        private List<ResoureRequierment> requiermentsMeet;
        private List<ResourceDisplayGroup> resourceDisplayGroups;
        private List<Resource> activeResources;

        #region Setup
        bool isSetup = false;
        public bool Setup()
        {
            resourceDisplayGroups = new List<ResourceDisplayGroup>();
            Array.Sort(allResources, delegate (Resource x, Resource y) { return x.resourceCategory.CompareTo(y.resourceCategory); });

            SetupResourceCategorys();

            AddRequierment(ResoureRequierment.Start);

            Reset();

            isSetup = true;
            return isSetup;
        }

        void SetupResourceCategorys()
        {
            foreach (Resource resource in allResources)
            {
                if (CheckDisplayGroupInstantiated(resource.resourceCategory))
                    continue;

                InstantiateResourceCategory(resource);
            }
        }

        bool CheckDisplayGroupInstantiated(ResourceCategory _resourceCategory)
        {
            foreach (ResourceDisplayGroup displayGroup in resourceDisplayGroups)
            {
                if (displayGroup.resourceCategory == _resourceCategory)
                    return true;
            }

            return false;
        }

        void InstantiateResourceCategory(Resource _resource)
        {
            GameObject gO = Instantiate(resourceDisplayGroupPrefab, resourceDisplayParent) as GameObject;
            ResourceDisplayGroup displayGroup = gO.GetComponent<ResourceDisplayGroup>();

            SetupResourceCategoryDetails(_resource, displayGroup);

            resourceDisplayGroups.Add(displayGroup);

            gO.SetActive(false);
        }

        void SetupResourceCategoryDetails(Resource _resource, ResourceDisplayGroup _displayGroup)
        {
            string title = _resource.resourceCategory.ToString();
            title = title.Replace("_", " ");
            _displayGroup.SetTitleText(title);

            _displayGroup.resourceCategory = _resource.resourceCategory;
        }
        #endregion

        #region Update Tick
        public void Tick()
        {
            if (isSetup == false)
                return;

        }
        #endregion

        #region Reset
        /// <summary>
        /// Resets all Resources
        /// </summary>
        public void Reset()
        {
            DestroyResourceDisplays();

            ResetAllResources();

            ResetLayout();
        }

        void DestroyResourceDisplays()
        {
            foreach (var resourceDisplay in GameObject.FindObjectsOfType<ResourceDisplay>())
            {
                Destroy(resourceDisplay.gameObject);
            }            
        }

        void ResetAllResources()
        {
            activeResources = new List<Resource>();

            foreach (Resource resource in allResources)
            {
                resource.Reset();

                if (resource.isEnabled == false)
                    continue;
                
                ActivateResource(resource);
            }
        }

        void ResetLayout()
        {
            Canvas.ForceUpdateCanvases();
            resourceDisplayParent.gameObject.GetComponent<VerticalLayoutGroup>().enabled = false;
            resourceDisplayParent.gameObject.GetComponent<VerticalLayoutGroup>().enabled = true;
        }
        #endregion

        #region Activate Resource
        /// <summary>
        /// Activates a given Resource and sets its Position
        /// </summary>
        /// <param name="_resource"></param>
        public void ActivateResource(Resource _resource)
        {
            if (activeResources == null)
                activeResources = new List<Resource>();

            if (activeResources.Contains(_resource))
                return;

            ResourceDisplayGroup displayGroup = GetResourceGroup(_resource);
            if (displayGroup == null)
                return;

            EnableResourceDisplayGroup(displayGroup);

            _resource.isEnabled = true;

            InstatiateResourceAtGroup(_resource, displayGroup);

            _resource.RegisterListener(this);
            activeResources.Add(_resource);
        }

        ResourceDisplayGroup GetResourceGroup(Resource _resource)
        {
            ResourceDisplayGroup displayGroup = null;
            foreach (var group in resourceDisplayGroups)
            {
                if (group.resourceCategory == _resource.resourceCategory)
                {
                    displayGroup = group;
                }
            }
            return displayGroup;
        }

        void EnableResourceDisplayGroup(ResourceDisplayGroup _displayGroup)
        {
            _displayGroup.gameObject.SetActive(true);
        }

        void InstatiateResourceAtGroup(Resource _resource, ResourceDisplayGroup _displayGroup)
        {
            GameObject gO = Instantiate(resourceDisplayPrefab, _displayGroup.GetContentTransform()) as GameObject;
            gO.GetComponent<ResourceDisplay>().SetResource(_resource);

            ResetLayout();            
        }
        #endregion

        #region Public Helper Functions
        /// <summary>
        /// Check if a given ResourceRequierment is meet
        /// </summary>
        /// <param name="_requierment"></param>
        /// <returns></returns>
        public bool CheckRequirement(ResoureRequierment _requierment)
        {
            if (requiermentsMeet.Contains(_requierment))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Adds a given Requierment to the meet Requierments
        /// </summary>
        /// <param name="_requierment"></param>
        public void AddRequierment(ResoureRequierment _requierment)
        {
            if (requiermentsMeet == null)
                requiermentsMeet = new List<ResoureRequierment>();
            
            requiermentsMeet.Add(_requierment);
        }

        /// <summary>
        /// Returns an Array of all Resources which have a Culture Value greater than 0
        /// </summary>
        /// <returns></returns>
        public Resource[] GetCultureGeneratingResources()
        {
            List<Resource> output = new List<Resource>();
            foreach (Resource resource in allResources)
            {
                if (resource.cultureValue == 0)
                    continue;
                
                output.Add(resource);
            }
            return output.ToArray();
        }

        /// <summary>
        /// Returns all Resources
        /// </summary>
        /// <returns></returns>
        public Resource[] GetAllResources()
        {
            return allResources;
        }
        #endregion

        #region Event System
        public void Evoke() { }
        public void Evoke(Resource _resource)
        {
            ActivateResource(_resource);
        }

        void OnDisable()
        {
            foreach (var item in allResources)
            {
                item.UnregisterListener(this);
            }
        }
        #endregion
    }
}
