using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickUI : MonoBehaviour, IPointerDownHandler,IPointerUpHandler
{
    public static bool clickPause = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        clickPause = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        clickPause = false;
    }

    // Start is called before the first frame update

}
