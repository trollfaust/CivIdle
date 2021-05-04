using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class MapCamera : MonoBehaviour
{
    Camera myCamera;
    [SerializeField] GameObject[] exculudedFromViewHeight = new GameObject[0];
    [SerializeField] GameObject[] exculudedFromViewWidth = new GameObject[0];
    [SerializeField] float padding = 20f;

    float screenWidth;
    float screenHeight;

    private void Start()
    {
        myCamera = GetComponent<Camera>();

        ChangeView();
    }

    private void Update()
    {
        if (screenWidth != Screen.width || screenHeight != Screen.height)
        {
            ChangeView();
        }
    }

    //TODO: fix this
    void ChangeView()
    {
        screenHeight = Screen.height;
        screenWidth = Screen.width;

        float camWidth = 1f;
        float camHeight = 1f;
        float camX = 0f;
        float camY = 0f;

        foreach (GameObject item in exculudedFromViewHeight)
        {
            RectTransform itemRect = item.GetComponent<RectTransform>();
            float yOffset = item.transform.position.y - (itemRect.rect.height / 2);
            float calc = ((itemRect.rect.height + ((item.transform.position.y < Screen.height / 2) ? yOffset : Screen.height - yOffset)) / Screen.height);

            camHeight = camHeight - (calc + (padding / Screen.height));
            if (item.transform.position.y < Screen.height / 2)
            {
                camY = camY + (calc + ((padding / Screen.height) / 2));
            }
        }
        foreach (GameObject item in exculudedFromViewWidth)
        {
            RectTransform itemRect = item.GetComponent<RectTransform>();
            float xOffset = item.transform.position.x - (itemRect.rect.width / 2);
            float calc = ((itemRect.rect.width + ((item.transform.position.x < Screen.width / 2) ? xOffset : Screen.width - xOffset)) / Screen.width);
            Debug.Log("Pos: " + item.transform.position.x);
            Debug.Log("Width: " + itemRect.rect.width);

            camWidth = camWidth - (calc + (padding / Screen.width));
            if (item.transform.position.x < Screen.width / 2)
            {
                camX = camX + (calc + (padding / Screen.width));
            }
        }

        myCamera.rect = new Rect(camX, camY, camWidth, camHeight);
    }

}
