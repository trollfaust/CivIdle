using System.Collections.Generic;
using trollschmiede.CivIdle.Generic;
using UnityEngine;

namespace trollschmiede.CivIdle.BuildingSys
{
    public class BuildingManager : MonoBehaviour
    {
        #region Singleton
        public static BuildingManager instance;
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

        public GameObject buildingDisplayPrefab = null;
        public Transform buildingContainer = null;
        public Building[] allBuildings = new Building[0];

        List<BuildingDisplay> buildingDisplays;
        List<Building> enabledBuildings;

        bool isSetup = false;
        public bool Setup()
        {
            buildingDisplays = new List<BuildingDisplay>();

            bool check = true;
            foreach (Building building in allBuildings)
            {
                if (building.isEnabled)
                {
                    if (InitBuilding(building) == false)
                    {
                        check = false;
                    }
                }
            }

            isSetup = check;
            return isSetup;
        }

        public void Tick()
        {
            if (isSetup == false)
                return;

            foreach (BuildingDisplay display in buildingDisplays)
            {
                display.Tick();
            }
        }

        public bool EnableBuilding(Building _building)
        {
            if (enabledBuildings == null)
            {
                enabledBuildings = new List<Building>();
            }
            if (enabledBuildings.Contains(_building))
            {
                GameManager.instance.CheckLogWarning(_building.name + " already exists in enabledBuildings!");
                return false;
            }

            enabledBuildings.Add(_building);
            _building.isEnabled = true;

            bool check = InitBuilding(_building);

            GameManager.instance.CheckLogWarning(check, "Building " + _building.name + " failed to Instantiate!");

            return check;
        }

        bool InitBuilding(Building _building)
        {
            if (buildingDisplays == null)
            {
                buildingDisplays = new List<BuildingDisplay>();
            }
            GameObject go = Instantiate(buildingDisplayPrefab, buildingContainer) as GameObject;
            BuildingDisplay display = go.GetComponent<BuildingDisplay>();
            buildingDisplays.Add(display);

            bool check = display.Setup(_building);
            return check;
        }

        public void Reset()
        {
            foreach (Building building in allBuildings)
            {
                building.Reset();
            }
        }
    }
}