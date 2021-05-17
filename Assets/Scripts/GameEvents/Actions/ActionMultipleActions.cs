﻿using UnityEngine;

namespace trollschmiede.CivIdle.GameEventSys
{
    [CreateAssetMenu(fileName = "New Action multiple Actions", menuName = "Scriptable Objects/Actions/Action multiple Actions")]
    public class ActionMultipleActions : Action
    {
        [SerializeField] Action[] actions = null;
        [SerializeField] bool invertOverrideValue = false;
        [SerializeField] bool notOverrideValues = false;

        int lastValue = 0;

        public override bool EvokeAction()
        {
            lastValue = 0;
            if (actions == null)
            {
                return false;
            }
            for (int index = 0; index < actions.Length; index++)
            {
                Action action = actions[index];
                action.overrideValue = lastValue;
                if (action.EvokeAction())
                {
                    int output = action.GetLastValue();
                    if (notOverrideValues)
                        continue;
                    
                    lastValue = (invertOverrideValue) ? -output : output;
                }
            }
            return true;
        }
        public override string GetActionString()
        {
            string actionsString = "";
            foreach (Action action in actions)
            {
                actionsString = actionsString + " " + action.GetActionString();
            }

            return actionsString;
        }
        public override int GetLastValue()
        {
            return lastValue;
        }
    }
}
