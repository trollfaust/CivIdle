using UnityEngine;
using trollschmiede.CivIdle.Resources;

namespace trollschmiede.CivIdle.UI
{
    public class ResetButton : MonoBehaviour
    {
        public void OnButtonPressed()
        {
            Reset.ResetData();
        }
    }
}