using UnityEngine;
using TMPro;
using trollschmiede.CivIdle.Resources;

namespace trollschmiede.CivIdle.UI
{
    public class ResourceDisplayGroup : MonoBehaviour
    {
        [SerializeField] Transform contentTransform = null;
        [SerializeField] TextMeshProUGUI title = null;
        [HideInInspector]
        public ResourceCategory resourceCategory;

        public void SetTitleText(string _text)
        {
            title.text = _text;
        }

        public Transform GetContentTransform()
        {
            return contentTransform;
        }
    }
}
