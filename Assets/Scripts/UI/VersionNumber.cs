using UnityEngine;
using TMPro;

namespace trollschmiede.CivIdle.UI
{
    public class VersionNumber : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI text = null;

        private void Start()
        {
            text.text = Application.version;
        }
    }
}