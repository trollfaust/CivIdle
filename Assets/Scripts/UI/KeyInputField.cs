using UnityEngine;
using UnityEngine.EventSystems;

namespace trollschmiede.CivIdle.UI
{
    public class KeyInputField : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] KeySettingUi settingUi = null;

        public void OnPointerClick(PointerEventData eventData)
        {
            settingUi.OnFocus();
            FindObjectOfType<KeySettingManager>().SetKey();
        }
    }
}