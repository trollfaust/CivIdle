using UnityEngine;
using trollschmiede.CivIdle.UI;

namespace trollschmiede.CivIdle.GameEventSys
{
    [CreateAssetMenu(fileName = "New Action Unlock Tab", menuName = "Scriptable Objects/Actions/Action Unlock Tab")]
    public class ActionUnlockTab : Action
    {
        [SerializeField] string tabName = "";

        int lastValue = 0;

        public override bool EvokeAction()
        {
            
            foreach (GameObject tabSwitch in TabBar.instance.tabSwitches)
            {
                if (tabSwitch.GetComponent<TabSwitch>().tab.name.Contains(tabName))
                {
                    tabSwitch.gameObject.SetActive(true);
                    return true;
                }
            }

            return false;
        }
        public override string GetActionString()
        {
            return "Unlocks Tab " + tabName;
        }
        public override int GetLastValue()
        {
            return lastValue;
        }
    }
}
