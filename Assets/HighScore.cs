using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScore : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GetComponent<Animator>().SetBool("Open", false);
        }
    }
    private void GameOver()
    {
     
        if (CtrlData.CountPlay % 2 == 0)
        {
            GameManager.Ins.OpenWindow(TypeWindow.Continue);
        }
        else
        {
            GameManager.Ins.OpenWindow(TypeWindow.OverGame);
        }
     
    }
    private void OnEnable()
    {
        AudioManganger.Ins.PlaySound("HighScore");
    }

}
