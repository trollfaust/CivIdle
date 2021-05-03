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
        private List<GatheringObjectDisplay> gatheringObjectDisplays = null;

        #region Setup
        bool isSetup = false;
        public bool Setup()
        {
            gatheringObjectDisplays = new List<GatheringObjectDisplay>();

            isSetup = true;
            return isSetup;
        }
        #endregion

        #region Update Tick
        public void Tick()
        {
            if (isSetup == false)
                return;

            foreach (GatheringObjectDisplay gatheringObjectDisplay in gatheringObjectDisplays)
            {
                gatheringObjectDisplay.Tick();
            }
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
            GatheringObjectDisplay gatheringObjectDisplay = newGO.GetComponent<GatheringObjectDisplay>();
            bool check = gatheringObjectDisplay.Setup(_gatheringObject);

            GameManager.instance.CheckLogWarning(check, "Gathering Object " + _gatheringObject.name + " failed to Instantiate!");

            if (check)
                gatheringObjectDisplays.Add(gatheringObjectDisplay);
            

            return check;
        }

        /// <summary>
        /// Resets All Gethering Objects
        /// </summary>
        public void Reset()
        {
            foreach (GatheringObject gatheringObject in allGatheringObjects)
            {
                gatheringObject.Reset();
            }

            foreach (var item in gatheringObjectContainer.GetComponentsInChildren<GatheringObjectDisplay>())
            {
                Destroy(item.gameObject);
            }

            gatheringObjectDisplays = new List<GatheringObjectDisplay>();
            enabledGatheringObjects = new List<GatheringObject>();
        }
    }
}