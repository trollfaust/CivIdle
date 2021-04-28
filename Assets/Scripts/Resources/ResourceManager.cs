﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using trollschmiede.CivIdle.UI;
using trollschmiede.CivIdle.Events;
using trollschmiede.CivIdle.GameEvents;
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

        private string requierementMeetKey = "REQUIERMENTMEET";
        public Resource[] allResources = null;
        [SerializeField] GameObject resourceDisplayGroupPrefab = null;
        [SerializeField] GameObject resourceDisplayPrefab = null;
        [SerializeField] Transform resourceDisplayParent = null;

        char requSeperator = '$';

        public void Evoke() { }
        public void Evoke(Resource _resource)
        {
            ActivateResource(_resource);
        }

        
        private List<ResoureRequierment> requiermentsMeet;
        private List<ResourceDisplayGroup> resourceDisplayGroups;
        private List<ResourceCategory> resourceCategories;

        private void Start()
        {
            resourceDisplayGroups = new List<ResourceDisplayGroup>();
            resourceCategories = new List<ResourceCategory>();
            System.Array.Sort(allResources, delegate (Resource x, Resource y) { return x.resourceCategory.CompareTo(y.resourceCategory); });

            for (int i = 0; i < allResources.Length; i++)
            {
                Resource item = allResources[i];
                if (!resourceCategories.Contains(item.resourceCategory))
                {
                    GameObject gO = Instantiate(resourceDisplayGroupPrefab, resourceDisplayParent) as GameObject;
                    ResourceDisplayGroup group = gO.GetComponent<ResourceDisplayGroup>();
                    string title = item.resourceCategory.ToString();
                    title = title.Replace("_", " ");
                    group.SetTitleText(title);
                    group.resourceCategory = item.resourceCategory;
                    resourceDisplayGroups.Add(group);
                    resourceCategories.Add(item.resourceCategory);
                    gO.SetActive(false);
                }
            }
            requiermentsMeet = new List<ResoureRequierment>();
            requiermentsMeet.Add(ResoureRequierment.Start);

            string safeString = PlayerPrefs.GetString(requierementMeetKey);
            if (safeString != string.Empty)
            {
                string[] cut = safeString.Split(requSeperator);
                for (int i = 0; i < cut.Length; i++)
                {
                    foreach (ResoureRequierment requierment in Enum.GetValues(typeof(ResoureRequierment)))
                    {
                        if (requierment.ToString() == cut[i])
                        {
                            AddRequierment(requierment);
                        }
                    }
                }
            }

            Reset();
        }


        public void Reset()
        {
            foreach (var resourceDisplay in GameObject.FindObjectsOfType<ResourceDisplay>())
            {
                Destroy(resourceDisplay.gameObject);
            }
            foreach (var item in allResources)
            {
                if (item.isEnabled)
                {
                    int index = resourceCategories.IndexOf(item.resourceCategory);
                    resourceDisplayGroups[index].gameObject.SetActive(true);
                    GameObject gO = Instantiate(resourceDisplayPrefab, resourceDisplayGroups[index].GetContentTransform()) as GameObject;
                    gO.GetComponent<ResourceDisplay>().SetResource(item);
                }
                item.RegisterListener(this);
            }

            ResetLayout();
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
                ResourceDisplayGroup displayGroup = null;
                foreach (var group in resourceDisplayGroups)
                {
                    if (group.resourceCategory == _resource.resourceCategory)
                    {
                        displayGroup = group;
                        displayGroup.gameObject.SetActive(true);
                    }
                }
                if (displayGroup == null)
                    return;
                GameObject gO = Instantiate(resourceDisplayPrefab, displayGroup.GetContentTransform()) as GameObject;
                gO.GetComponent<ResourceDisplay>().SetResource(_resource);

                ResetLayout();
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
            string safeString = PlayerPrefs.GetString(requierementMeetKey);
            safeString = safeString + _requierment.ToString() + requSeperator;
            PlayerPrefs.SetString(requierementMeetKey, safeString);
        }

        
        void ResetLayout()
        {
            Canvas.ForceUpdateCanvases();
            resourceDisplayParent.gameObject.GetComponent<VerticalLayoutGroup>().enabled = false;
            resourceDisplayParent.gameObject.GetComponent<VerticalLayoutGroup>().enabled = true;
        }
    }
}
