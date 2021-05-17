using UnityEngine;
using UnityEngine.EventSystems;

namespace trollschmiede.CivIdle.MapSys
{
    public class MapMover : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        Camera cam;
        Vector3 origPos;
        Vector3 deltaValue;
        Vector3 origPosRect;

        float zValue = 0f;

        void Start()
        {
            cam = Camera.main;
            zValue = cam.transform.position.z - transform.position.z;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            origPos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, zValue));
            origPosRect = transform.position;
            deltaValue = Vector3.zero;
            GetComponent<MainMap>().isDragging = true;
        }

        public void OnDrag(PointerEventData eventData)
        {
            deltaValue = origPos - cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, zValue));

            transform.position = origPosRect + deltaValue;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            deltaValue = Vector3.zero;
            GetComponent<MainMap>().isDragging = false;
        }
    }
}