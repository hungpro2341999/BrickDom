using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CtrlData : MonoBehaviour
{
    #region Cube
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
       

            case 5:
                type = TypeShape.L_4_0;

                break;
            case 6:
                type = TypeShape.L4_90;
                break;
            case 4:
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
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
