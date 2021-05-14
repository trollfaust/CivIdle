using System.Collections.Generic;
using UnityEngine;

namespace trollschmiede.CivIdle.Building
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
        public Building[] buildings = new Building[0];

        List<BuildingDisplay> buildingDisplays;

        bool isSetup = false;
        public bool Setup()
        {
            buildingDisplays = new List<BuildingDisplay>();

            bool check = true;
            foreach (Building building in buildings)
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

    }
}