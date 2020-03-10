using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class CtrlData : MonoBehaviour
{
    #region
    public static int Level;
    public static int Score;
    public static int CountPlay = 0;
    public static int CountGame = 0;
    public const string Key_HighScore = "Key_High_Score_Hight";
    public const string Key_Sound = "Key_Sound";
    public Transform UI;
    #endregion

    #region 
    public List<GameObject> EffectGame = new List<GameObject>();



    #endregion
    #region Cube

    public DataGame DataGame;
    public static int[,] Cube_Cross_1 = new int[4, 4]
              {
                    { 1, 0, 0, 0} ,
                    { 0, 0, 0, 0} ,
                    { 0, 0, 0, 0} ,
                    { 0, 0, 0, 0} ,
              };
    public static int[,] Cube_Cross_2 = new int[4, 4]
              {
                    { 1, 1, 0, 0} ,
                    { 0, 0, 0, 0} ,
                    { 0, 0, 0, 0} ,
                    { 0, 0, 0, 0} ,
              };

    public static int[,] Cube_Cross_3 = new int[4, 4]
              {
                    { 1, 1, 1, 0} ,
                    { 0, 0, 0, 0} ,
                    { 0, 0, 0, 0} ,
                    { 0, 0, 0, 0} ,
              };

    public static int[,] Cube_Cross_4 = new int[4, 4]
              {
                    { 1, 1, 1, 1} ,
                    { 0, 0, 0, 0} ,
                    { 0, 0, 0, 0} ,
                    { 0, 0, 0, 0} ,
              };

    public static int[,] Cube_3 = new int[4, 4]
              {
                    { 1, 0, 0, 0} ,
                    { 1, 1, 0, 0} ,
                    { 0, 0, 0, 0} ,
                    { 0, 0, 0, 0} ,
              };

    public static int[,] Cube_L4_0 = new int[4, 4]
              {
                    { 1, 1, 0, 0} ,
                    { 1, 0, 0, 0} ,
                    { 1, 0, 0, 0} ,
                    { 1, 0, 0, 0} ,
              };
    public static int[,] Cube_Quare = new int[4, 4]
             {
                    { 1, 1, 0, 0} ,
                    { 1, 1, 0, 0} ,
                    { 0, 0, 0, 0} ,
                    { 0, 0, 0, 0} ,
             };
    public static int[,] Cube_L4_90 = new int[4, 4]
            {
                    { 1, 1, 0, 0} ,
                    { 0, 1, 0, 0} ,
                    { 0, 1, 0, 0} ,
                    { 0, 1, 0, 0} ,
            };
    public static int[,] Cube_L3_0 = new int[4, 4]
            {
                    { 1, 1, 0, 0} ,
                    { 1, 0, 0, 0} ,
                    { 1, 0, 0, 0} ,
                    { 0, 0, 0, 0} ,
            };
    public static int[,] Cube_L3_90 = new int[4, 4]
          {
                    { 1, 1, 0, 0} ,
                    { 0, 1, 0, 0} ,
                    { 0, 1, 0, 0} ,
                    { 0, 0, 0, 0} ,
          };
    public static int[,] T = new int[4, 4]
         {
                    { 1, 1, 1, 0} ,
                    { 0, 1, 0, 0} ,
                    { 0, 0, 0, 0} ,
                    { 0, 0, 0, 0} ,
         };

    public const int NotRoll = 0;
    public const int Roll_Cross = 2;
    public const int Roll_Cube_L = 4;
    public static int[] RollCorss_1x1 = new int[5] { 0, 1, 2, 3, 4 };
    public static int[] RollCorss_1x2 = new int[5] { 0, 1, 2, 3, 4 };
    public static int[] RollCorss_2x1 = new int[5] { 0, 1, 2, 3, 4 };
    public static int[] RollCorss_1x3 = new int[4] { 0, 1, 3, 4 };
    public static int[] RollCorss_3x1 = new int[4] { 0, 1, 2, 4 };
    public static int[] RollCorss_1x4 = new int[3] { 0, 1, 3 };
    public static int[] RollCorss_4x1 = new int[1] { 2 };
    public static int[] L2_0 = new int[2] { 2, 4 };
    public static int[] L2_90 = new int[2] { 2, 3 };
    public static int[] L3_0 = new int[1] { 0 };
    public static int[] L3_90 = new int[1] { 1 };
    public static int[] L3_180 = new int[1] { 3 };
    public static int[] L3_270 = new int[1] { 4 };
    public static int[] L4_0 = new int[1] { 0 };
    public static int[] L4_90 = new int[1] { 1 };
    public static int[] L4_180 = new int[1] { 3 };
    public static int[] L4_270 = new int[1] { 4 };
    public static int[] Square = new int[5] { 0, 1, 2, 3, 4 };
    public static int[] T_0 = new int[1] { 2 };
    public static int[] T_90 = new int[1] { 3 };
    public static int[] T_180 = new int[1] { 4 };
    public static List<TypeShape> AllShape = new List<TypeShape>();

    #endregion
    public static CtrlData Ins;
    public List<Sprite> Img_Cube_3 = new List<Sprite>();
    public List<Sprite> Img_Cube_L_4 = new List<Sprite>();
    public List<Sprite> Img_Cube_Cross_2 = new List<Sprite>();
    public List<Sprite> Img_Cube_Cross_3 = new List<Sprite>();
    public List<Sprite> Img_Cube_Cross_4 = new List<Sprite>();
    public List<int> indexRoll = new List<int>();
    public List<TypeShape> Type_Ver_1;
    public List<TypeShape> Type_Ver_2;
    public List<TypeShape> Type_Ver_3;

    public static List<int[,]> ShapeType = new List<int[,]>();

    public Text TScore;
    public Text THighScore;

    public Dictionary<TypeShape, List<string>> InfoRollShape;

    public void GenerateShape()
    {
        AllShape.Add(TypeShape.crossBar_1);
        AllShape.Add(TypeShape.crossBar_2);
        AllShape.Add(TypeShape.crossBar_3);
        AllShape.Add(TypeShape.crossBar_4);
        AllShape.Add(TypeShape.three_cube);
        AllShape.Add(TypeShape.L3_0);
        AllShape.Add(TypeShape.L3_90);
        AllShape.Add(TypeShape.L_4_0);
        AllShape.Add(TypeShape.L4_90);
        AllShape.Add(TypeShape.square);
        AllShape.Add(TypeShape.T);
       
    }
    public void SpawnInit()
    {
        for (int i = 0; i < EffectGame.Count; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                var a = Poolers.Ins.GetObject(EffectGame[0]);
                if (a.GetComponent<DestroyParice>() != null)
                {
                    a.GetComponent<DestroyParice>().AddEff();
                }

            }
        }
        Poolers.Ins.ClearAll();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            View();
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            Test_1();
        }
    }
    public void ResetScore()
    {
        CtrlData.Level = 0;
        CtrlData.Score = 0;
        TScore.text = "0";
        CtrlData.CountPlay = 0;
    }

    public void InitKey()
    {
        InitMatrixRoll();

        Type_Ver_1.Add(TypeShape.crossBar_1);
        Type_Ver_1.Add(TypeShape.crossBar_2);
        Type_Ver_2.Add(TypeShape.crossBar_3);
        Type_Ver_2.Add(TypeShape.three_cube);


        Type_Ver_2.Add(TypeShape.square);
        Type_Ver_3.Add(TypeShape.crossBar_4);
        Type_Ver_3.Add(TypeShape.L4_90);
        Type_Ver_3.Add(TypeShape.L_4_0);
        Type_Ver_3.Add(TypeShape.T);

        //AllTypeShape.Add(TypeShape.crossBar_1);
        //AllTypeShape.Add(TypeShape.crossBar_2);
        //AllTypeShape.Add(TypeShape.crossBar_3);
        //AllTypeShape.Add(TypeShape.three_cube);
        //AllTypeShape.Add(TypeShape.T);
        //AllTypeShape.Add(TypeShape.square);
        //AllTypeShape.Add(TypeShape.crossBar_4);
        //AllTypeShape.Add(TypeShape.L4_90);
        //AllTypeShape.Add(TypeShape.L_4_0);
        //AllTypeShape.Add(TypeShape.L);

        //  PlayerPrefs.DeleteKey(Key_HighScore);
        if (!PlayerPrefs.HasKey(Key_HighScore))
        {
            PlayerPrefs.SetInt(Key_HighScore, 0);
            int Score = GetHighScore();

            THighScore.text = Score.ToString();
        }
        else
        {
            int Score = GetHighScore();

            THighScore.text = Score.ToString();

        }
        if (!PlayerPrefs.HasKey(Key_Sound))
        {


            PlayerPrefs.SetInt(Key_Sound, 1);
        }
    }
    public void SetTextScore(int Score)
    {
        TScore.text = Score.ToString();
    }
    public void SetHighScore(int Score)
    {
        THighScore.text = Score.ToString();
    }
    public void SetScore()
    {
        TScore.text = Score.ToString();
    }


    public bool Set_High_Score(int Score)
    {
        int HighScore = PlayerPrefs.GetInt(Key_HighScore);
        if (Score > HighScore)
        {
            PlayerPrefs.SetInt(Key_HighScore, Score);
            THighScore.text = GetHighScore().ToString();
            return true;
        }
        else
        {

            return false;
        }
    }
    public int GetHighScore()
    {
        return PlayerPrefs.GetInt(Key_HighScore);

    }


    // 1 On
    // 0 Off
    public void ChangeSound()
    {
        if (PlayerPrefs.GetInt(Key_Sound) == 1)
        {
            PlayerPrefs.SetInt(Key_Sound, 0);
        }
        else
        {
            PlayerPrefs.SetInt(Key_Sound, 1);
        }
    }
    public bool IsSound()
    {
        if (PlayerPrefs.GetInt(Key_Sound) == 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void VisibleScore(int Score)
    {
        var a = Poolers.Ins.GetObject(GetEffect(6), new Vector3(Screen.width / 2, Screen.height / 2, 0), Quaternion.identity);
        a.OnSpawn();
        a.GetComponent<Text>().text = Score.ToString();

    }
    public void VisibleCombo(int i)
    {
        var a = Poolers.Ins.GetObject(GetEffect(7), new Vector3(Screen.width / 2, Screen.height / 2, 0), Quaternion.identity);
        //   var a = Instantiate(GetEffect(7), new Vector3(Screen.width / 2, Screen.height / 2, 0), Quaternion.identity, UI);
        a.OnSpawn();
        a.transform.Find("Text").GetComponent<Text>().text = "c" + i;

    }


    public static TypeShape GetShapeType(int i)
    {

        TypeShape type = TypeShape.None;
        switch (i)
        {
            case 0:
                type = TypeShape.crossBar_1;
                break;
            case 1:
                type = TypeShape.crossBar_2;
                break;
            case 2:
                type = TypeShape.crossBar_3;
                break;
            case 3:
                type = TypeShape.crossBar_4;
                break;
            case 4:
                type = TypeShape.L_4_0;
                break;
            case 5:
                type = TypeShape.L4_90;

                break;
            case 6:
                type = TypeShape.square;
                break;


            case 7:
                type = TypeShape.three_cube;
                break;
            case 8:
                type = TypeShape.L3_0;
                break;
            case 9:
                type = TypeShape.L3_90;
                break;
            case 10:
                type = TypeShape.T;
                break;
            default:
                Debug.Log("Khong Co Type nhu the");
                break;
        }
        return type;
    }
    public static int[,] GetMatrixShapeType(TypeShape type)
    {
        int[,] matrix = null;

        switch (type)
        {
            case TypeShape.crossBar_1:
                matrix = CtrlData.Cube_Cross_1;
                break;
            case TypeShape.crossBar_2:
                matrix = CtrlData.Cube_Cross_2;
                break;
            case TypeShape.crossBar_3:
                matrix = CtrlData.Cube_Cross_3;
                break;
            case TypeShape.crossBar_4:
                matrix = CtrlData.Cube_Cross_4;
                break;
            case TypeShape.L_4_0:
                matrix = CtrlData.Cube_L4_0;
                break;
            case TypeShape.L4_90:
                matrix = CtrlData.Cube_L4_90;

                break;
            case TypeShape.square:
                matrix = CtrlData.Cube_Quare;
                break;


            case TypeShape.three_cube:
                matrix = CtrlData.Cube_3;
                break;
            case TypeShape.L3_0:
                matrix = CtrlData.Cube_L3_0;
                break;
            case TypeShape.L3_90:
                matrix = CtrlData.Cube_L3_90;
                break;
            case TypeShape.T:
                matrix = CtrlData.T;
                break;
            default:
                Debug.Log("Khong Co Type nhu the");
                break;
        }
        return matrix;
    }

    public static int CountRoll(TypeShape type)
    {
        int roll = 0;

        switch (type)
        {
            case TypeShape.crossBar_1:
                roll = 0;
                break;
            case TypeShape.crossBar_2:
                roll = 2;
                break;
            case TypeShape.crossBar_3:
                roll = 2;
                break;
            case TypeShape.crossBar_4:
                roll = 2;
                break;
            case TypeShape.L_4_0:
                roll = 4;
                break;
            case TypeShape.L4_90:
                roll = 4;

                break;
            case TypeShape.square:
                roll = 4;
                break;


            case TypeShape.three_cube:
                roll = 4;
                break;
            case TypeShape.L3_0:
                roll = 4;
                break;
            case TypeShape.L3_90:
                roll = 4;
                break;
            case TypeShape.T:
                roll = 4;
                break;
            default:
                Debug.Log("Khong Co Type nhu the");
                break;
        }
        return roll;
    }


    private void Awake()
    {
        if (Ins != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Ins = this;
        }
        ShapeType.Add(Cube_Cross_1); //0
        ShapeType.Add(Cube_Cross_2);//1
        ShapeType.Add(Cube_Cross_3);//2
        ShapeType.Add(Cube_Cross_4);//3
        ShapeType.Add(Cube_L4_0);//4
        ShapeType.Add(Cube_L4_90);//5
        ShapeType.Add(Cube_Quare);//6
        ShapeType.Add(Cube_3);//7
        ShapeType.Add(Cube_L3_0);//8
        ShapeType.Add(Cube_L3_90);//9
        GenerateShape();
        InitKey();

    }
    private void Start()
    {
        SpawnInit();
        CtrlGamePlay.Ins.Event_Start_Game += ResetScore;
    }
    // Start is called before the first frame update
    public static int RandomColor(TypeShape type, int roll)
    {
        int Color = 0;
        switch (type)
        {
            case TypeShape.crossBar_1:
                Color = RollCorss_1x1[Random.Range(0, RollCorss_1x1.Length)];
                break;
            case TypeShape.crossBar_2:
                switch (roll)
                {
                    case 0:
                        Color = RollCorss_1x2[Random.Range(0, RollCorss_1x2.Length)];
                        break;
                    case 1:
                        Color = RollCorss_2x1[Random.Range(0, RollCorss_2x1.Length)];
                        break;
                }

                Color = Random.Range(0, 5);
                break;
            case TypeShape.crossBar_3:
                switch (roll)
                {
                    case 0:
                        Color = RollCorss_1x3[Random.Range(0, RollCorss_1x3.Length)];
                        break;
                    case 1:
                        Color = RollCorss_3x1[Random.Range(0, RollCorss_3x1.Length)];
                        break;
                }


                break;
            case TypeShape.crossBar_4:
                switch (roll)
                {
                    case 0:
                        Color = RollCorss_1x4[Random.Range(0, RollCorss_1x4.Length)];
                        break;
                    case 1:
                        Color = RollCorss_4x1[Random.Range(0, RollCorss_4x1.Length)];
                        break;
                }

                break;

            case TypeShape.three_cube:

                switch (roll)
                {
                    case 0:
                        Color = L2_90[Random.Range(0, L2_90.Length)];
                        break;
                    case 1:
                        Color = L2_90[Random.Range(0, L2_90.Length)];
                        break;
                    case 2:
                        Color = L2_0[Random.Range(0, L2_0.Length)];
                        break;
                    case 3:
                        Color = L2_0[Random.Range(0, L2_0.Length)];
                        break;
                }
                break;
            case TypeShape.L3_0:
                switch (roll)
                {
                    case 0:
                        Color = L3_270[Random.Range(0, L3_270.Length)];
                        break;
                    case 1:
                        Color = L3_180[Random.Range(0, L3_180.Length)];
                        break;
                    case 2:
                        Color = L3_90[Random.Range(0, L3_90.Length)];
                        break;
                    case 3:
                        Color = L3_0[Random.Range(0, L3_0.Length)];
                        break;

                }

                break;
            case TypeShape.L3_90:
                switch (roll)
                {
                    case 0:
                        Color = 4;
                        break;
                    case 1:
                        Color = 0;
                        break;
                    case 2:
                        Color = L3_90[Random.Range(0, L3_90.Length)];
                        break;
                    case 3:
                        Color = 3;
                        break;

                }
                break;
            case TypeShape.L_4_0:
                switch (roll)
                {
                    case 0:
                        Color = 4;
                        break;
                    case 1:
                        Color = 3;
                        break;
                    case 2:
                        Color = 1;
                        break;
                    case 3:
                        Color = 0;
                        break;

                }
                break;
            case TypeShape.L4_90:
                switch (roll)
                {
                    case 0:
                        Color = 4;
                        break;
                    case 1:
                        Color = 0;
                        break;
                    case 2:
                        Color = 1;
                        break;
                    case 3:
                        Color = 3;
                        break;

                }
                break;


            case TypeShape.square:

                Color = Square[Random.Range(0, Square.Length)];
                break;
            case TypeShape.T:
                switch (roll)
                {
                    case 0:
                        Color = 4;
                        break;
                    case 1:
                        Color = 2;
                        break;
                    case 2:
                        Color = 3;
                        break;
                    case 3:
                        Color = 2;
                        break;

                }

                break;


        }
        return Color;

    }
    public GameObject GetEffect(int id)
    {
        for (int i = 0; i < EffectGame.Count; i++)
        {
            if (i == id)
            {
                return EffectGame[i];
            }
        }
        return null;
    }


    public void InitMatrixRoll()
    {
        InfoRollShape = new Dictionary<TypeShape, List<string>>();
        for (int i = 0; i < 11; i++)
        {
            InfoRollShape.Add(GetShapeType(i), new List<string>());
        }
        for (int i = 0; i < InfoRollShape.Count; i++)
        {
            switch (i)
            {
                case 0:
                    string ss = "1";
                    InfoRollShape[TypeShape.crossBar_1].Add(ss);
                    break;
                case 1:

                    for (int j = 0; j < 2; j++)
                    {

                        string s1 = CtrlGamePlay.Render(Shape.SplitMatrix(CtrlGamePlay.SimulateRoll(j, TypeShape.crossBar_2, false)));
                        InfoRollShape[TypeShape.crossBar_2].Add(s1);


                    }
                    break;
                case 2:

                    for (int j = 0; j < 2; j++)
                    {

                        string s2 = CtrlGamePlay.Render(Shape.SplitMatrix(CtrlGamePlay.SimulateRoll(j, TypeShape.crossBar_3, false)));
                        InfoRollShape[TypeShape.crossBar_3].Add(s2);


                    }


                    break;
                case 3:

                    for (int j = 0; j < 2; j++)
                    {

                        string s3 = CtrlGamePlay.Render(Shape.SplitMatrix(CtrlGamePlay.SimulateRoll(j, TypeShape.crossBar_4, false)));
                        InfoRollShape[TypeShape.crossBar_4].Add(s3);


                    }

                    break;
                case 4:
                    for (int j = 0; j < 4; j++)
                    {

                        string s4 = CtrlGamePlay.Render(Shape.SplitMatrix(CtrlGamePlay.SimulateRoll(j, TypeShape.three_cube, false)));
                        InfoRollShape[TypeShape.three_cube].Add(s4);


                    }

                    break;
                case 5:
                    for (int j = 0; j < 4; j++)
                    {

                        string s5 = CtrlGamePlay.Render(Shape.SplitMatrix(CtrlGamePlay.SimulateRoll(j, TypeShape.L3_0, false)));
                        InfoRollShape[TypeShape.L3_0].Add(s5);


                    }
                    break;
                case 6:
                    for (int j = 0; j < 4; j++)
                    {

                        string s6 = CtrlGamePlay.Render(Shape.SplitMatrix(CtrlGamePlay.SimulateRoll(j, TypeShape.L3_90, false)));
                        InfoRollShape[TypeShape.L3_90].Add(s6);


                    }

                    break;
                case 7:
                    for (int j = 0; j < 4; j++)
                    {

                        string s7 = CtrlGamePlay.Render(Shape.SplitMatrix(CtrlGamePlay.SimulateRoll(j, TypeShape.L4_90, false)));
                        InfoRollShape[TypeShape.L4_90].Add(s7);


                    }
                    break;
                case 8:
                    for (int j = 0; j < 4; j++)
                    {

                        string s8 = CtrlGamePlay.Render(Shape.SplitMatrix(CtrlGamePlay.SimulateRoll(j, TypeShape.L_4_0, false)));
                        InfoRollShape[TypeShape.L_4_0].Add(s8);


                    }
                    break;
                case 9:
                    for (int j = 0; j < 4; j++)
                    {

                        string s9 = CtrlGamePlay.Render(Shape.SplitMatrix(CtrlGamePlay.SimulateRoll(j, TypeShape.T, false)));
                        InfoRollShape[TypeShape.T].Add(s9);


                    }
                    break;

            }
        }

    }
    public void View()
    {
        string s = "";
        Debug.Log("ROLLLING : : ");
        for (int i = 0; i < 4; i++)
        {
            s += "\n" + "" + InfoRollShape[TypeShape.T][i];
        }
        Debug.Log(s);
    }
    public string GetSimulateRoll(TypeShape type, int roll)
    {
        return InfoRollShape[type][roll];
    }
    public static int[,] ConverStringToMatrix(string s)
    {
        Debug.Log("Matrix : " + s);

        StringReader strReader = new StringReader(s);
        
        int[,] matrix = null;
        List<List<int>> ListRow = new List<List<int>>();
        while (true)
        {
            string line = strReader.ReadLine();
            Debug.Log(line);
            if (line == "")
            {
                continue;
            }
            if (line != null)
            {
                List<int> Row = new List<int>();
                line.TrimEnd();
                line.TrimStart();
               
             //   Debug.Log("ROW ::: " + line);
                
                char[] space = {' '};
                
                string[] row = line.Split(space);

              


                for (int i = 0; i < row.Length; i++)
                {
                    if (row[i] == "")
                        continue;
               //     Debug.Log("Index :" + row[i]);
                    Row.Add(int.Parse(row[i]));
                }
                ListRow.Add(Row);




            }
            else
            {

                break;
            }
        }

        return matrix = CtrlGamePlay.Ins.ListToMatrix(ListRow);

    }
    public void Test_1()
    {
           Debug.Log(CtrlGamePlay.Render(ConverStringToMatrix(GetSimulateRoll(TypeShape.three_cube, 0))));  
        string phrase = "1 123 231 321 31 123 123 123 12";
        //string[] words = phrase.Split(' ');

        //foreach (var word in words)
        //{
        //    Debug.Log(word);
        //}
    }
}


