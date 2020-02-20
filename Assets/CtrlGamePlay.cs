﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CtrlGamePlay : MonoBehaviour
{
  
    public static int idShape = 0;
    public static CtrlGamePlay Ins;
    public int Row;
    public int Column;
    public int[,] Board;
    public int[,] BackUpBoard;
    public Vector2 initPoint;
    public float offsetX;
    public float offsetY;
    public GameObject PrebShape;
    public List<GameObject> Cubes = new List<GameObject>();
    public List<Shape> List_Shape = new List<Shape>();
  
    public Vector2 Destroy;
    public LayerMask LayerShape;
    public Shape ShapeClick = null;
    public List<Vector2> neighbor = new List<Vector2>();
    public float ClampY;
    public bool IsRandom = false;
    #region localVariable
    bool initPos = false;
    public Vector2 PosInit;
    public Vector2 PosCurr;

    #endregion
   
    #region Mouse
    public Vector2 PosMouseInit;
    public Vector2 PosMouseCurr;
    public bool isClick_down = false;
    public bool isClick_up = false;
    #endregion
    #region Delegate
    public delegate void ClickDownHander();
    public delegate void ClickUpHander();
    public delegate void MoveDownComplete();
    public delegate void StartGame();
    public delegate void CompleteChangeCube();
    #endregion
    #region Action
    public event ClickDownHander Event_Click_Down;
    public event ClickUpHander Event_Click_Up;
    public event MoveDownComplete Event_Completed_Move_Down;
    public event StartGame Event_Start_Game;
    public event MoveDownComplete Event_Completed_Change;
    #endregion

    // Simulate 
    public Text Matrix;
    public Text Select;
    public Text ShapeSelect;
    public Text MoveSelect;
    public Text Error;


    //Test
    public int xx;
    public int yy;
    // Start is called before the first frame update
     
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
        Init_Event();
        Vector3 pos = transform.position;
        ClampY = PosInit.y - Row * offsetY;
    }
    public void Init_Event()
    {
        Event_Click_Down += Init_Mouse_Down;
        Event_Click_Up += SetUpMoveDown;
        Event_Completed_Move_Down += SpawnShape;
        Event_Start_Game += SpawnShape;
        Event_Start_Game += UnRigidBody;
        Event_Completed_Change += SetUpMoveDown;
   


    }

    void Start()
    {
      //  Event_Completed_Move_Down();
        Board = new int[Row,Column];
        Event_Start_Game();
        ///

        ActiveRigidBoy(false);
        int x = Random.Range(0, Column);
        var a = Instantiate(PrebShape, null);
        a.name = "Shape : " + CtrlGamePlay.idShape;
        idShape++;
        a.GetComponent<Rigidbody2D>().mass -= idShape * 0.2f;
        List_Shape.Add(a.GetComponent<Shape>());
        a.GetComponent<Shape>().AddCubeToBoard();
    }

    // Update is called once per frame
    void Update()
    {

       // RefershBoard();
        if (Input.GetKeyDown(KeyCode.E))
        {
            SpawnShape();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            RefershBoard();
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            DestroyRow(13);
            SplitShape(List_Shape[0]);
        }
           
        if (ShapeClick != null)
        {

        }
        Matrix.text = Render((Board));

        if (Input.GetKeyDown(KeyCode.L))
        {
            int[,] matrix = new int[4, 4]
            {
                    { 0, 0, 0, 1} ,
                    { 0, 0, 0, 1} ,
                    { 0, 0, 0, 0} ,
                    { 0, 0, 0, 0} 
                    
            };
             Debug.Log(" Matrix Remove ");
             Debug.Log(Render(RemoveRow(matrix)));


        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log(Render(List_Shape[0].shape));
        }

        if (Input.GetMouseButtonDown(0))
        {

            Event_Click_Down();
            isClick_down = true;



        }
        if (Input.GetMouseButtonUp(0))
        {


            Event_Click_Up();
            isClick_down = false;
            isClick_up = true;
            ActiveRigidBoy(true);
            SetUpAll();
            return;

        }
        // Button_Up_And_Complete_Move_Down
        if (isClick_up)
        {
            RefershBoard();
            if (IsListShapeMove())
            {
                RefershBoard();
                Event_Completed_Move_Down();
                Debug.Log("Complete_Move");
               // ActiveRigidBoy(false);
                
                 CheckDestroyRow();
              // Reset Staus
            
                initPos = false;
             
                // Destroy Row
                isClick_up = false;
            }

        }
        
        if (isClick_down)
        {
            int Direct = UpdatePoint(Input.mousePosition);
            float dis = 0;
           // RefershBoard();
            if (ShapeClick != null)
            {
                ActiveRigidBoy(false);
                ShapeClick.PushToBoard();
                Event_Completed_Change();
              
                dis = PosMouseInit.x - Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
                ShapeClick.MoveTo(dis);
                //  Shape.transform.position = ScreenToWord(Input.mousePosition);
                Debug.Log("Move");
            }  
            else
            {

                ShapeSelect.text = "NULL";
            }
            Select.text += "\n Direct :" + Direct;
          
        }
        else
        {
          
            if (ShapeClick != null)
            {
               
                ShapeClick.Snap();
                ShapeClick.ResetStatus();
                ShapeClick = null;
              //  Event_Completed_Change();
           
                

            }
          
                ActiveRigidBoy(true);
           
               

            // CheckDestroyRow();
            //// Reset Staus
            //ActiveShape(true);
            //initPos = false;
            //SplitShape();
            //// Destroy Row



        }
    }
    public void SetUpAll()
    {
        for (int i = 0; i < List_Shape.Count; i++)
        {
            List_Shape[i].gameObject.layer = 8;
            for (int j = 0; j < List_Shape[i].ListShape.Count; j++)
            {
                List_Shape[i].ListShape[j].gameObject.layer = 8;
            }
          
        }
    }
    public void SetUpMoveDown()
    {
       // ActiveRigidBoy(true);
        // RefershBoard();
        //Debug.Log("SetUpMoveDown");
        //for(int i = 0; i < List_Shape.Count; i++)
        //{
        //    int x = 0;
        //        ClampShape_Down(List_Shape[i]);
          
          
        //}
    }
    public void ActiveRigidBoy(bool active)
    {
       
        for(int i=0 ; i < List_Shape.Count; i++)
        {
           if(List_Shape[i].gameObject.layer == 8)
            {
                List_Shape[i].Body.isKinematic = !active;
            }
           
        }
    }

    public void UnRigidBody()
    {
        ActiveRigidBoy(false);
    }
    public bool IsListShapeMove()
    {
        for (int i = 0; i < List_Shape.Count; i++)
        {
            if (isDowmShape(List_Shape[i])!=0)
            {
                return false;
            }
        }
        return true;
    }
    public void SplitShape()
    {
        for(int i = 0; i < List_Shape.Count; i++)
        {
            SplitShape(List_Shape[i]);
        }
    }
   
     
    public void Init_Mouse_Down()
    {
        initPos = true;
        Vector3 posInit = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        BackUpBoard = Board;
        PosInit = PointToMatrix(posInit);
        PosMouseInit = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        ShapeClick = isShapeClick();
        if (ShapeClick != null)
        {
             ClampShape(ShapeClick);

            ShapeClick.InitPoint();

        }
    }
    public void CheckDestroyRow()
    {
        int[] row;
      
        if (DestroyAtRow(out row))
        {

            for (int i = 0; i < row.Length; i++)
            {
                Debug.Log("DESTROY ROW :" + i);
                DestroyRow(row[i]);
            }
           

        }
        else
        {
            Debug.Log(" NOTTHING ");
        }
    }

    public static Vector3 ScreenToWord(Vector2 posScreen)
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;
        return pos;
    }
    public Vector2 PointToMatrix(Vector2 Pos)
    {
        int x = Mathf.FloorToInt((Pos.x - CtrlGamePlay.Ins.initPoint.x) / CtrlGamePlay.Ins.offsetX);
        int y = Mathf.FloorToInt((CtrlGamePlay.Ins.initPoint.y - Pos.y) / CtrlGamePlay.Ins.offsetY);
        //  Debug.Log(x + "  " + y);
        if (x < 0 || x >= Column || y < 0 || y > Row)
        {

            return -Vector2.one;

        }
        else
        {
            Vector2 point = new Vector2(x, y);
            return point;
        }


    }
    public int UpdatePoint(Vector3 point)
    {
        int vertical = 0;
        PosCurr = PointToMatrix(Camera.main.ScreenToWorldPoint(point));
        Vector3 pos = Camera.main.ScreenToWorldPoint(point);
        // Debug.Log(PosCurr +" ::: " + CheckPointCorrect(pos.x,pos.y));

        Select.text = PosInit + "::" + PosCurr + " ::: " + CheckPointCorrect(pos.x, pos.y);

        if ((PosCurr.x - PosInit.x) == 0)
        {
            return 0;
        }
        return (int)Mathf.Sign(PosCurr.x - PosInit.x);

    }
    public bool isMove(Shape shape)
    {
        shape.PushToBoard();

        for (int x = 0; x < shape.ListShape.Count; x++)
        {


        }
        return false;
    }

    public Shape isShapeClick()
    {
        RaycastHit2D rayHit;
        rayHit = Physics2D.GetRayIntersection(Camera.main.ScreenPointToRay(Input.mousePosition));
        if (rayHit.collider != null)
        {
            if (rayHit.collider.gameObject.layer == 8)
            {

                Shape shape = rayHit.collider.gameObject.GetComponent<DestroySelf>().shape;
                ShapeSelect.text = shape.gameObject.name;
                return shape;

            }



        }
        else
        {
            return null;
        }

        return null;




    }

    public void SpawnShape()
    {
        //ActiveRigidBoy(false);
        //int x = Random.Range(0, Column);
        //var a = Instantiate(PrebShape, null);
        //a.name = "Shape : " + CtrlGamePlay.idShape;
        //idShape++;
        //a.GetComponent<Rigidbody2D>().mass -= idShape * 0.2f;
        //List_Shape.Add(a.GetComponent<Shape>());
        //a.GetComponent<Shape>().AddCubeToBoard();

    }

    public static Vector3 RandomPosShape()
    {
        int x = Random.Range(0, CtrlGamePlay.Ins.Column);
        return new Vector3(CtrlGamePlay.Ins.initPoint.x + x * CtrlGamePlay.Ins.offsetX, CtrlGamePlay.Ins.initPoint.y);
    }

    public void AddCubeIntoBoard(GameObject cube)
    {
        if (!Cubes.Contains(cube))
        {
            Cubes.Add(cube);
        }


    }

    private void PushToBoard(GameObject cube)
    {
        if (cube.GetComponent<DestroySelf>() == null)
            return;
        Vector2 point = cube.GetComponent<DestroySelf>().Point;
        try
        {
            Board[(int)point.y, (int)point.x] = 1;
        }
        catch
        {
            Error.text = " ERROR : " + point.x + ":" + point.y;
        }

    }
    public bool isCubeCorrect(GameObject cube)
    {
        Vector2 point = cube.GetComponent<DestroySelf>().Point;
        for (int i = 0; i < Column; i++)
        {
            for (int j = 0; j < Row; j++)
            {
                if (j == point.x && i == point.y && Board[j, i] == 1)
                {
                    return false;
                }
                else
                {
                    return true;
                }

            }
        }
        return false;
    }

    public bool IsCubeCorrect(int x, int y)
    {
        if (!isInMatrix(x, y))
        {
            return false;
        }
        string s = "";
        for (int j = 0; j < Row; j++)
        {
            s += "\n";
            for (int i = 0; i < Column; i++)
            {


                s += Board[j, i] + "  ";

                if (j == x && i == y && Board[j, i] == 1)
                {
                    Debug.Log(i + " " + j);

                    return false;
                }


            }
        }
        Debug.Log(s);

        return true;

    }
    public bool isShapeCorrect(Shape shape, int x, int y)
    {
        for (int i = 0; i < shape.ListShape.Count; i++)
        {
            Vector2 point = shape.ListShape[i].GetComponent<DestroySelf>().Point;
            if (!IsCubeCorrect((int)point.x, (int)point.y))
            {

                return false;
            }
        }
        return true;

    }
    public bool isCorrectWithBoard(Shape shape)
    {
        bool correct = false;


        return correct;
    }
    private bool IsCubeCorrect(GameObject Cube)
    {

        bool correct = false;

        //        Cube.GetComponent<DestroySelf>().

        return correct;
    }



    public void DestroyRow(int row)
    {

        Shape shape;
        for (int i = 0; i < Column ; i++)
        {
            for (int x = 0; x < Cubes.Count; x++)
            {

                if (Cubes[x].GetComponent<DestroySelf>().Point == new Vector2((float)i,row))
                {
                    shape = Cubes[x].GetComponent<DestroySelf>().shape;
                    Cubes[x].GetComponent<DestroySelf>().Destroy();
                  
                       Debug.Log(shape.name);


                }
            }
        }

        RefershBoard();


    }

    private string Render(int[,] matrix)
    {
        string s = "";
        for (int j = 0; j < matrix.GetLength(0); j++)
        {
            s += "\n";
            for (int i = 0; i < matrix.GetLength(1); i++)
            {
                s += matrix[j, i] + "  ";
            }
        }
        // Debug.Log(s);
        return s;
    }

    #region ReflectShape

    public void RefershBoard()
    {
        ReflectShape();
        Board = new int[Row,Column];
        for (int j = 0; j < List_Shape.Count; j++)
        {

            for (int i = 0; i < List_Shape[j].ListShape.Count; i++)
            {
                PushToBoard(List_Shape[j].ListShape[i]);
            }
        }

    }

    public void ReflectShape()
    {
        for (int i = 0; i < List_Shape.Count; i++)
        {

            int count = 0;

            count = List_Shape[i].ListShape.Count;
            //   Debug.Log("List_Shape : " + count);
            if (count == 0)
            {
                //      Debug.Log("Destroy_Shape : " + count);
                //   Debug.Log("Remove");

                List_Shape[i].CheckDestroy();
            }
        }
    }
    #endregion


    public static bool CheckPointCorrect(float PosX, float PosY)
    {

        Vector2 point = PositonToMatrix(PosX, PosY);
        if (point.x < 0 || point.x > CtrlGamePlay.Ins.Column || point.y < 0 || point.y > CtrlGamePlay.Ins.Row)
        {
            return false;
        }
        else
        {
            return true;
        }

    }
    public static bool isInMatrix(int x, int y)
    {
        if (x < 0 || x >= CtrlGamePlay.Ins.Column || y < 0 || y >= CtrlGamePlay.Ins.Row)
        {
            return false;
        }
        return true;
    }
    public static Vector2 PositonToMatrix(float PosX, float PosY)
    {
        int x = Mathf.FloorToInt((PosX - CtrlGamePlay.Ins.initPoint.x) / CtrlGamePlay.Ins.offsetX);
        int y = Mathf.FloorToInt((CtrlGamePlay.Ins.initPoint.y - PosY) / CtrlGamePlay.Ins.offsetY);
        return new Vector2(x, y);

    }
    public static Vector2 PositonToMatrixRound(float PosX, float PosY)
    {

        int x = Mathf.RoundToInt((PosX - CtrlGamePlay.Ins.initPoint.x) / CtrlGamePlay.Ins.offsetX);
        int y = Mathf.RoundToInt((CtrlGamePlay.Ins.initPoint.y - PosY) / CtrlGamePlay.Ins.offsetY);
        return new Vector2(x, y);

    }
    public static Vector2 MatrixToPoint(int x, int y)
    {
        Vector2 pos = Vector2.zero;
        pos.x = CtrlGamePlay.Ins.PosInit.x + x * CtrlGamePlay.Ins.offsetX;
        pos.y = CtrlGamePlay.Ins.PosInit.y - y * CtrlGamePlay.Ins.offsetY;
        return pos;
    }
   
    public bool DestroyAtRow(out int[] row)
    {
        List<int> Row = new List<int>();
        bool isDestroy = false;
        for (int i = 0; i < this.Row; i++)
        {
            int count = this.Column;
            for (int j = 0; j < Column; j++)
            {

                if (Board[i, j] == 1)
                {
                    count--;
                }
                if (count == 0)
                {
                    isDestroy = true;
                    Row.Add(i);
                }
            }
        }
        row = Row.ToArray();
        return isDestroy;
    }

    public void ClampShape(Shape shape)
    {


        int minX = 0;
        int maxX = 0;
        List<Vector2> Point = new List<Vector2>();
        //for(int i = 0; i < shape.ListShape.Count; i++)
        //{
        //    Point.Add(shape.ListShape[i].GetComponent<DestroySelf>().Point);

        //}

        //Move_left

        bool isMove = true;
        string s = "";
        s += "MOVE LEFT : \n";
        while (isMove)
        {
            minX++;

            for (int i = 0; i < shape.ListShape.Count; i++)
            {
                int x = (int)shape.ListShape[i].GetComponent<DestroySelf>().Point.x - minX;
                int y = (int)shape.ListShape[i].GetComponent<DestroySelf>().Point.y;

                s += "apply x : " + x + "y :" + y + "\n";
                if (!IsCanMove(shape.ListShape[i].GetComponent<DestroySelf>(), x, y))
                {
                    isMove = false;
                    minX--;
                    minX = Mathf.Clamp(minX, 0, Column);
                    s += " reject x : " + x + "y :" + y + "\n";
                    break;
                }

            }


        }

        //Move_Right
        isMove = true;
        s += "MOVE RIGHT : \n";
        while (isMove)
        {
            maxX++;
            for (int i = 0; i < shape.ListShape.Count; i++)
            {
                int x = (int)shape.ListShape[i].GetComponent<DestroySelf>().Point.x + maxX;
                int y = (int)shape.ListShape[i].GetComponent<DestroySelf>().Point.y;
                s += " apply x : " + x + "y :" + y + "\n";
                if (!IsCanMove(shape.ListShape[i].GetComponent<DestroySelf>(), x, y))
                {

                    isMove = false;
                    maxX--;
                    maxX = Mathf.Clamp(maxX, 0, Column);
                    s += " reject  x : " + x + "y :" + y + "\n";

                    break;
                }
            }

        }

        MoveSelect.text = "LEFT : " + minX + "RIGHT :  " + maxX + "\n";
        float ClampMinX = minX * offsetX;
        float ClampMaxX = maxX * offsetY;

        MoveSelect.text += ClampMinX + " :: " + ClampMaxX;
        shape.SetUpClamp(ClampMinX, ClampMaxX);

        MoveSelect.text = s;
        Debug.Log(s);


    }
    
    public bool IsCanMove(DestroySelf Cube, int x, int y)
    {
        if (!isInMatrix(x, y))
        {
            return false;
        }


        string s = "";
        for (int j = 0; j < Row; j++)
        {
            s += "\n";
            for (int i = 0; i < Column; i++)
            {

                
                s += Board[j, i] + "  ";

                if (i == x && j == y && Board[j, i] == 1 && !isCubeInShape(Cube, new Vector2(i, j)))
                {
                    Debug.Log(i + " " + j);
                        
                    return false;
                }


            }
        }
        //   Debug.Log(s);

        return true;

    }
    public bool isCubeInShape(DestroySelf Cube, Vector2 point)
    {
        for (int i = 0; i < Cube.shape.ListShape.Count; i++)
        {
            if (Cube != Cube.shape.ListShape[i].GetComponent<DestroySelf>())
            {
                if (Cube.shape.ListShape[i].GetComponent<DestroySelf>().Point == point)
                {
                    return true;
                }
            }

        }
        return false;
    }
    public void SplitShape(Shape shape)
    {
        Debug.Log("INIT_1 : \n " + Render(shape.shape));
        shape.ReflectShape();
        Debug.Log("REFLECT : \n "+ Render(shape.shape));
        int colum = shape.shape.GetLength(1);
        List<Shape> splitShape = new List<Shape>();
        List<int> LenghtRow = new List<int>();
        
        List<List<int>> ShapeSplit = new List<List<int>>();
        bool next = false;
        List<int>[] Split_Row = Cut_Shape(shape);
   //for(int j = 0; j < Split_Row.Length; j++)
   //     {
   //    //    Debug.Log( RenderList(Split_Row[j]));
   //     }
        bool hasCut = false;
        if (Split_Row.GetLength(0) > 0)
        {
            next = true;
            List<int> GrounpRow = new List<int>();

                int count = 1;
          
            for(int i = 0; i < Split_Row.Length; i++)
            {
                if (!IsRowZero(Split_Row[i].ToArray()))
                {
                    GrounpRow.Add(i);
                    
                }
                else
                {
                    hasCut = true;
                    GrounpRow.Add(-1);
                }
            }
            Debug.Log(CtrlGamePlay.Ins.RenderList(GrounpRow));
                //while (count < Split_Row.GetLength(0))
                //{
                //    int[] row1 = Split_Row[count - 1].ToArray();
                //    int[] row2 = Split_Row[count].ToArray();
           
                   
                //if (isConnect(row1, row2))
                //{
                //    if (!GrounpRow.Contains(count-1))
                //    {
                //        GrounpRow.Add(count-1);
                //    }
                //    if (!GrounpRow.Contains(count))
                //    {
                //        GrounpRow.Add(count);
                //    }
                //    Debug.Log("CONNECT : " + (count-1).ToString() + " " + (count).ToString());
                //}
                //else
                //{
                //    hasCut = true;
                //    if (GrounpRow.Count != 0)
                //    {

                //        GrounpRow.Add(-1);
                //    }
                    
                   

                //    if (!IsRowZero(row1))
                //    {
                //        if (!GrounpRow.Contains(count-1))
                //        {
                //            GrounpRow.Add(count - 1);
                //        }
                          
                //    }
                //    if (!IsRowZero(row2))
                //    {
                //        if (!GrounpRow.Contains(count))
                //        {
                //            GrounpRow.Add(count);
                //        }
                            
                //    }
                //    Debug.Log("NOT_CONNECT : " + (count - 1).ToString() + " " + (count).ToString());
                //}
               
                //    count++;
                //}
            if (hasCut)
            {

                ShapeSplit.Add(GrounpRow);
                Debug.Log(RenderList(GrounpRow));
                List<int> SplitRow = new List<int>();
                int indexSplit = 0;
                List<List<int>> shapeSplit = Split(GrounpRow);
                for(int i = 0; i < shapeSplit.Count; i++)
                {
                    Debug.Log("LIST : " + i);
                    Debug.Log(RenderList(shapeSplit[i]));
                }
                for (int i = 0; i < shapeSplit.Count; i++)
                {
                    List<List<int>> shape_Split = new List<List<int>>();
                    Vector3 pos = shape.transform.position;
                    pos -= new Vector3(0, shapeSplit[i][0]* CtrlGamePlay.Ins.offsetX);
                    for (int j = 0; j < shapeSplit[i].Count; j++)
                    {
                        shape_Split.Add(Split_Row[shapeSplit[i][j]]);
                    }
                    SpawnShape(ListToMatrix(shape_Split), pos);
                    //if (i == GrounpRow.Count - 1)
                    //{
                    //    if (GrounpRow[i] != -1 && GrounpRow[indexSplit] != -1)
                    //    {
                    //        indexSplit = Mathf.Clamp(indexSplit, 0, GrounpRow.Count - 1);
                    //        List<List<int>> shapeSplit = new List<List<int>>();
                    //        for (int j = indexSplit; j <= GrounpRow.Count-1; j++)
                    //        {
                    //            shapeSplit.Add(Split_Row[j]);
                    //        }
                    //        Vector3 pos = shape.transform.position;
                    //        pos -= new Vector3(0, indexSplit * 0.6f);
                    //        Debug.Log("Split");

                    //        SpawnShape(ListToMatrix(shapeSplit), pos);
                    //    }

                    //    continue;
                    //}
                    //if (GrounpRow[i] == -1 && GrounpRow[indexSplit] != -1)
                    //{
                    //    indexSplit = Mathf.Clamp(indexSplit, 0, GrounpRow.Count - 1);
                    //    List<List<int>> shapeSplit = new List<List<int>>();
                    //    for (int j = indexSplit; j < i; j++)
                    //    {
                    //        shapeSplit.Add(Split_Row[j]);
                    //    }
                    //    Vector3 pos = shape.transform.position;
                    //    pos -= new Vector3(0, indexSplit * 0.6f);
                    //    Debug.Log("Split");

                    //    SpawnShape(ListToMatrix(shapeSplit), pos);
                    //    indexSplit = i + 1;

                    //}
                  
                      
                }
                shape.DestroyAllCubeAndShape();

            }

        }
        
     

    }
    public  List<List<int>> Split(List<int> GrounpRow)
    {
        List<List<int>> ListRow = new List<List<int>>();
        List<int> Row = new List<int>();
         for (int i = 0; i < GrounpRow.Count; i++)
        {
            if (GrounpRow[i] != -1)
            {
                Row.Add(GrounpRow[i]);
            }
            else
            {
                if (Row.Count != 0)
                {
                    ListRow.Add(CloneList(Row));
                    Row = new List<int>();
                }
            }
        }
        if (Row.Count != 0)
        {
            ListRow.Add(CloneList(Row));
            Row = new List<int>();
        }
        return ListRow;
    }
    public int[,] ListToMatrix(List<List<int>> shape)
    {
        int[,] matrix = null ; 
        if (shape.Count != 0)
        {
            int column = shape[0].Count;
            Debug.Log("Row : " + shape.Count + " " + column);
            matrix = new int[shape.Count, column];
            for(int i = 0; i < shape.Count; i++)
            {
                for(int j = 0; j < column; j++)
                {
                    matrix[i,j] = shape[i][j];
                }
            }
        }
        else
        {
            Debug.LogError("LIST MATRIX NULL");
        }
       
        return matrix;
    }


    public void  SpawnShape(int[,] Type,Vector2 pos)
    {

      //  Type = CtrlGamePlay.RemoveRow(Type);
   
        Debug.Log(Render(Type));
        var a =  Instantiate(PrebShape, pos, Quaternion.identity, null);
        a.GetComponent<Shape>().shape = Type;
    
        a.GetComponent<Shape>().AddCubeToBoard(Type, pos, Color.black,BackTo(Type));

        a.GetComponent<Shape>().ActiveShape();


    }
    public int BackTo(int[,] Type)
    {
        int x = 0;
        int Colum = Type.GetLength(1);
        int Row = Type.GetLength(0);
        for(int i = 0; i < Colum; i++)
        {
            for(int j = 0; j < Row; j++)
            {
                if (Type[j, i] != 0)
                {
                    return x;
                }
               
                   
                
            }
            x++;
        }
        Error.text = x.ToString();
       

        return x;
    }
    
    public static bool IsRowZero(int[] row)
    {
        for(int i = 0; i < row.Length; i++)
        {
            if (row[i] != 0)
            {
                return false;
            }
        }
        return true;
    }
    public string RenderList(List<int> list)
    {
        string s = "";
        for(int i = 0; i < list.Count; i++)
        {
            s += "  " + list[i];
        }
        return " ROW " +s;
    }
    public static List<int> CloneList(List<int> list)
    {
        List<int> lists = new List<int>();
        for(int i = 0; i < list.Count; i++)
        {
            int x = list[i];
            lists.Add(x);
        }
        return lists;
    }
    public static void InitShape(List<int> shape,int colum)
    {
        int count = 0;
        int row = shape.Count / colum;
        int[,] Typeshape = new int[row,colum];
        Debug.Log("ROW : " + row + "COL : " + colum);
        int[] Matrixshape = shape.ToArray();
        for(int i = 0; i < row; i++)
        {
            for(int j = 0; j < colum; j++)
            {
                Typeshape[i, j] = Matrixshape[count];
                count++;
            }
        }
        Debug.Log("SHAPE SPLIT : \n" + " " + Shape.Render( Typeshape));

    }
   
    public static bool isConnect(int[] row1, int[] row2)
    {
        int row = 0;
       // bool isConnect = false;
        for (int i = 0; i < row1.Length; i++)
        {
            if (row1[i] == 1 && row2[i] == 1)
            {
                row++;
            }
        }
        if (row > 0)
        {
            return true;
        }
        return false;
    }
    public static bool isInMatrix(int x, int y, int[,] matrix)
    {
        if (x < 0 || x > matrix.GetLength(0) || y < 0 || y > matrix.GetLength(1))
        {
            return false;
        }
        return true;
    }
    public static List<int>[] Cut_Shape(Shape shape_Split)
    {

     
        string s = "";
        int row = shape_Split.shape.GetLength(0);
        int col = shape_Split.shape.GetLength(1);
        s += "\n";

     //   Debug.Log(row + "  " + col);
        int[,] shape = shape_Split.shape;
      //  Debug.Log(Shape.Render(shape));
        List<int>[] ListRow = new List<int>[row];

        for (int i = 0; i < row; i++)
        {
          //  s += "\n";
            ListRow[i] = new List<int>();
          //  Debug.Log("ROW " + i + " : ");
            for (int j = 0; j < col; j++)
            {


                ListRow[i].Add(shape[i, j]);

                s += " " + shape[i, j];

            }

        }

     //   Debug.Log(s);
        return ListRow;

    }
    



    #region RelizeShape
    public TypeShape MatrixToType(int[,] typeShape)
    {
        TypeShape type = TypeShape.None;
        for (int i = 0; i < CtrlData.ShapeType.Count; i++)
        {
         //   Debug.Log(Render(Shape.extendMatrix(typeShape)) + "  " + Render(CtrlData.ShapeType[i]));
            if (isMatrixSame(Shape.extendMatrix(typeShape), CtrlData.ShapeType[i]))
            {
                 Debug.Log(Render(Shape.extendMatrix(typeShape)) + "  " + Render(CtrlData.ShapeType[i]));
                return CtrlData.GetShapeType(i);
            }
        }
        return type;

    }
   public bool isMatrixSame(int[,] matrix1, int[,] matrix2)
    {
        if (isSame(Shape.SplitMatrix(matrix1),Shape.SplitMatrix(matrix2)) && isSameCube(matrix1,matrix2))
        {
            return true;
        }
        return false;
    }
    private bool isSame(int[,] matrix1, int[,] matrix2)
    {
        if ((matrix1.GetLength(0) == matrix2.GetLength(0) && (matrix1.GetLength(1) == matrix2.GetLength(1))) ||
            (matrix1.GetLength(0) == matrix2.GetLength(1) && (matrix1.GetLength(1) == matrix2.GetLength(0))))
            
             
        {
                return true;
          
        }
        return false;
    }
    private bool isSameCube(int[,] matrix1, int[,] matrix2)
    {
        Debug.Log("COUNT");
        Debug.Log(CountInCube(matrix1) + "  " + CountInCube(matrix2));
        return CountInCube(matrix1) == CountInCube(matrix2);

    }
    public static int  CountInCube(int[,] matrix1)
    {
        int x = 0;
        for(int i = 0; i < matrix1.GetLength(0); i++)
        {
            for(int j = 0; j < matrix1.GetLength(1); j++)
            {
                if (matrix1[i,j] == 1)
                {
                    x++;
                }
            }
        }
        return x;
    }
    public static int[,] RemoveRow(int[,] matrix)
    {

        List<List<int>> Colum = Cut_Colum(matrix);
        List<List<int>> RemoveColum = new List<List<int>>();
        for (int i = 0; i < Colum.Count; i++)
        {
            //Debug.Log(i + "\n" + isListZero(Colum[i]).ToString() + "\n" + CtrlGamePlay.Ins.RenderList(Colum[i]));
            // Debug.Log(i);
            //Debug.Log(isListZero(Colum[i]).ToString());
            // Debug.Log(CtrlGamePlay.Ins.RenderList(Colum[i]));
        }
        for (int i = 0; i < Colum.Count; i++)
        {
            if (isListZero(Colum[i]))
            {
             
                RemoveColum.Add(Colum[i]);
            }
        }
        for(int i = 0; i < RemoveColum.Count; i++)
        {
            Debug.Log(CtrlGamePlay.Ins.RenderList(RemoveColum[i]));
        }
        int s = RemoveColum.Count;
        if (s > 0)
        {
            while (s > 0)
            {
                s--;
                Debug.Log("REMOVE ROW : " + s);
                Colum.Remove(Colum[s]);

              



            }
        }
       
          return ListColumToMatrix(Colum);
    }
    public static int[,] ListColumToMatrix(List<List<int>> list)
    {
        int row = list[0].Count;

        int[,] matrix = new int[row,list.Count];
        for(int i = 0; i < list.Count; i++)
        {
            for(int j = 0; j < row; j++)
            {
                matrix[j, i] = list[i][j];
            }
        }
        return matrix;
    }
    public static bool isListZero(List<int> list)
    {
        for(int i = 0; i < list.Count; i++)
        {
            if (list[i] != 0)
            {
                return false;
            }
        }
        return true;
    }
    public static List<List<int>> Cut_Colum(int[,] matrix)
    {
        List<List<int>> ColumGroup = new List<List<int>>();
        List<int> Colum = new List<int>();
        int row = matrix.GetLength(0);
        int col = matrix.GetLength(1);
        for (int i = 0; i < col; i++)
        {
            Colum = new List<int>();
            for (int j = 0; j < row; j++)
            {
                int x = matrix[j, i];
                Colum.Add(x);
            }

            ColumGroup.Add(CtrlGamePlay.CloneList(Colum));
            Debug.Log("Render : " + CtrlGamePlay.Ins.RenderList(CtrlGamePlay.CloneList(Colum)));
            Debug.Log("Cut Column : " + ColumGroup.Count);
           

        }
    
        return ColumGroup;



    }
    public void ClampShape_Down(Shape shape)
    {


       
        int minY =0;
        List<Vector2> Point = new List<Vector2>();
      
        bool isMove = true;
        string s = "";
        isMove = true;
        s += "MOVE Down : \n";
        while (isMove)
        {
            minY ++;
            for (int i = 0; i < shape.ListShape.Count; i++)
            {
                int x = (int)shape.ListShape[i].GetComponent<DestroySelf>().Point.x;
                int y = (int)shape.ListShape[i].GetComponent<DestroySelf>().Point.y + minY;
                s += " apply x : " + x + "y :" + y + "\n";
                if (!IsCanMove(shape.ListShape[i].GetComponent<DestroySelf>(), x, y))
                {

                    isMove = false;
                    minY--;
                    minY = Mathf.Clamp(minY, 0,Row);
                    s += " reject  x : " + x + "y :" + y + "\n";

                    break;
                }
            }

        }

        MoveSelect.text = "DOWN : " + minY +"\n";
        float ClampMinY = minY * offsetY;
     

        //MoveSelect.text += ClampMinX + " :: " + ClampMaxX;
        shape.SetUpClamp(ClampMinY);

        //MoveSelect.text = s;
        //Debug.Log(s);
       
     
    }


     

    public int isDowmShape(Shape shape)
    {



        int minY = 0;
        List<Vector2> Point = new List<Vector2>();

        bool isMove = true;
        string s = "";
        isMove = true;
        s += "MOVE Down : \n";
        while (isMove)
        {
            minY++;
            for (int i = 0; i < shape.ListShape.Count; i++)
            {
                int x = (int)shape.ListShape[i].GetComponent<DestroySelf>().Point.x;
                int y = (int)shape.ListShape[i].GetComponent<DestroySelf>().Point.y + minY;
                s += " apply x : " + x + "y :" + y + "\n";
                if (!IsCanMove(shape.ListShape[i].GetComponent<DestroySelf>(), x, y))
                {

                    isMove = false;
                    minY--;
                    minY = Mathf.Clamp(minY, 0, Row);
                    s += " reject  x : " + x + "y :" + y + "\n";

                    break;
                }
            }

        }

        MoveSelect.text = "DOWN : " + minY + "\n";
        float ClampMinY = minY * offsetY;


        return minY;

    }



    #endregion

    #region StartGame
    public void GenerateStartGame(int index)
    {
        int[,] backup = Shape.Clone(Board);
        int[,] CloneBoard = Shape.Clone(Board);
        bool isCorrect = false;
        while (!isCorrect)
        {
           
            Vector3 pos = CtrlGamePlay.RandomPosShape();
           
            for(int i = 0; i < index; i++)
            {
                if(isSpawnCorrect(CloneBoard,Shape.RandomShape(), CtrlGamePlay.RandomPosShape()))
                {

                }
            }
              
            isCorrect = true;
        }
    }
    
    public bool isSpawnCorrect(int[,] Board,TypeShape type,Vector3 pos)
    {
       
        

        return true;
    }
    public bool PushToBoard(int[,] Board,int x,int y)
    {
        return IsPushShapeCorrect(Board, x, y);
    }
    public  bool IsPushShapeCorrect(int[,] Board, int x, int y)
    {
        for(int i = 0; i < Board.GetLength(0); i++)
        {
            for(int j = 0; j < Board.GetLength(1); j++)
            {
                if (Board[i, j] == 1)
                {
                    return false;
                }
            }
        }
        return true;
    }
     
    public int RollShape(TypeShape TypeShape,int[,] matrix)
    {
        int[,] shape = null;
        switch (TypeShape)
        {
            case TypeShape.crossBar_1:
                shape = CtrlData.Cube_Cross_1;

                break;
            case TypeShape.crossBar_2:

                shape = CtrlData.Cube_Cross_2;

                break;
            case TypeShape.crossBar_3:

                shape = CtrlData.Cube_Cross_3;

                break;
            case TypeShape.crossBar_4:
                shape = CtrlData.Cube_Cross_4;

                break;
            case TypeShape.square :
                shape = CtrlData.Cube_Quare;
                break;
            case TypeShape.L_3:
                shape = CtrlData.Cube_L3_0;

                break;
         
            case TypeShape.three_cube:
                shape = CtrlData.Cube_3;

                break;

        }
        int roll = 0;


        return roll;
    }
    public int[,] RollShape(int roll,TypeShape type)
    {

        int[,] shape = null;
        switch (type)
        {
            case TypeShape.crossBar_1:
                shape = CtrlData.Cube_Cross_1;
             
                break;
            case TypeShape.crossBar_2:

                shape = CtrlData.Cube_Cross_2;

                break;
            case TypeShape.crossBar_3:

                shape = CtrlData.Cube_Cross_3;

                break;
            case TypeShape.crossBar_4:
                shape = CtrlData.Cube_Cross_4;

                break;
            case TypeShape.square:
                shape = CtrlData.Cube_Quare;
                break;
            case TypeShape.L_3:
                shape = CtrlData.Cube_L3_0;

                break;

            case TypeShape.three_cube:
                shape = CtrlData.Cube_3;

                break;

        }
        shape = Shape.RotationMaxtrix(shape, roll);
        return shape;
       

       

    }
    



    #endregion


}






