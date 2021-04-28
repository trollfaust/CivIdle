﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using trollschmiede.CivIdle.UI;

namespace trollschmiede.CivIdle.Resources
{
    public class GatheringManager : MonoBehaviour
    {
        #region Singleton
        public static GatheringManager instance;
        private void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }
            instance = this;
        }
        #endregion

        [SerializeField] GatheringObject[] allGatheringObjects = new GatheringObject[0];
        [SerializeField] GameObject gatheringObjectPrefab = null;
        [SerializeField] Transform gatheringObjectContainer = null;

        private List<GatheringObject> enabledGatheringObjects;

        private void Start()
        {
            foreach (var item in allGatheringObjects)
            {
                if (item.isEnabled)
                {
                    EnableGatheringObject(item);
                }
            }
        }

        public void EnableGatheringObject(GatheringObject _gatheringObject)
        {
            if (enabledGatheringObjects == null)
            {
                enabledGatheringObjects = new List<GatheringObject>();
            }
            if (enabledGatheringObjects.Contains(_gatheringObject))
            {
                return;
            }
            enabledGatheringObjects.Add(_gatheringObject);
            _gatheringObject.isEnabled = true;

            GameObject newGO = Instantiate(gatheringObjectPrefab, gatheringObjectContainer, false) as GameObject;
            newGO.GetComponent<GatheringObjectDisplay>().Setup(_gatheringObject);
        }

        public void Reset()
        {
            foreach (GatheringObject item in allGatheringObjects)
            {
                item.isEnabled = false;
                item.peopleWorking = 0;
                item.peopleWishedWorking = 0;
            }

            foreach (var item in gatheringObjectContainer.GetComponentsInChildren<GatheringObjectDisplay>())
            {
                Destroy(item.gameObject);
            }
            enabledGatheringObjects = new List<GatheringObject>();
        }
    }
}