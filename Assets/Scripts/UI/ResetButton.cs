using UnityEngine;
using trollschmiede.CivIdle.Resources;
using System.Collections;
using trollschmiede.CivIdle.Generic;

namespace trollschmiede.CivIdle.UI
{
    public class ResetButton : MonoBehaviour
    {
        private void Start()
        {
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