using UnityEngine;
using trollschmiede.CivIdle.Events;
using System.Collections.Generic;

namespace trollschmiede.CivIdle.Resources
{
    [CreateAssetMenu(fileName = "New Resource", menuName = "Scriptable Objects/Resources/Resource")]
    public class Resource : ScriptableObject
    {
        public int id;
        public new string name;
        public bool isEnabled;
        public int amount;
        public ResourceCategory resourceCategory;
        public ResoureRequierment resoureRequierment;
        public Sprite iconSprite;
        private List<IEventListener> listeners;

        #region Event Managment
        public void RegisterListener(IEventListener listener)
        {
            if (listeners == null)
            {
                listeners = new List<IEventListener>();
            }
            listeners.Add(listener);
        }

        public void UnregisterListener(IEventListener listener)
        {
            listeners.Remove(listener);
        }

        private void EvokeAll()
        {
            foreach (var listener in listeners)
            {
                listener.Evoke(this);
            }
        }
        #endregion
        #region Resource Changes
        public void AmountChange(int value)
        {
            if (value == 0)
            {
                return;
            }
            amount = amount + value;
            EvokeAll();
        }

        public void EnableResource()
        {
            isEnabled = true;
            EvokeAll();
        }
        #endregion
    }
}
