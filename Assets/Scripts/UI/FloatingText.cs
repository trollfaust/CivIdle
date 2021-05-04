using UnityEngine;
using TMPro;

namespace trollschmiede.CivIdle.UI
{
    public class FloatingText : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI text = null;
        [SerializeField] float timeOut = 0f;
        [SerializeField] CanvasGroup canvasGroup = null;

        Transform myTransform;
        bool isPositive = false;
        float timePassed = 0f;
        [HideInInspector]
        public bool isRunning = false;
        float rng = 0f; 

        private void Start()
        {
            myTransform = gameObject.transform;
        }

        public void SetValue(int value)
        {
            text.text = (value >= 0) ? "+" + value.ToString() : value.ToString();
            if (value > 0)
            {
                isPositive = true;
            }
            else
            {
                isPositive = false;
            }
            timePassed = 0f;
            canvasGroup.alpha = 1;
            rng = Random.Range(-10f, 10f);
            isRunning = true;
        }

        private void Update()
        {
            if (isRunning == false)
                return;
            
            if (timePassed > timeOut / 2)
            {
                canvasGroup.alpha = Mathf.Lerp(1, 0, timePassed / timeOut);
            }
            timePassed += Time.deltaTime;

            if (isPositive)
            {
                myTransform.position = new Vector3(myTransform.position.x + (Time.deltaTime * 10) + (rng / 400), myTransform.position.y + (Time.deltaTime * 30), 0);
            } else
            {
                myTransform.position = new Vector3(myTransform.position.x + (Time.deltaTime * 10) + (rng / 400), myTransform.position.y - (Time.deltaTime * 30), 0);
            }

            if (timePassed > timeOut)
            {
                FloatingTextManager.instance.ResetFloatingText(this);
                isRunning = false;
            }
        }
    }
}
