using UnityEngine;
using trollschmiede.Generic.Tooltip;
using UnityEngine.SceneManagement;

public class TutorialStart : MonoBehaviour
{
    [SerializeField] TooltipHoverElement hoverElement = null;
    bool hasClicked = false;

    void Start()
    {
        hoverElement.TooltipInitialize("Welcome");
    }

    void Update()
    {
        if (Input.GetKeyDown(TooltipManager.instance.keySettings.unlockKey))
        {
            if (hasClicked == true)
            {
                SceneManager.LoadScene(2);
            }

            hasClicked = true;
        }
    }
}
