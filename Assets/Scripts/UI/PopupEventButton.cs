using UnityEngine;
using TMPro;
using trollschmiede.CivIdle.GameEvents;
using UnityEngine.UI;

namespace trollschmiede.CivIdle.UI
{
    public class PopupEventButton : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI text = null;

        PopupGameEventDisplay eventDisplay;
        int id;

        public void Setup(PopupGameEventDisplay _eventDisplay, int _id, string _buttonText, Requierment requierment = null)
        {
            eventDisplay = _eventDisplay;
            id = _id;
            text.text = _buttonText;
            if (requierment != null && requierment.CheckRequierment() == false)
            {
                GetComponent<Button>().interactable = false;
            }
        }

        public void OnButtonClicked()
        {
            eventDisplay.ButtonPressed(id);
        }
    }
}