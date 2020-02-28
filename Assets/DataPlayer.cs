using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataPlayer : MonoBehaviour
{
    public static DataPlayer Ins;
    public static int Score;
    public static int HighScore;
    public int Combo = 14;
    public Text T_Score;
    public Text T_HighScore;
    public static List<TypeShape> Type_1 = new List<TypeShape>();
    public static List<TypeShape> Type_2 = new List<TypeShape>();
    public static List<TypeShape> Type_3 = new List<TypeShape>();
    // Start is called before the first frame update
    void Start()
    {
        CtrlGamePlay.Ins.Event_Start_Game += ResetGame;
        if (Ins != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Ins = this;
        }
      

        T_HighScore.text = CtrlData.Ins.GetHighScore().ToString();
        initLevel();
    }
    public void initLevel()
    {
        // type 1
        Type_1.Add(TypeShape.crossBar_1);
        Type_1.Add(TypeShape.crossBar_2);
        Type_1.Add(TypeShape.crossBar_3);
        Type_1.Add(TypeShape.three_cube);
      
        // type 2
        Type_2.Add(TypeShape.square);

        // type 3
        Type_3.Add(TypeShape.crossBar_4);
        Type_3.Add(TypeShape.L4_90);
        Type_3.Add(TypeShape.L_4_0);
    }
    public TypeShape RandomType(List<TypeShape> type)
    {
        return type[Random.Range(0, type.Count)];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetScore()
    {
        T_Score.text = Score.ToString();
        
    }
    public void SetHighScore(int Score)
    {
        T_HighScore.text = Score.ToString();

    }
    public void ResetGame()
    {
        Score = 0;
        T_Score.text = "0";
    }

}
