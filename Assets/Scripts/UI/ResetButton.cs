using UnityEngine;
using trollschmiede.CivIdle.Resources;
using trollschmiede.CivIdle.GameEvents;

namespace trollschmiede.CivIdle.UI
{
    public class ResetButton : MonoBehaviour
    {
        [SerializeField] GameEvent[] gameEvents;
        public void OnButtonPressed()
        {
            Reset.ResetData();
            foreach (var item in gameEvents)
            {
                item.Reset();
            }
        }
    }
}