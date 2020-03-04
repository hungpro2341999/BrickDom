using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UiSound : MonoBehaviour,IPointerDownHandler
{
    public AudioSource Audio;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        if(CtrlData.Ins.IsSound())
       
        Audio.Play();
    }
}
