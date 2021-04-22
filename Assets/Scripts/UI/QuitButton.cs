using UnityEngine;

namespace trollschmiede.CivIdle.UI
{
    public class QuitButton : MonoBehaviour
    {
        public void OnButtonPressed()
        {
            Application.Quit();
        }
    }
}