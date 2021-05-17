using UnityEngine;
using trollschmiede.Generic.Tooltip;
using System.Collections.Generic;
using trollschmiede.CivIdle.EventSys;
using trollschmiede.CivIdle.ResourceSys;

namespace trollschmiede.CivIdle.BuildingSys
{
    [CreateAssetMenu(fileName = "New Building", menuName = "Scriptable Objects/Building/Building")]
    public class Building : ScriptableObject, ITooltipValueElement
    {
        public new string name;
        public ResourceChancePair[] buildingMaterials = new ResourceChancePair[0];
        public int landNeeded = 1;

        public int housingValue = 0;

        //[HideInInspector]
        public bool isEnabled;
        [HideInInspector]
        public int buildingCount = 0;
        [HideInInspector]
        public int buildingWishedCount = 0;

        public Sprite iconSprite;

        public Dictionary<string, string> GetTooltipValues()
        {
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
            keyValuePairs.Add("name", name);
            keyValuePairs.Add("landNeeded", landNeeded.ToString() + " Land");
            keyValuePairs.Add("housingValue", housingValue.ToString() + " People");

            string buildingMats = "";
            for (int i = 0; i < buildingMaterials.Length; i++)
            {
                ResourceChancePair item = buildingMaterials[i];
                string itemName = item.resource.name;
                string minAmount = item.minValue.ToString();
                string maxAmount = item.maxValue.ToString();
                string chance = item.chance.ToString();

                buildingMats = buildingMats + minAmount + ((maxAmount == minAmount) ? "" : "-" + maxAmount) + " " + itemName + ((item.chance < 100) ? " at a " + chance + "% Chance" : "") + ((i == buildingMaterials.Length - 1) ? "" : ", ");
            }
            keyValuePairs.Add("buildingMaterials", buildingMats);

            return keyValuePairs;
        }

        public void Reset()
        {
            buildingCount = 0;
            buildingWishedCount = 0;
            isEnabled = false;
        }

        #region Event Managment
        private List<IEventListener> listeners;

        public void RegisterListener(IEventListener _listener)
        {
            if (_listener == null)
                return;

            if (listeners == null)
                listeners = new List<IEventListener>();

            listeners.Add(_listener);
        }

        public void UnregisterListener(IEventListener _listener)
        {
            if (_listener == null)
                return;

            listeners?.Remove(_listener);
        }

        public void EvokeAll()
        {
            if (listeners == null)
                return;

            foreach (var listener in listeners)
            {
                listener.Evoke();
            }
        }
        #endregion
    }
}