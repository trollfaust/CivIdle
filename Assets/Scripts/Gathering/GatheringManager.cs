using System.Collections.Generic;
using UnityEngine;
using trollschmiede.CivIdle.UI;
using trollschmiede.CivIdle.Generic;

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

        #region Setup
        bool isSetup = false;
        public bool Setup()
        {
            bool checkAll = true;
            foreach (var item in allGatheringObjects)
            {
                if (item.isEnabled)
                {
                    bool check = EnableGatheringObject(item);
                    if (check == false)
                        checkAll = false;
                }
            }

            if (checkAll == false)
            {
                return false;
            }

            isSetup = true;
            return isSetup;
        }
        #endregion

        #region Update Tick
        public void Tick()
        {
            if (isSetup == false)
                return;

        }
        #endregion


        /// <summary>
        /// Instantiates a Gathering Object as GameObject and set it up
        /// </summary>
        /// <param name="_gatheringObject"></param>
        /// <returns></returns>
        public bool EnableGatheringObject(GatheringObject _gatheringObject)
        {
            if (enabledGatheringObjects == null)
            {
                enabledGatheringObjects = new List<GatheringObject>();
            }
            if (enabledGatheringObjects.Contains(_gatheringObject))
            {
                GameManager.instance.CheckLogWarning(_gatheringObject.name + " already exists in enabledGatheringObjects!");
                return false;
            }

            enabledGatheringObjects.Add(_gatheringObject);
            _gatheringObject.isEnabled = true;

            GameObject newGO = Instantiate(gatheringObjectPrefab, gatheringObjectContainer, false) as GameObject;
            bool check = newGO.GetComponent<GatheringObjectDisplay>().Setup(_gatheringObject);

            GameManager.instance.CheckLogWarning(check, "Gathering Object " + _gatheringObject.name + " failed to Instantiate!");

            return check;
        }

        /// <summary>
        /// Resets All Gethering Objects
        /// </summary>
        public void Reset()
        {
            foreach (GatheringObject item in allGatheringObjects)
            {
                item.Reset();
            }

            foreach (var item in gatheringObjectContainer.GetComponentsInChildren<GatheringObjectDisplay>())
            {
                Destroy(item.gameObject);
            }

            enabledGatheringObjects = new List<GatheringObject>();
        }
    }
}