using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class VisibleScore : MonoBehaviour
{
    public Text Score;
    public Text HighScore;
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
        AudioManganger.Ins.PlaySound("GameOver");
        Score.text = CtrlData.Score.ToString();
        HighScore.text = CtrlData.Ins.GetHighScore().ToString();
    }
    
}
