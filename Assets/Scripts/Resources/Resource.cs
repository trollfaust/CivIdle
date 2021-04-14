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
        public bool hasAmountOpen;
        public int amountOpen;
        public ResourceCategory resourceCategory;
        public ResoureRequierment resoureRequierment;
        public Sprite iconSprite;
        private List<IResourceEventListener> listeners;

        #region Event Managment
        public void RegisterListener(IResourceEventListener listener)
        {
            if (listeners == null)
            {
                listeners = new List<IResourceEventListener>();
            }
            listeners.Add(listener);
        }

        public void UnregisterListener(IResourceEventListener listener)
        {
            listeners.Remove(listener);
        }

        public void EvokeAll()
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
                return;
            amount = amount + value;
            if (hasAmountOpen)
                amountOpen = amountOpen + value;
            EvokeAll();
        }

        public void AmountOpenChange(int value)
        {
            if (value == 0)
                return;
            if (hasAmountOpen)
                amountOpen = amountOpen + value;
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
