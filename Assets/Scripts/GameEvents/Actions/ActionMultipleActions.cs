using UnityEngine;

namespace trollschmiede.CivIdle.GameEvents
{
    [CreateAssetMenu(fileName = "New Action multiple Actions", menuName = "Scriptable Objects/Actions/Action multiple Actions")]
    public class ActionMultipleActions : Action
    {
        [SerializeField] Action[] actions = null;
        [SerializeField] bool invertOverrideValue = false;

        public override int EvokeAction()
        {
            int tempValue = 0;
            if (actions == null)
            {
                return 0;
            }
            for (int index = 0; index < actions.Length; index++)
            {
                Action action = actions[index];
                action.overrideValue = tempValue;
                int output = action.EvokeAction();
                tempValue = (invertOverrideValue) ? -output : output;           
            }
            return tempValue;
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
    }
}
