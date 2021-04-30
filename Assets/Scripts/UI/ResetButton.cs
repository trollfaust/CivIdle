using UnityEngine;
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
<<<<<<< HEAD
=======
            PlayerPrefs.SetInt(firstLaunchKey, 1);
>>>>>>> 72f39e707d2661234c37f87e39a009cfdbe1ced0
            StartCoroutine(OnStartUp());
            PlayerPrefs.SetInt(firstLaunchKey, 1);
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