using UnityEngine;
using trollschmiede.CivIdle.Events;
using System.Collections.Generic;
using System;

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
        public int maxAmount;
        public int baseMaxAmount;
        public ResourceCategory resourceCategory;
        public ResoureRequierment resoureRequierment;
        public Sprite iconSprite;
        [Range(0,5f)]
        public float saturationValue = 0f;

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
            if (amount > maxAmount && maxAmount > 0)
                amount = maxAmount;
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

        public void MaxAmountChange(int value)
        {
            if (value == 0)
                return;
            maxAmount = maxAmount + value;
            if (amount > maxAmount)
            {
                amount = maxAmount;
            }
            EvokeAll();
        }

        public void EnableResource()
        {
            isEnabled = true;
            EvokeAll();
        }

        public void Reset()
        {
            maxAmount = baseMaxAmount;
            amount = 0;
            amountOpen = 0;
            isEnabled = false;
            if (resoureRequierment == ResoureRequierment.Start)
            {
                isEnabled = true;
                EvokeAll();
            }
        }
        #endregion
    }
}
