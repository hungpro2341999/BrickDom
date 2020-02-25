using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeScreen {LoadingScreen,PlayScreen}
public class Screen : MonoBehaviour
{

    public TypeScreen Type;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Open()
    {
        gameObject.SetActive(true);
    }
    public void Close()
    {
        gameObject.SetActive(false);
    }


}
