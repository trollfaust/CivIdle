using System.Collections;
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

        private string gatheringObjectKey = "GATHERINGOBJECT";
        [SerializeField] GatheringObject[] allGatheringObjects = new GatheringObject[0];
        [SerializeField] GameObject gatheringObjectPrefab = null;
        [SerializeField] Transform gatheringObjectContainer = null;

        private List<GatheringObject> enabledGatheringObjects;

        private void Start()
        {
            foreach (var item in allGatheringObjects)
            {
<<<<<<< HEAD
                item.peopleWorking = PlayerPrefs.GetInt(gatheringObjectKey + item.name + "PEOPLEWORKING");
                item.peopleWishedWorking = PlayerPrefs.GetInt(gatheringObjectKey + item.name + "PEOPLEWISHEDWORKING");
                item.isEnabled = (PlayerPrefs.GetInt(gatheringObjectKey + item.name + "ISENABLED") == 0) ? false : true;

=======
>>>>>>> 72f39e707d2661234c37f87e39a009cfdbe1ced0
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

            PlayerPrefs.SetInt(gatheringObjectKey + _gatheringObject.name + "PEOPLEWORKING", _gatheringObject.peopleWorking);
            PlayerPrefs.SetInt(gatheringObjectKey + _gatheringObject.name + "PEOPLEWISHEDWORKING", _gatheringObject.peopleWishedWorking);
            PlayerPrefs.SetInt(gatheringObjectKey + _gatheringObject.name + "ISENABLED", (_gatheringObject.isEnabled) ? 1 : 0);
        }

        public void Reset()
        {
            foreach (GatheringObject item in allGatheringObjects)
            {
                item.isEnabled = false;
                item.peopleWorking = 0;
                item.peopleWishedWorking = 0;
<<<<<<< HEAD

                PlayerPrefs.SetInt(gatheringObjectKey + item.name + "PEOPLEWORKING", item.peopleWorking);
                PlayerPrefs.SetInt(gatheringObjectKey + item.name + "PEOPLEWISHEDWORKING", item.peopleWishedWorking);
                PlayerPrefs.SetInt(gatheringObjectKey + item.name + "ISENABLED", (item.isEnabled) ? 1 : 0);
=======
>>>>>>> 72f39e707d2661234c37f87e39a009cfdbe1ced0
            }

            foreach (var item in gatheringObjectContainer.GetComponentsInChildren<GatheringObjectDisplay>())
            {
                Destroy(item.gameObject);
            }
            enabledGatheringObjects = new List<GatheringObject>();
        }
    }
}