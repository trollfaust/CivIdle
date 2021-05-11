using UnityEngine;
using trollschmiede.CivIdle.Events;
using System.Collections.Generic;
using trollschmiede.Generic.Tooltip;

namespace trollschmiede.CivIdle.Resources
{
    [CreateAssetMenu(fileName = "New Resource", menuName = "Scriptable Objects/Resources/Resource")]
    public class Resource : ScriptableObject, ITooltipValueElement
    {
        public int id;
        public new string name;
        [HideInInspector]
        public bool isEnabled;
        [HideInInspector]
        public int amount;
        public bool hasAmountOpen;
        [HideInInspector]
        public int openAmount;
        [HideInInspector]
        private int maxAmount;
        public int baseMaxAmount;
        public ResourceCategory resourceCategory;
        public ResoureRequierment resoureRequierment;
        public Sprite iconSprite;
        
        [Range(0,5f)]
        public float typeAmountMultiplier = 1f;
        [Range(0,5)]
        public int cultureValue = 0;

        public Resource[] overflowResources;


        private List<IResourceEventListener> listeners;

        #region Event Managment
        public void RegisterListener(IResourceEventListener _listener)
        {
            if (_listener == null)
                return;
            
            if (listeners == null)
                listeners = new List<IResourceEventListener>();
            
            listeners.Add(_listener);
        }

        public void UnregisterListener(IResourceEventListener _listener)
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
                listener.Evoke(this);
            }
        }
        #endregion

        #region Resource Changes
        public bool AmountChange(int value)
        {
            if (value == 0)
                return true;
            if (value < 0 && amount + value < 0)
                return false;

            amount = amount + value;
            if (hasAmountOpen)
                openAmount = openAmount + value;
            if (amount > GetTempMaxAmount() && GetTempMaxAmount() > 0)
                amount = GetTempMaxAmount();
            EvokeAll();
            return true;
        }

        public int GetTempMaxAmount()
        {
            int tempMaxAmount = maxAmount;
            if (overflowResources != null && overflowResources.Length != 0)
            {
                foreach (var item in overflowResources)
                {
                    tempMaxAmount -= item.amount;
                }
            }
            return tempMaxAmount;
        }

        public int GetMaxAmount()
        {
            return maxAmount;
        }

        public bool AmountOpenChange(int value)
        {
            if (value == 0)
                return true;
            if (value < 0 && openAmount + value < 0)
                return false;
            if (hasAmountOpen)
                openAmount = openAmount + value;
            EvokeAll();
            return true;
        }

        public bool MaxAmountChange(int value)
        {
            if (value == 0)
                return true;
            if (value < 0 && maxAmount + value < 0)
                return false;
            maxAmount = maxAmount + value;
            if (amount > maxAmount)
            {
                amount = maxAmount;
            }
            EvokeAll();
            return true;
        }

        public void Reset()
        {
            maxAmount = baseMaxAmount;
            amount = 0;
            openAmount = 0;
            isEnabled = false;
            if (resoureRequierment == ResoureRequierment.Start)
            {
                isEnabled = true;
                EvokeAll();
            }
        }

        public Dictionary<string, string> GetTooltipValues()
        {
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();

            keyValuePairs.Add("name", name);
            keyValuePairs.Add("amount", amount.ToString());
            keyValuePairs.Add("amountOpen", openAmount.ToString());
            keyValuePairs.Add("maxAmount", maxAmount.ToString());
            keyValuePairs.Add("saturationValue", typeAmountMultiplier.ToString());
            keyValuePairs.Add("cultureValue", cultureValue.ToString());

            return keyValuePairs;
        }
        #endregion
    }
}
