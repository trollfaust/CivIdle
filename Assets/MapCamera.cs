using UnityEngine;

[RequireComponent(typeof(Camera))]
public class MapCamera : MonoBehaviour
{
    Camera myCamera;
    [SerializeField] GameObject[] exculudedFromViewHeight = new GameObject[0];
    [SerializeField] GameObject[] exculudedFromViewWidth = new GameObject[0];
    [SerializeField] float padding = 20f;
    [SerializeField] Canvas mainCanvas = null;

    float screenWidth;
    float screenHeight;
    RectTransform canvasRect;

    private void Start()
    {
        myCamera = GetComponent<Camera>();
        canvasRect = mainCanvas.GetComponent<RectTransform>();

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
            float yOffset = Mathf.Abs(itemRect.anchoredPosition.y) - (itemRect.rect.height / 2);
            float calc = ((itemRect.rect.height + ((itemRect.position.y < canvasRect.rect.height / 2) ? yOffset : canvasRect.rect.height - yOffset)) / canvasRect.rect.height);
            Debug.Log(item.name + " pos: " + itemRect.position.y);
            Debug.Log(item.name + ": " + yOffset);
            camHeight = camHeight - (calc + (padding / canvasRect.rect.height));
            if (itemRect.position.y < canvasRect.rect.height / 2)
            {
                camY = camY + (calc + ((padding / canvasRect.rect.height) / 2));
            }
        }
        foreach (GameObject item in exculudedFromViewWidth)
        {
            RectTransform itemRect = item.GetComponent<RectTransform>();
            float xOffset = itemRect.anchoredPosition.x - (itemRect.rect.width / 2);
            float calc = ((itemRect.rect.width + ((itemRect.anchoredPosition.x < canvasRect.rect.width / 2) ? xOffset : canvasRect.rect.width - xOffset)) / canvasRect.rect.width);

            camWidth = camWidth - (calc + (padding / canvasRect.rect.width));
            if (itemRect.anchoredPosition.x < canvasRect.rect.width / 2)
            {
                camX = camX + (calc + ((padding / canvasRect.rect.width) / 2));
            }
        }

        myCamera.rect = new Rect(camX, camY, camWidth, camHeight);
    }

}
