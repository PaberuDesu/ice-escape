using UnityEngine;
using UnityEngine.EventSystems;

public class DragController : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    public static int x, y;

    public void OnBeginDrag(PointerEventData eventData) {
        if (Mathf.Abs(eventData.delta.x) > Mathf.Abs(eventData.delta.y)) {
            y = 0;
            if (eventData.delta.x > 0) {
                x = 1;
                return;
            }
            x = -1;
            return;
        }
        else {
            x = 0;
            if (eventData.delta.y > 0) {
                y = 1;
                return;
            }
            y = -1;
            return;
        }
    }

    public void OnDrag(PointerEventData eventData) {}
}