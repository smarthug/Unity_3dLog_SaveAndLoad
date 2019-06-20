using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragObject : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    private Vector3 mOffset;
    private float mZCoord;

    private Vector3 GetMouseAsWorldPoint()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = mZCoord;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        mZCoord = Camera.main.WorldToScreenPoint(
            gameObject.transform.position).z;

        mOffset = gameObject.transform.position - GetMouseAsWorldPoint();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 result = GetMouseAsWorldPoint() + mOffset;
        transform.position = result;
    }

    public void OnEndDrag(PointerEventData eventData)
    {

    }
}