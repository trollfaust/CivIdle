using UnityEngine;
using trollschmiede.CivIdle.Resources;
using trollschmiede.CivIdle.GameEvents;

namespace trollschmiede.CivIdle.UI
{
    public class ResetButton : MonoBehaviour
    {
        [SerializeField] GameEvent[] gameEvents = new GameEvent[0];
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