using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class AnimButton : MonoBehaviour, IPointerDownHandler,IPointerUpHandler
{
    RectTransform UI;
    float time;
    private void Awake()
    {
         UI = GetComponent<RectTransform>();
    }
    private void Start()
    {
        time = 0.2f;
      
    }
    public void OnPointerDown(PointerEventData pointerEventData)
    {
        UI =  GetComponent<RectTransform>();
        Debug.Log("Click");
        StartAnim(Vector3.one * 1.1f);
    }

    public void OnPointerUp(PointerEventData eventData)
    {

        UI = GetComponent<RectTransform>();
        StartAnim(Vector3.one);
    }
    public void StartAnim(Vector3 target)
    {
        UI.DOScale(target, time);
    }
}
