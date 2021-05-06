using System.Collections;
using System.Collections.Generic;
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
        if (Input.GetMouseButtonDown(0))
        {
            if (hasClicked == true)
            {
                SceneManager.LoadScene(1);
            }

            hasClicked = true;
        }
    }
}
