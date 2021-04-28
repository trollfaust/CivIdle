using UnityEngine;
using trollschmiede.CivIdle.Resources;
using System.Collections;
using trollschmiede.CivIdle.Generic;

namespace trollschmiede.CivIdle.UI
{
    public class ResetButton : MonoBehaviour
    {
        private string firstLaunchKey = "FIRSTLAUNCHDONE";

        private void Start()
        {
            if (PlayerPrefs.GetInt(firstLaunchKey) >= 1)
            {
                return;
            }
            PlayerPrefs.SetInt(firstLaunchKey, 1);
            StartCoroutine(OnStartUp());
        }

        public void OnButtonPressed()
        {
            Reset.ResetData();
        }

        IEnumerator OnStartUp()
        {
            yield return new WaitForSeconds(.1f);
            OnButtonPressed();
        }
    }
}