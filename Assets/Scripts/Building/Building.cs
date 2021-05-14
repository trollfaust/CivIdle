using UnityEngine;
using trollschmiede.Generic.Tooltip;
using System.Collections.Generic;
using trollschmiede.CivIdle.Events;
using trollschmiede.CivIdle.Resources;

namespace trollschmiede.CivIdle.Building
{
    [CreateAssetMenu(fileName = "New Building", menuName = "Scriptable Objects/Building/Building")]
    public class Building : ScriptableObject, ITooltipValueElement
    {
        public new string name;
        public ResourceChancePair[] buildingMaterials = new ResourceChancePair[0];
        public int landNeeded = 1;

        //[HideInInspector]
        public bool isEnabled;
        [HideInInspector]
        public int buildingCount = 0;
        [HideInInspector]
        public int buildingWishedCount = 0;

        public Sprite iconSprite;

        public Dictionary<string, string> GetTooltipValues()
        {
            throw new System.NotImplementedException();
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