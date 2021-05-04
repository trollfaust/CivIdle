using UnityEngine;
using System.Collections.Generic;

namespace trollschmiede.CivIdle.UI
{
    public class FloatingTextManager : MonoBehaviour
    {
        #region Singleton
        public static FloatingTextManager instance;
        private void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }
            instance = this;
        }
        #endregion

        [SerializeField] GameObject floatingTextPrefab = null;
        List<FloatingText> allFloatingTexts;

        private void Start()
        {
            allFloatingTexts = new List<FloatingText>();
        }

        public void SetFloatingText(Transform _parent, int _value)
        {
            FloatingText newFloatingText = null;

            foreach (FloatingText floatingText in allFloatingTexts)
            {
                if (floatingText.isRunning)
                {
                    continue;
                }
                newFloatingText = floatingText;
            }

            if (newFloatingText == null)
            {
                GameObject newFloatingTextGO = Instantiate(floatingTextPrefab) as GameObject;
                newFloatingText = newFloatingTextGO.GetComponent<FloatingText>();
            }

            newFloatingText.SetValue(_value);
            newFloatingText.transform.SetParent(_parent);
            newFloatingText.transform.localScale = new Vector3(1f, 1f, 1f);
            newFloatingText.transform.localPosition = new Vector3(_parent.GetComponent<RectTransform>().rect.width / 2, 0f, 0f);

            allFloatingTexts.Add(newFloatingText);
        }

        public void ResetFloatingText(FloatingText floatingText)
        {
            floatingText.transform.SetParent(this.transform);
        }
    }
}
