using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using trollschmiede.CivIdle.Resources;
using TMPro;

public class Test : MonoBehaviour
{
    public Resource resource;

    public void OnPressed() => resource.AmountChange(1);


    public TextMeshProUGUI text;
    private string LastClickedWord;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var wordIndex = TMP_TextUtilities.FindIntersectingWord(text, Input.mousePosition, null);

            if (wordIndex != -1)
            {
                LastClickedWord = text.textInfo.wordInfo[wordIndex].GetWord();

                Debug.Log("Clicked on " + LastClickedWord);
            }
        }
    }
}
