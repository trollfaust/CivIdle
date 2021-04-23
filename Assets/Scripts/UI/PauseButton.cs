using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseButton : MonoBehaviour
{
    bool isInPause = false;
    public void OnButtonPressed()
    {
        if (isInPause)
        {
            Time.timeScale = 1f;
            isInPause = false;
        } else
        {
            Time.timeScale = 0f;
            isInPause = true;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnButtonPressed();
        }
    }
}
