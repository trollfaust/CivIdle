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
        public int amountOpen;
        [HideInInspector]
        public int maxAmount;
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
            if (listeners == null)
                return;

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
            if (amount > GetTempMaxAmount() && GetTempMaxAmount() > 0)
                amount = GetTempMaxAmount();
            EvokeAll();
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

        public Dictionary<string, string> GetTooltipValues()
        {
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();

            keyValuePairs.Add("name", name);
            keyValuePairs.Add("amount", amount.ToString());
            keyValuePairs.Add("amountOpen", amountOpen.ToString());
            keyValuePairs.Add("maxAmount", maxAmount.ToString());
            keyValuePairs.Add("saturationValue", typeAmountMultiplier.ToString());
            keyValuePairs.Add("cultureValue", cultureValue.ToString());

            return keyValuePairs;
        }
        #endregion
    }
}
