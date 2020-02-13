﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public enum TypeShape {crossBar_1,crossBar_2,crossBar_3, crossBar_4, square,three_cube,L_3}
public class Shape : MonoBehaviour
{
    public int id;
    public TypeShape TypeShape;
    public int[,] shape;
    public int MaxLength;
    public GameObject PrebShape;
    public List<GameObject> ListShape = new List<GameObject>();
    public float offsetX=0.6f;
    public float offsetY=0.6f;
    public Vector2 point;
    bool initPos = false;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
         PushToBoard();
        if (Input.GetKeyDown(KeyCode.W))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
          //for(int i = 0; i < ListShape.Count; i++)
          //  {
          //      CtrlGamePlay.Ins.AddCubeIntoBoard(ListShape[i]);
          //      Debug.Log("Push");
          //  }
        }
    }
    public void isMove(int vertical)
    {
      
    }
  
    public void AddCubeToBoard()
    {
        init();
        initShape(TypeShape);
        for (int i = 0; i < ListShape.Count; i++)
        {
            CtrlGamePlay.Ins.AddCubeIntoBoard(ListShape[i]);
        }
    }

    #region InitShape



    public void SetNextId()
  {

  }
    public void init()
    {
        
        shape = new int[4, 4]
              {
                    { 0, 0, 0, 0} ,
                    { 0, 0, 0, 0} ,
                    { 0, 0, 0, 0} ,
                    { 0, 0, 0, 0} ,
              };
    }
  
    public void initShape(TypeShape type)
    {

        switch (type)
        {
            case TypeShape.crossBar_1:
                shape = new int[4, 4]
                {
                    { 1, 0, 0, 0} ,
                    { 0, 0, 0, 0} ,
                    { 0, 0, 0, 0} ,
                    { 0, 0, 0, 0} ,
                };
                RotationMaxtrix(Random.Range(0, 7));
                break;
            case TypeShape.crossBar_2:
                shape = new int[4, 4]
                {
                    { 1, 1, 0, 0} ,
                    { 0, 0, 0, 0} ,
                    { 0, 0, 0, 0} ,
                    { 0, 0, 0, 0} ,
                };
                RotationMaxtrix(Random.Range(0, 7));
                break;
            case TypeShape.crossBar_3:
                shape = new int[4, 4]
              {
                    { 1, 1, 1, 0} ,
                    { 0, 0, 0, 0} ,
                    { 0, 0, 0, 0} ,
                    { 0, 0, 0, 0} ,
              };
                RotationMaxtrix(Random.Range(0, 7));

                break;
            case TypeShape.crossBar_4:
                shape = new int[4, 4]
              {
                    { 1, 1, 1, 1} ,
                    { 0, 0, 0, 0} ,
                    { 0, 0, 0, 0} ,
                    { 0, 0, 0, 0} ,
              };
                RotationMaxtrix(Random.Range(0, 7));
                SplitMatrix(shape);
                Render(shape);
                
                break;
            case TypeShape.square:
                shape = new int[4, 4]
            {
                    { 1, 1, 0, 0} ,
                    { 1, 1, 0, 0} ,
                    { 0, 0, 0, 0} ,
                    { 0, 0, 0, 0} ,
            };
                RotationMaxtrix(Random.Range(0, 7));
                SplitMatrix(shape);
                break;
            case TypeShape.three_cube:
                shape = new int[4, 4]
            {
               
                    { 1, 1, 0, 0} ,
                    { 1, 0, 0, 0} ,
                    { 0, 0, 0, 0} ,
                    { 0, 0, 0, 0} ,
            };
                RotationMaxtrix(Random.Range(0,7));
                
                SplitMatrix(shape);
                break;
            case TypeShape.L_3:
                shape = new int[4, 4]
            {

                    { 1, 1, 0, 0} ,
                    { 1, 0, 0, 0} ,
                    { 1, 0, 0, 0} ,
                    { 1, 0, 0, 0} ,
            };
                RotationMaxtrix(Random.Range(0, 7));

                SplitMatrix(shape);
                break;
        }
        SetShape();
        
       
    }
    public int[,] SplitMatrix(int[,] matrix)
    {
        Render(matrix);
        int x = -1;
        int y = -1;
        int xMatrixSlip = 100;
        int yMatrixSlip = 100;
        int xSlip = 0;
        int ySlip = 0;
        // Check Width
        int width=0;
        int height=0;
        string s = "";
        for (int j=0;j<matrix.GetLength(1); j++)
        {
            s += "\n";
          
           
         
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                s += matrix[i,j];
                if (matrix[i, j] !=0)
                {
                   xMatrixSlip =  Mathf.Min(xMatrixSlip, j);
               //     Debug.Log("HANG Min" + j + " :" + xMatrixSlip);

                    width++;
                  
              
                }

            }
            xSlip = Mathf.Max(xSlip, width);
            width = 0;
           // Debug.Log("HANG " + "[" + j + "," + "]" + " :: " + width);
        }
     
        //   Debug.Log("Check Width: "+xSlip );

        // Check Height
        for (int j = 0; j < matrix.GetLength(0); j++)
        {
            for (int i = 0; i < matrix.GetLength(1); i++)
            {
                if (matrix[j, i] !=0)
                {
                     height++;
                  
                    yMatrixSlip = Mathf.Min(yMatrixSlip, j);
               //     Debug.Log("Cot Min" + j + " :" + yMatrixSlip);
                }
            }
            ySlip = Mathf.Max(ySlip, height);
            height = 0;
         //   Debug.Log("COT " + j + " :" + height);
        }
    //    Debug.Log("Check Height: " + ySlip);

        int[,] MatrixSlip = new int[xSlip,ySlip];
     //   Debug.Log(xMatrixSlip + "  Size   " + yMatrixSlip);
      //  Debug.Log(xSlip + "   " + ySlip);
        //Get Slip Matrix
        string ss = "";
        ss += "Colum : " + xMatrixSlip + "  " + "WIDTH :" + yMatrixSlip +"\n"; 
        for (int j = xMatrixSlip; j < ySlip+xMatrixSlip; j++)
        {
            y++;
            x = 0;
            ss += "\n";
            for (int i = yMatrixSlip; i < xSlip + yMatrixSlip; i++)
            {
                int a = matrix[i, j];
                MatrixSlip[x, y] = a;
                    x++;
                ss += "  a[" + i + "," + j + "]  : " + matrix[i, j];
              //  Debug.Log("" + i + "  " + j + "  "+ matrix[i, j]);
               
            }
        }
        Debug.Log(ss);
        Render(MatrixSlip);
        return MatrixSlip;
                        
            
    }
    public int[,] RotationMaxtrix(int countRoll)
    {
      
        int[,] cloneMatrix = Clone(shape);
     
        int count = 4;
        for(int x = 0; x <countRoll; x++)
        {
            count = 4;
            for (int i = 0; i < 4; i++)
            {
                count--;
                for (int j = 0; j < 4; j++)
                {

                    shape[i, j] = cloneMatrix[j,count];
                }
            }
            cloneMatrix = Clone(shape);
           

        }
        return cloneMatrix;
        
       
    }
    public static int[,] Clone(int[,] matrix)
    {
        int[,] matrixs = new int[matrix.GetLength(0), matrix.GetLength(1)];
        for(int i = 0; i < matrix.GetLength(1); i++)
        {
          
            for(int j = 0; j < matrix.GetLength(0); j++)
            {
                int x = matrix[j, i];
                matrixs[j, i] = x;
            }
        }
        return matrixs;
        
    }
    public void Render(int[,] matrix)
    {
        string s = "";
        for(int i = 0; i < matrix.GetLength(1); i++)
        {
            s+="\n";
            for (int j = 0; j <matrix.GetLength(0); j++)
            {
               s+= matrix[j, i] + "  ";    
            }
        }
        Debug.Log(s);
    }
    private void SetShape()
    {
        shape = SplitMatrix(shape);
     //   Debug.Log("SET ");
        Render(shape);

        int i = 0;
        for(int y = 0; y < shape.GetLength(1); y++)
        {
            
            for(int x = 0; x < shape.GetLength(0); x++)
            {
                if (shape[x,y] != 0)
                {
                  
                   var a =  Instantiate(PrebShape, transform);
                    a.transform.localPosition = new Vector3(y * offsetX,- x * offsetY);
                    i++;
                    a.name = "Shape " + i;
                    ListShape.Add(a);
                }
               
            }
        }
        
    }
    #endregion

    #region PushToBoard


    public Vector2[] PushToBoard()
    {
        List<Vector2> ListPoint = new List<Vector2>();
        for(int i = 0; i < ListShape.Count; i++)
        {
            int x = Mathf.Abs(Mathf.RoundToInt((ListShape[i].transform.position.x - CtrlGamePlay.Ins.initPoint.x)/CtrlGamePlay.Ins.offsetX));
            int y = Mathf.Abs(Mathf.RoundToInt((ListShape[i].transform.position.y - CtrlGamePlay.Ins.initPoint.y)/CtrlGamePlay.Ins.offsetY));
            Vector2 point = new Vector2(x, y);
         //   ListPoint.Add(point);
            ListShape[i].GetComponent<DestroySelf>().Point = point;
        }
           return ListPoint.ToArray();
    }
    public void RemoveCube(GameObject game)
    {
        CtrlGamePlay.Ins.Cubes.Remove(game);
        ListShape.Remove(game);
    }
    public bool Check_Destroy()
    {
        if (ListShape.Count == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void Destroy()
    {
        Destroy(gameObject);
    }
 

    #endregion
}
