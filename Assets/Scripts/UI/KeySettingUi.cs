using UnityEngine;
using TMPro;
using System.Collections.Generic;
using trollschmiede.CivIdle.Util;
using UnityEngine.UI;

namespace trollschmiede.CivIdle.UI
{
    public class KeySettingUi : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI title = null;
        [SerializeField] TMP_InputField inputField = null;
        [SerializeField] Color highlightColor = new Color();

        public bool isFocused = false;
        public KeyCodeNames codeName;
        public KeyCode keyCode;

        Color oldColor;

        public void Setup(KeyValuePair<KeyCodeNames, KeyCode> pair)
        {
            title.text = pair.Key.ToString();
            inputField.text = pair.Value.ToString();
            codeName = pair.Key;
            keyCode = pair.Value;
        }

        public void UpdateKey(KeyCode key)
        {
            keyCode = key;
            inputField.text = key.ToString();
            isFocused = false;
            inputField.GetComponent<Image>().color = oldColor;
        }

        public void OnFocus()
        {
            isFocused = true;
            oldColor = inputField.GetComponent<Image>().color;
            inputField.GetComponent<Image>().color = highlightColor;
        }
    }
}