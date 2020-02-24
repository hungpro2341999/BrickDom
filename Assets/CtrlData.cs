using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CtrlData : MonoBehaviour
{
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
    
    public  const int NotRoll = 0;
    public  const int Roll_Cross = 2;
    public  const int Roll_Cube_L = 4;
    public static int[] RollCorss_1x1 = new int[5] { 0, 1, 2, 3, 4 };
    public static int[] RollCorss_1x2 = new int[5] { 0,1,2,3,4};
    public static int[] RollCorss_2x1 = new int[5] { 0, 1, 2, 3, 4 };
    public static int[] RollCorss_1x3 = new int[4] { 0,1,3,4 };
    public static int[] RollCorss_3x1 = new int[4] { 0, 1, 2, 4 };
    public static int[] RollCorss_1x4 = new int[3] { 0, 1, 3 };
    public static int[] RollCorss_4x1 = new int[1] {2};
    public static int[] L2_0 = new int[2] {2,4 };
    public static int[] L2_90 = new int[2] { 2, 3 };
    public static int[] L3_0 = new int[1] {0};
    public static int[] L3_90 = new int[1] {1};
    public static int[] L3_180 = new int[1] {3};
    public static int[] L3_270 = new int[1] {4};
    public static int[] L4_0 = new int[1] {0};
    public static int[] L4_90 = new int[1] {1};
    public static int[] L4_180 = new int[1] {3};
    public static int[] L4_270 = new int[1] {4};
    public static int[] Square = new int[5] {0,1,2,3,4};
    public static int[] T_0 = new int[1] { 2 };
    public static int[] T_90 = new int[1] { 3 };
    public static int[] T_180 = new int[1] { 4 };


    #endregion
    public static CtrlData Ins;
    public List<Sprite>  Img_Cube_3 = new List<Sprite>();
    public List<Sprite>  Img_Cube_L_4 = new List<Sprite>();
    public List<Sprite> Img_Cube_Cross_2 = new List<Sprite>();
    public List<Sprite> Img_Cube_Cross_3 = new List<Sprite>();
    public List<Sprite> Img_Cube_Cross_4 = new List<Sprite>();


    public static List<int[,]> ShapeType = new List<int[,]>();
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
            default:
                Debug.Log("Khong Co Type nhu the");
                break;
        }
        return type;
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
    }
    // Start is called before the first frame update
    public static int RandomColor(TypeShape type,int roll)
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
            
                Color =   Random.Range(0, 5);
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

        }
        return Color;

    }
    public GameObject GetEffect(string name)
    {
        for(int i = 0; i < EffectGame.Count; i++)
        {
            if(EffectGame[i].name == name)
            {
                return EffectGame[i];
            }
        }
        return null;
    }

}
