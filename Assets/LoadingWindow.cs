﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LoadingWindow : Windown
{
    public Image LoadingBar;
    public float time;
    public float Speed = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnEnable()
    {
        LoadingBar.fillAmount = 0;
        StartCoroutine(LoadingGame());
    }
    private void OnDisable()
    {
        LoadingBar.fillAmount = 0;
    }
    public IEnumerator LoadingGame()
    {
        while (LoadingBar.fillAmount!=1)
        {
            LoadingBar.fillAmount += Random.Range(0,0.05f);

            yield return new WaitForSeconds(Random.Range(0,1f));
        }
        GameManager.Ins.StartGame();




    }
}
