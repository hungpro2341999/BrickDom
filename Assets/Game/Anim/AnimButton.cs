using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class AnimButton : MonoBehaviour, IPointerDownHandler,IPointerUpHandler
{
    RectTransform UI;
    float time;
    private void Start()
    {
        time = 0.5f;
        UI.GetComponent<RectTransform>();
    }
    public void OnPointerDown(PointerEventData pointerEventData)
    {
        Debug.Log("Click");
        StartAnim(Vector3.one * 1.2f);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        StartAnim(Vector3.one);
    }
    public void StartAnim(Vector3 target)
    {
        UI.DOScale(target, time);
    }
}
