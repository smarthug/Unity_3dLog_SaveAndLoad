using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragUI : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    private float m_PosX;
    private float m_PosY;

    public CanvasScaler scaler;

    private Vector3 m_PositionCorrection;

    public void OnBeginDrag(PointerEventData eventData)
    {
        // UI의 피벗 위치와 사용자가 드래그를 시작한 위치간 보정을 통해 UI가 순간이동 하는 일이 없도록 함.
        m_PositionCorrection = new Vector2(
            eventData.pressPosition.x - GetComponent<RectTransform>().position.x,
            eventData.pressPosition.y - GetComponent<RectTransform>().position.y
            );
    }

    public void OnDrag(PointerEventData eventData)
    {
        // 사용자의 해상도에 따른 UI 드래그 위치를 제한하는 최대/최소 경계 생성
        float _maxX = Screen.width - (GetComponent<RectTransform>().rect.width * scaler.scaleFactor);
        float _minY = GetComponent<RectTransform>().rect.height * scaler.scaleFactor;

        // 위의 최대/최소 경계선 계산된 값과 m_PositionCorrection 을 더하여, 
        // 화면밖으로 나가지는 않으면서, 자연스럽게 조작할 수 있도록 해줌.
        m_PosX = Mathf.Clamp(eventData.position.x - m_PositionCorrection.x, 0f, _maxX);
        m_PosY = Mathf.Clamp(eventData.position.y - m_PositionCorrection.y, _minY, Screen.height);

        transform.position = new Vector2(m_PosX, m_PosY);
    }

    public void OnEndDrag(PointerEventData eventData)
    {

    }
}
