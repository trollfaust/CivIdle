using System.Collections.Generic;
using trollschmiede.CivIdle.Util;
using UnityEngine;

namespace trollschmiede.CivIdle.UI
{
    public class KeySettingManager : MonoBehaviour
    {
        [SerializeField] GameObject KeySettingPrefab = null;
        [SerializeField] KeySettings keySettings = null;

        List<KeySettingUi> keySettingUis;
        bool isSetKeyMode = false;

        private void Start()
        {
            keySettingUis = new List<KeySettingUi>();
            foreach (KeyValuePair<KeyCodeNames, KeyCode> pair in keySettings.GetKeys())
            {
                GameObject newGo = Instantiate(KeySettingPrefab, this.transform) as GameObject;
                KeySettingUi settingUi = newGo.GetComponent<KeySettingUi>();
                settingUi.Setup(pair);

                keySettingUis.Add(settingUi);
            }
        }

        private void OnGUI()
        {
            if (isSetKeyMode && Input.anyKeyDown)
            {
                KeyCode key = KeyCode.Escape;
                Event e = Event.current;
                if (e != null && (e.isKey || (e.isMouse && e.type == EventType.MouseDown)))
                {
                    if (e.isKey)
                    {
                        key = e.keyCode;
                    }
                    if (e.isMouse)
                    {
                        switch (e.button)
                        {
                            case 0:
                                key = KeyCode.Mouse0;
                                break;
                            case 1:
                                key = KeyCode.Mouse1;
                                break;
                            case 2:
                                key = KeyCode.Mouse2;
                                break;
                            case 3:
                                key = KeyCode.Mouse3;
                                break;
                            case 4:
                                key = KeyCode.Mouse4;
                                break;
                            case 5:
                                key = KeyCode.Mouse5;
                                break;
                            case 6:
                                key = KeyCode.Mouse6;
                                break;
                            default:
                                break;
                        }
                    }

                    foreach (KeySettingUi keySetting in keySettingUis)
                    {
                        if (keySetting.isFocused)
                        {
                            keySetting.UpdateKey(key);
                            keySettings.ChangeKey(new KeyValuePair<KeyCodeNames, KeyCode>(keySetting.codeName, key));
                        }
                    }

                    isSetKeyMode = false;
                }
            }
        }

        public void SetKey()
        {
            isSetKeyMode = true;
        }
    }
}