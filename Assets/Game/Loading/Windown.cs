﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum TypeWindow {GamePlay,OverGame,PauseGame,Loading,HighScore,Continue}
public class Windown : MonoBehaviour
{
    public TypeWindow TypeWindown; 
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
