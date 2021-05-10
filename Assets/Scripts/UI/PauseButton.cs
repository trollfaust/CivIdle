using UnityEngine;
using trollschmiede.Generic.Tooltip;
using trollschmiede.CivIdle.Util;

namespace trollschmiede.CivIdle.UI
{
    public class PauseButton : MonoBehaviour
    {
        public void OnButtonPressed()
        {
            Pause.TogglePause();
        }

        private void Start()
        {
            GetComponent<TooltipHoverElement>().TooltipInitialize("Pause");
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                OnButtonPressed();
            }
        }

    }
}
