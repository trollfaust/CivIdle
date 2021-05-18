using UnityEngine;
using trollschmiede.CivIdle.ResourceSys;
using TMPro;

namespace trollschmiede.CivIdle.UI
{
    public class HappinessDisplay : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI text = null;
        float timeStamp;
        float timeSpan = 1f;

        public void Setup()
        {
            UpdateText();
            timeStamp = Time.time;
        }

        void Update()
        {
            if (timeStamp + timeSpan < Time.time)
            {
                timeStamp = Time.time;
                UpdateText();
            }
        }

        void UpdateText()
        {
            text.text = "Happiness: " + Mathf.RoundToInt(PeopleManager.instance.GetHappiness()).ToString() + "%";
        }
    }
}
