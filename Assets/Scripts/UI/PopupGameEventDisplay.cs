using UnityEngine;
using trollschmiede.CivIdle.GameEventSys;
using TMPro;

namespace trollschmiede.CivIdle.UI
{
    public class PopupGameEventDisplay : MonoBehaviour
    {
        [SerializeField] GameObject buttonPrefab = null;
        [SerializeField] Transform buttonContainer = null;
        [SerializeField] TextMeshProUGUI title = null;
        [SerializeField] TextMeshProUGUI description = null;

        PopupGameEvent gameEvent;

        public void Setup(PopupGameEvent _gameEvent)
        {
            gameEvent = _gameEvent;

            title.text = gameEvent.title;
            description.text = gameEvent.description;

            for (int i = 0; i < gameEvent.answers.Length; i++)
            {
                string buttonText = gameEvent.answers[i];
                GameObject go = Instantiate(buttonPrefab, buttonContainer) as GameObject;
                Requierment requierment = null;
                if (gameEvent.answerRequierments.Length > i)
                {
                    requierment = gameEvent.answerRequierments[i];
                }

                go.GetComponent<PopupEventButton>().Setup(this, i, buttonText, requierment);
            }
        }

        public void ButtonPressed(int id)
        {
            gameEvent.EvokeAction(id);
            Destroy(gameObject);
        }
    }
}