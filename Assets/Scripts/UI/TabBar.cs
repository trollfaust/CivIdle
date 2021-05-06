using UnityEngine;

namespace trollschmiede.CivIdle.UI
{
    public class TabBar : MonoBehaviour
    {
        public static TabBar instance;
        private void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }
            instance = this;
        }

        public GameObject[] tabSwitches = new GameObject[0];
    }
}