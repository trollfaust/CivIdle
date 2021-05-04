using UnityEngine;
using UnityEngine.EventSystems;

public class MapMover : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] RectTransform parentRect = null;

    RectTransform rect;

    void Start()
    {
        rect = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        
    }

    //TODO: better calcs & change to map scroll not image
    public void OnDrag(PointerEventData eventData)
    {
        Vector3 offset = (Vector3)eventData.delta;

        offset.x = (offset.x / rect.rect.width) * parentRect.rect.width * 4f;
        offset.y = (offset.y / rect.rect.height) * parentRect.rect.height * 5f;

        transform.position = transform.position + offset;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        
    }
}
