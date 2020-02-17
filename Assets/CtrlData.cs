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

    public static int[,] Cube_L4 = new int[4, 4]
              {
                    { 1, 1, 1, 1} ,
                    { 1, 0, 0, 0} ,
                    { 0, 0, 0, 0} ,
                    { 0, 0, 0, 0} ,
              };
    public static int[,] Cube_Quare = new int[4, 4]
             {
                    { 1, 1, 0, 0} ,
                    { 1, 1, 0, 0} ,
                    { 0, 0, 0, 0} ,
                    { 0, 0, 0, 0} ,
             };
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
                type = TypeShape.L_3;
                break;
            case 5:
                type = TypeShape.square;
                break;
            case 6:
                type = TypeShape.three_cube;
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
        ShapeType.Add(Cube_Cross_1);
        ShapeType.Add(Cube_Cross_2);
        ShapeType.Add(Cube_Cross_3);
        ShapeType.Add(Cube_Cross_4);
        ShapeType.Add(Cube_L4);
        ShapeType.Add(Cube_Quare);
        ShapeType.Add(Cube_3);
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
