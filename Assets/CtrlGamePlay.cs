using System.Collections;
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
    public Shape ShapeClone;
    public GameObject SimulateColumn;
    public float SpeedMoveDown = 0;
    public Vector2 Point;
    public bool Watting = false;
    public float WaitTime = 0.4f;
    private float TimeWait;
    public float time;
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
    public delegate void GameOver();
    #endregion
    #region Action
    public event ClickDownHander Event_Click_Down;
    public event ClickUpHander Event_Click_Up;
    public event MoveDownComplete Event_Completed_Move_Down;
    public event StartGame Event_Start_Game;
    public event MoveDownComplete Event_Completed_Change;
    public event GameOver Event_Game_Over;
    #endregion

    // Simulate 
    public Text Matrix;
    public Text Select;
    public Text ShapeSelect;
    public Text MoveSelect;
    public Text Error;
    public Text TextPointInit;


    //Test
    
    public int xx;
    public int yy;
    public int rowDestroy;
    public int rotaion;
  
    // Start is called before the first frame update
     
    private void Awake()
    {
        Application.targetFrameRate = 100;
        if (Ins != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Ins = this;
        }
        Board = new int[Row, Column];
        string s = Render(Board);
        Debug.Log(s);
        Init_Event();
        Vector3 pos = transform.position;
        ClampY = PosInit.y - Row * offsetY;
        TimeWait = 0;
       
    }
    public void Init_Event()
    {
        Event_Click_Down += Init_Mouse_Down;
        Event_Click_Up += SetUpMoveDown;
        Event_Completed_Move_Down += SpawnShape;
        Event_Start_Game +=SpawnStartGame;
        Event_Start_Game += UnRigidBody;
        Event_Start_Game += SpawnShape;
        Event_Completed_Change += SetUpMoveDown;
   


    }

    #region StatusGame

    public void Start_Game()
    {
        Event_Start_Game();
    }
    public void Over_Game()
    {
        Reset_Game();
    }
    public void Rest_Game()
    {
        Debug.Log("RestGame");
        Reset_Game();
        GameManager.Ins.StartGame();
    }
    

    #endregion
    void Start()
    {
      //  Event_Completed_Move_Down();
     

      
     //   Event_Start_Game();
      
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Ins.isGameOver && !GameManager.Ins.isGamePause)
            return;

        if (TimeWait > 0)
        {
            TimeWait-=Time.deltaTime;
            if (TimeWait <= 0)
            {
                SimulateDown();
            }
            return;
        }
    
        #region
      
        if (Input.GetKeyDown(KeyCode.E))
        {
            SpawnShape();
        }
        if (Input.GetKeyDown(KeyCode.A))
        {

            SortShape();

        }
        if (Input.GetKeyDown(KeyCode.Z))
        {


            SimulateDown();

        }
           
        if (ShapeClick != null)
        {

        }
        Matrix.text = Render((Board));

        if (Input.GetKeyDown(KeyCode.L))
        {
            Test();
           
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            DestroyRow(13);
            SplitShape();
            SplitShape();
        }


        #endregion
        if (Input.GetMouseButtonDown(0))
        {

            if(!isClick_up)
            {
                Event_Click_Down();
                isClick_down = true;
            }
              
          
          



        }
        if (isClick_down)
        {
            int Direct = UpdatePoint(Input.mousePosition);
            float dis = 0;
            // RefershBoard();
            if (ShapeClick != null)
            {
                Time.timeScale = 1;



                //    ActiveRigidBoy(false);                                 //Reset
                ShapeClick.PushToBoard();
                Event_Completed_Change();
                SimulateColumn.gameObject.SetActive(true);
                SimulateColumn.GetComponent<MoveFollowCube>().shape = ShapeClick;
                SimulateColumn.GetComponent<MoveFollowCube>().ScaleX(ScaleShape(ShapeClick));
                dis = PosMouseInit.x - Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
                ShapeClick.MoveTo(dis);
                //  Shape.transform.position = ScreenToWord(Input.mousePosition);
                //   Debug.Log("Move");
            }
            else
            {


                ShapeSelect.text = "NULL";
            }
            Select.text += "\n Direct :" + Direct;

        }

        if (Input.GetMouseButtonUp(0))
        {

            if (!isClick_up)
            {

                Event_Click_Up();
                isClick_down = false;
                isClick_up = true;
                if (ShapeClick != null)
                {
                    Vector2 point = ShapeClick.Snap();
                  //  ShapeClone.gameObject.AddComponent<DestroySelf1>().Destroy();
                    ShapeClick.ResetStatus();
                    RefershBoard();
                    if (ShapeClick.isShapeMove(point))
                    {
                        SimulateDown();
                        SetUpAll();                                             
                    }
                    SimulateColumn.gameObject.SetActive(false);
                    ShapeClick = null;
                    //  Event_Completed_Change();



                }
            }
          
          
           
          

        }
        // Button_Up_And_Complete_Move_Down
        if (isClick_up)
        {
           
         
            if (IsListShapeMove())
            {
                
                RefershBoard();
              
                Event_Completed_Move_Down();
                Debug.Log("Complete_Move");
              
                initPos = false;

                if (HasScore())
                {
                    CheckDestroyRow();
                    SplitShape();
                    SplitShape();

                    RefershBoard();
                    TimeWait = WaitTime;
                }
                else
                {
                    isClick_up = false;
                }
           
              
              
              
                    
               

            }

        }
        //////////////                MOVE DOWN  ///////////////////////
        ///

     



        
       
    }
    public bool HasScore()
    {
        int has = 0;
        for(int i = 0; i < Board.GetLength(0); i++)
        {
            int count = 0;
            for(int j = 0; j < Board.GetLength(1); j++)
            {
                if (Board[i, j] == 1)
                {
                    count++;
                }
            }
            if(count == Board.GetLength(1))
            {
                has++;
            }
        }
        if (has != 0)
        {
            return true;
        }
        return false;
    }
    public int ScaleShape(Shape shape)
    {
           if(shape!=null)
        return shape.shape.GetLength(1);
        return 0;
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
      
    }
    public void ActiveRigidBoy(bool active)
    {
       
        for(int i=0 ; i < List_Shape.Count; i++)
        {
           if(List_Shape[i].gameObject.layer == 8)
            {
            //    List_Shape[i].Body.isKinematic = !active;
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
            if (List_Shape[i].isMovingDown)
                return false;
        }
        return true;
    }
    public void AddForce()
    {
        for (int i = 0; i < List_Shape.Count; i++)
        {
            List_Shape[i].Body.AddForce(new Vector3(0, -100, 0), ForceMode2D.Impulse);
        }
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
         //   CloneShape(ShapeClick);


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
        ReflectShape();
        SplitShape();
        SplitShape();
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

        GenerateStartGame(Random.Range(1, 4),false);
    }
    public void SpawnStartGame()
    {
        GenerateStartGame(Random.Range(1, 4), true);
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
       

      


    }

    public static string Render(int[,] matrix)
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

    public  Vector2 RandomPosStart()
    {
      
      
       
        int y = Random.Range(Row-Random.Range(0,4), Row);
        int x = Random.Range(0, CtrlGamePlay.Ins.Column);
    
     
        return new Vector3(CtrlGamePlay.Ins.initPoint.x + x * CtrlGamePlay.Ins.offsetX, CtrlGamePlay.Ins.initPoint.y-y* CtrlGamePlay.Ins.offsetX);

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
                 //   Debug.Log(i + " " + j);
                        
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
        int Color = shape.IDColor;
       // Debug.Log("INIT_1 : \n " + Render(shape.shape));
        shape.ReflectShape();
     //   Debug.Log("REFLECT : \n "+ Render(shape.shape));
        int colum = shape.shape.GetLength(1);
        List<Shape> splitShape = new List<Shape>();
        List<int> LenghtRow = new List<int>();
        
        List<List<int>> ShapeSplit = new List<List<int>>();
        bool next = false;
        List<int>[] Split_Row = Cut_Shape(shape);
        for (int j = 0; j < Split_Row.Length; j++)
        {
          //     Debug.Log( RenderList(Split_Row[j]));
        }
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
         //   Debug.Log(CtrlGamePlay.Ins.RenderList(GrounpRow));
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
                   //   Debug.Log("LIST : " + i);
                   // Debug.Log(RenderList(shapeSplit[i]));
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
                    string s = "Shape Split : ";

                    s +="\n" +  Render(ListToMatrix(shape_Split)) ;
                    Debug.Log(s);
                    SpawnShape(ListToMatrix(shape_Split), pos,Color);
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
          //  Debug.Log("Row : " + shape.Count + " " + column);
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


    public void  SpawnShape(int[,] Type,Vector2 pos,int color)
    {

        int back = BackTo(Type);
        pos = new Vector2(pos.x += back * offsetX, pos.y);
        TypeShape type = CtrlGamePlay.Ins.MatrixToType(Type);
        int roll = RollShape(type, Type);
       Type =  Shape.SplitMatrix(CtrlGamePlay.standardizedMatrix(Type));
        Debug.Log("INFOR SHAPE SPLIT : " + back + " :: " + type.ToString() + " :: " + roll);

        InforShape infor = new InforShape(CtrlGamePlay.Ins.MatrixToType(Type), pos, Type, roll,color);

        SpawnShape(infor);
   
       


        // a.GetComponent<Shape>().AddCubeToBoard(Type, pos, Color.black,BackTo(Type));




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
        if (x < 0 || x >= matrix.GetLength(0) || y < 0 || y >= matrix.GetLength(1))
        {
            return false;
        }
        return true;
    }
    public static bool isInMatrixBoard(int row, int column, int[,] matrix)
    {
        if (column < 0 || column >=matrix.GetLength(1) || row < 0 || row >=matrix.GetLength(0))
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
        if (CountInCube(typeShape)==1)
        {

            return TypeShape.crossBar_1;
        }

        for (int i = 0; i < CtrlData.ShapeType.Count; i++)
        {
            if (isTypeOf(CtrlData.GetShapeType(i),Shape.Clone(typeShape)))
            {
                return CtrlData.GetShapeType(i);
            } 
           
        }
        return TypeShape.None;

    }
    public bool isTypeOf(TypeShape type,int[,] shape)
    {
        int[,] CloneShape = standardizedMatrix(shape);
        CloneShape = Shape.SplitMatrix(CloneShape);
     //   Debug.Log("Clone Shape : " + Render(shape));
        string s1 = Render(CloneShape);
        int[,] matrix = null;
        int roll = 0;
        switch (type)
        {
            case TypeShape.crossBar_1:
                matrix = CtrlData.ShapeType[0];
                roll = CtrlData.NotRoll;
                break;
            case TypeShape.crossBar_2:
                matrix = CtrlData.ShapeType[1];
               // Debug.Log("Cross 2 : " + Render(matrix));
                roll = CtrlData.Roll_Cross;
                break;
            case TypeShape.crossBar_3:
                matrix = CtrlData.ShapeType[2];
                Debug.Log("Cross 3 : " + Render(matrix));
                roll = CtrlData.Roll_Cross;
                break;
            case TypeShape.crossBar_4:
                matrix = CtrlData.ShapeType[3];
                roll = CtrlData.Roll_Cross;
                break;
            
            case TypeShape.L3_0:
                matrix = CtrlData.ShapeType[8];
                roll = CtrlData.Roll_Cube_L;
                break;
            case TypeShape.L3_90:
                matrix = CtrlData.ShapeType[9];
                roll = CtrlData.Roll_Cube_L;
                break;
            case TypeShape.L_4_0:
                matrix = CtrlData.ShapeType[4];
                roll = CtrlData.Roll_Cube_L;
                break;
            case TypeShape.L4_90:
                matrix = CtrlData.ShapeType[5];
                roll = CtrlData.Roll_Cube_L;
                break;

            case TypeShape.three_cube:
                matrix = CtrlData.ShapeType[7];
                roll = CtrlData.Roll_Cube_L;
                break;
            case TypeShape.square:
                matrix = CtrlData.ShapeType[6];
                roll = CtrlData.NotRoll;
                break;
              
        }

       
        for(int i = 0; i < roll; i++)
        {
            string s = Render(Shape.SplitMatrix(SimulateRoll(i,type,false)));
            Debug.Log(s1 + "\n" + s);
            if (s == s1)
            {
                return true;
            }
        }
        return false;
     

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
       //     Debug.Log(CtrlGamePlay.Ins.RenderList(RemoveColum[i]));
        }
        int s = RemoveColum.Count;
        if (s > 0)
        {
            while (s > 0)
            {
                s--;
              //  Debug.Log("REMOVE ROW : " + s);
               // Colum.Remove(Colum[s]);

              



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
        //    Debug.Log("Render : " + CtrlGamePlay.Ins.RenderList(CtrlGamePlay.CloneList(Colum)));
         //   Debug.Log("Cut Column : " + ColumGroup.Count);
           

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
     public bool IsCanMove(DestroySelf Cube, int x, int y,int[,] Board)
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
                 //   Debug.Log(i + " " + j);
                        
                    return false;
                }


            }
        }
        //   Debug.Log(s);

        return true;

    }

    public int isDowmShape(Shape shape,int[,] Board) 
    {



        int minY = 0;
        List<Vector2> Point = new List<Vector2>();

        bool isMove = true;
        string s = "";
        isMove = true;
        s += "MOVE Down : \n";
      
            minY++;
            for (int i = 0; i < shape.ListShape.Count; i++)
            {
                int x = (int)shape.ListShape[i].GetComponent<DestroySelf>().Point.x;
                int y = (int)shape.ListShape[i].GetComponent<DestroySelf>().Point.y + 1;
                s += " apply x : " + x + "y :" + y + "\n";
                if (!IsCanMove(shape.ListShape[i].GetComponent<DestroySelf>(), x, y))
                {

                    isMove = false;
                    minY--;
                    minY = Mathf.Clamp(minY, 0, Row);
                    s += " reject  x : " + x + "y :" + y + "\n";

                  return 0;
                 }
            else
            {
                return 1;
            }
            }

           return 1;

       

    }
    public void MoveDown()
    {

    }

    public void SimulateDown()
    {
        SortShape();
        List<Shape> ListShapeMove = new List<Shape>();
        int[,] shape = CloneBoard(this.Board);
       // Debug.Log("INIT  : " + Render(shape));
        List<List<Vector2>> Shapes = PushListCubeInList(List_Shape);
        List<int> ListMove = GenerateList(Shapes.Count);
        while(isCheckDown(shape,1,Shapes))
        for(int i = 0; i < Shapes.Count; i++)
        {
       //     Debug.Log(" : " + i + "  :" + Render(shape));
            if (isMoveDown(shape, 1, Shapes[i]))
            {
                ListMove[i]++;
                SimulateMoveDown(Shapes[i], 1, shape);
              //  ListShapeMove.Add(List_Shape[i]);
              //  Debug.Log(" : " + i + "  :" + Render(shape));
            }
            else
            {
                ResetMove(Shapes[i], shape);
            }
          

        }

        if (ListShapeMove.Count == 0)
        {
         //   Debug.Log("Khong Co");
        }
      //  Debug.Log("CANVAS : " + Render(shape));
         for(int i = 0; i < List_Shape.Count; i++)
        {
          
            StartCoroutine(MoveDownOneCube(List_Shape[i],ListMove[i]));
        }
            

           
    }
    public List<int> GenerateList(int count)
    {
        List<int> l = new List<int>();
        for(int i = 0; i < count; i++)
        {
          
            l.Add(0);
        }
        return l;
    }
    
   
     public bool isCheckDown(int[,] Board,int space, List<List<Vector2>> Shapes) 
    {

        for(int i = 0; i < Shapes.Count; i++)
        {
            if (HasMoveDown(Board, space, Shapes[i]))
            {
                return true;
            }
        }
        return false;

    }
    public bool isListShapeMove()
    {
        for(int i = 0; i < List_Shape.Count; i++)
        {
            if (List_Shape[i].isMovingDown)
            {
                return false;
            }
           
        }
        return true;
    }
    public bool HasMoveDown(int[,] Board, int space, List<Vector2> shape)
    {
        StartMove(shape, Board);
        for (int i = 0; i < shape.Count; i++)
        {
          
            if (CtrlGamePlay.isInMatrix((int)shape[i].y + space, (int)shape[i].x, Board))
            {
                if (Board[((int)shape[i].y + space), (int)shape[i].x] == 1)
                {
                    ResetMove(shape, Board);
                    return false;
                }

            }
            else
            {
                ResetMove(shape, Board);
                return false;
            }
        }
        ResetMove(shape, Board);
        return true;
    }

    public bool isMoveDown(int[,] Board,int space,List<Vector2> shape)
    {
        StartMove(shape, Board);
        for (int i = 0; i < shape.Count; i++)
        {
         
            if (CtrlGamePlay.isInMatrix((int)shape[i].y+space, (int)shape[i].x,Board))
            {
                if(Board[((int)shape[i].y + space), (int)shape[i].x] == 1)
                {

                    return false;
                }
               
            }
            else
            {
                return false;
            }
        }
        return true;
    }
   

    public List<List<Vector2>> PushListCubeInList(List<Shape> list_shape)
    {
        List<List<Vector2>> ListShape = new List<List<Vector2>>();

        for(int i = 0; i < list_shape.Count; i++)
        {
            List<Vector2> shape = new List<Vector2>();
            for(int j = 0; j < list_shape[i].ListShape.Count; j++)
            {
                shape.Add(list_shape[i].ListShape[j].GetComponent<DestroySelf>().Point);
            }
            ListShape.Add(shape);
        }
        return ListShape;
    }
    public void StartMove(List<Vector2> Shape,int[,] Board)
    {
        for(int i = 0; i < Shape.Count; i++)
        {
            Vector2 point = Shape[i];
            Board[(int)point.y, (int)point.x] = 0;
        }
    }
    public void ResetMove(List<Vector2> Shape, int[,] Board)
    {
        for (int i = 0; i < Shape.Count; i++)
        {
            Vector2 point = Shape[i];
            Board[(int)point.y, (int)point.x] = 1;
        }
    }

    public void SimulateMoveDown(List<Vector2> shape, int space, int[,] Board)
    {
       
        for (int i = 0; i < shape.Count; i++)
        {
            Vector2 point = shape[i];
            Board[(int)point.y, (int)point.x] = 0;

        }
        for (int i = 0; i < shape.Count; i++)
        {
            Vector2 point = shape[i];
            Board[((int)point.y + space), (int)point.x] = 1;
            shape[i] = new Vector2((int)point.x, ((int)point.y + space));
        }

    }
    public IEnumerator MoveDownOneCube(Shape shape,int Space)
    {
        
        float dis = 0;
        float  space = offsetX/ SpeedMoveDown;
        if (Space != 0)
        {


            if (Space != 1)
            {
                while (dis < offsetX * Space - space)
                {
                    shape.isMovingDown = true;
                    Vector3 pos = shape.transform.position;
                    pos.y -= space;
                    dis += space;
                    shape.transform.position = pos;
                    yield return new WaitForSeconds(0);
                }
            
                shape.isMovingDown = false;
            }
            else if (Space == 1)
            {
                while (dis < offsetX * Space)
                {
                    shape.isMovingDown = true;
                    Vector3 pos = shape.transform.position;
                    pos.y -= space;
                    dis += space;
                    shape.transform.position = pos;
                    yield return new WaitForSeconds(0);
                }
              
            }
        }
        int offset = Mathf.RoundToInt((initPoint.y- shape.transform.position.y) / offsetY);
        shape.transform.position = new Vector2(shape.transform.position.x, initPoint.y - offset * offsetY);
        shape.isMovingDown = false;
        yield return new WaitForSeconds(0);

    }



    #endregion

    #region StartGame
    public void GenerateStartGame(int index,bool Start)
    {
        int[,] backup = CtrlGamePlay.CloneBoard(Board);
        int[,] CloneBoard = CtrlGamePlay.CloneBoard(Board);
        Debug.Log("Board Clone : "+"\n"+ Render(CloneBoard));

        bool isCorrect = false;
        List<InforShape> InforShape = new List<InforShape>();
        int cont = 0;
        while (!isCorrect)
        {
            cont++;
            isCorrect = true;
       //     Debug.Log("Spawn Again ");
        
            
            for(int i = 0; i < index; i++)
            {



                if (!isCorrect)
                    break;
                //      Debug.Log("Check : " + "\n" + Render(CloneBoard));
                    Vector3 pos;
               
                    TypeShape type = Shape.RandomShape();
                    int[,] shape = null;
                    int roll = 0;

                if (Start)
                {

                    pos = CtrlGamePlay.Ins.RandomPosStart();
                    if (isSpawnCorrect_For_Start_Game(CloneBoard, type, pos, out shape, out roll))
                    {
                        int color  = CtrlData.RandomColor(type, roll);
                        InforShape infor = new InforShape(type, pos, shape, roll,color);
                        //    Debug.Log("Spawn SS : " + i);
                        //     Debug.Log("Board SS : " + "\n" + Render(CloneBoard));
                        InforShape.Add(infor);
                    }
                    else
                    {
                        InforShape.Clear();
                        CloneBoard = CtrlGamePlay.CloneBoard(Board);
                        isCorrect = false;
                    }

                }
                else
                {
                    
                    pos = CtrlGamePlay.RandomPosShape();
                    if (isSpawnCorrect(CloneBoard, type, pos, out shape, out roll))
                    {
                        int color = CtrlData.RandomColor(type, roll);
                        InforShape infor = new InforShape(type, pos, shape, roll,color);
                        //    Debug.Log("Spawn SS : " + i);
                        //     Debug.Log("Board SS : " + "\n" + Render(CloneBoard));
                        InforShape.Add(infor);
                    }
                    else
                    {
                        InforShape.Clear();
                        CloneBoard = CtrlGamePlay.CloneBoard(Board);
                        isCorrect = false;
                    }

                }


            }

        
        }
        //   Debug.Log("SPAWWN :" +InforShape.Count);
       
        SpawnShape(InforShape,Start);
    }
    public void SpawnShape(List<InforShape> ListInfor,bool isActive)
    {

        for(int i = 0; i < ListInfor.Count; i++)
        {


            var a = Instantiate(PrebShape, ListInfor[i].pos, Quaternion.identity, null);
            a.GetComponent<Shape>().isCubeStart = isActive;
            
            a.GetComponent<Shape>().SetTypeShape(ListInfor[i].type, ListInfor[i].shape);
          
            a.GetComponent<Shape>().Set_Up_Corrs(ListInfor[i].roll, ListInfor[i].type,ListInfor[i].color);


            idShape++;
         //   a.GetComponent<Shape>().GetComponent<Rigidbody2D>().mass = (100000 - idShape*2);
            a.name = "Shape : " + idShape;
            List_Shape.Add(a.GetComponent<Shape>());
            if (isActive)
            {
              //    a.GetComponent<Shape>().Body.isKinematic = false;
                //a.gameObject.layer = 8;
                //List<GameObject> Cubes = a.GetComponent<Shape>().ListShape;
                //for (int j = 0; j<Cubes.Count; j++)
                //{
                
                //    Cubes[i].layer = 8;
                //}
                
            }
          
        }
     
    }
    public void SpawnShape(InforShape ListInfor)
    {
        var a = Instantiate(PrebShape, ListInfor.pos, Quaternion.identity, null);
        a.GetComponent<Shape>().SetTypeShape(ListInfor.type, ListInfor.shape);
        a.GetComponent<Shape>().Set_Up_Corrs(ListInfor.roll, ListInfor.type,ListInfor.color);
        idShape++;
        a.name = "Shape : " + idShape;
        List_Shape.Add(a.GetComponent<Shape>());
        a.GetComponent<Shape>().ActiveShape();
    }

    public void Test()
    {
       
        //int i = 3;
        int i = Random.Range(0, 4);
        int[,] shape = Shape.RotationMaxtrix(CtrlData.Cube_L3_90, i);
        shape = Shape.SplitMatrix(shape);
        int color = CtrlData.RandomColor(TypeShape.L3_90 , i);
        InforShape infor = new InforShape(TypeShape.L3_90, CtrlGamePlay.RandomPosShape(),shape, i,color);
        List<InforShape> ListInfor = new List<InforShape>();
        ListInfor.Add(infor);
      
        SpawnShape(ListInfor,false);
    }


    
    public bool isSpawnCorrect(int[,] Board,TypeShape type,Vector3 pos,out int[,] shapeCorrect,out int Roll)
    {
        int indexRoll = 0;
        int[,] shape = SimulateRoll(0, type, true,out indexRoll);
        Roll = indexRoll;
        shape = Shape.SplitMatrix(shape);
      //  Debug.Log("Board :::: ");
    //    Debug.Log(Render(shape));
       for(int i = 0; i < shape.GetLength(0); i++)
        {
            for(int j = 0; j < shape.GetLength(1); j++)
            {
                if (shape[i, j] != 0)
                {
                    Vector3 posCurr = new Vector3(pos.x + j* CtrlGamePlay.Ins.offsetX, pos.y - i * CtrlGamePlay.Ins.offsetY);

                     Vector2 point = CtrlGamePlay.PositonToPointMatrix(posCurr.x, posCurr.y);
                 //   Debug.Log(point);
                    if (IsPushShapeCorrect(Board,(int)point.x, (int)point.y))
                    {
                        Board[(int)point.x, (int)point.y] = 1;
                      
                    }
                    else
                    {
                        shapeCorrect = null;
                 //       Debug.Log("Spawn False : " + point.x +" :: "+point.y);
                        return false;
                    }
                }
               

            }
        }

        shapeCorrect = shape;
        return true;
                   
             
    }
  
    public bool isSpawnCorrect_For_Start_Game(int[,] Board, TypeShape type, Vector3 pos, out int[,] shapeCorrect, out int Roll)
    {
        int indexRoll = 0;
        int[,] shape = SimulateRoll(0, type, true, out indexRoll);
        Roll = indexRoll;
        shape = Shape.SplitMatrix(shape);
        //  Debug.Log("Board :::: ");
        //    Debug.Log(Render(shape));
        int Ground = 0;
        for (int i = 0; i < shape.GetLength(0); i++)
        {
            for (int j = 0; j < shape.GetLength(1); j++)
            {
                if (shape[i, j] != 0)
                {
                    Vector3 posCurr = new Vector3(pos.x + j * CtrlGamePlay.Ins.offsetX, pos.y - i * CtrlGamePlay.Ins.offsetY);

                    Vector2 point = CtrlGamePlay.PositonToPointMatrix(posCurr.x, posCurr.y);
                    //   Debug.Log(point);
                    if (IsPushShapeCorrect(Board, (int)point.x, (int)point.y))
                    {
                        Board[(int)point.x, (int)point.y] = 1;
                        if(isGround((int)point.x))
                        {
                            Ground++;
                        }

                    }
                    else
                    {
                        shapeCorrect = null;
                        //       Debug.Log("Spawn False : " + point.x +" :: "+point.y);
                        return false;
                    }
                }


            }
        }
        if (Ground==0)
        {
            shapeCorrect = null;
            return false;
        }
        else
        {
            shapeCorrect = shape;
            return true;
        }
      
       

    }
    public bool isGround(int row)
    {
      if(row == (CtrlGamePlay.Ins.Row - 1))
        {
            return true;
        }
        else
        {
            return false;
        }
     
    }

    public static Vector2 PositonToPointMatrix(float PosX, float PosY)
    {

       
        int x = Mathf.RoundToInt((PosX - CtrlGamePlay.Ins.initPoint.x) / CtrlGamePlay.Ins.offsetX);
        int y = Mathf.RoundToInt((CtrlGamePlay.Ins.initPoint.y - PosY) / CtrlGamePlay.Ins.offsetY);
        return new Vector2(y, x);

    }
    public bool TryPushToBoard(int[,] Board,int row,int column)
    {
        return IsPushShapeCorrect(Board, row, column);
    }
    public static bool IsPushShapeCorrect(int[,] Board, int row, int column)
    {
        if (isInMatrixBoard(row,column,Board))
        {
           // Debug.Log(row + "  " + column);
            if(Board[row, column] != 1)
            return true;
        }
        return false;
    }
  

    public static int RollShape(TypeShape TypeShape,int[,] matrix)
    {
        int roll = 0;
        int[,] shape = null;
        int[,] shape_Clone =null;
        
        switch (TypeShape)
        {
            case TypeShape.crossBar_1:
                shape = Shape.Clone(CtrlData.Cube_Cross_1);
                roll = CtrlData.NotRoll;
                break;
            case TypeShape.crossBar_2:

                shape = Shape.Clone(CtrlData.Cube_Cross_2);
                roll = CtrlData.Roll_Cross ;
                break;
            case TypeShape.crossBar_3:

                shape = Shape.Clone(CtrlData.Cube_Cross_3);
                roll = CtrlData.Roll_Cross;
                break;
            case TypeShape.crossBar_4:
                shape = Shape.Clone(CtrlData.Cube_Cross_4);
                roll = CtrlData.Roll_Cross;
                break;
            case TypeShape.square :
                shape = Shape.Clone(CtrlData.Cube_Quare);
                roll  =  CtrlData.NotRoll;
                break;
            case TypeShape.L3_0:
                shape = Shape.Clone(CtrlData.Cube_L3_0);
                roll = CtrlData.Roll_Cube_L;
                break;
            case TypeShape.L3_90:
                shape = Shape.Clone(CtrlData.Cube_L3_90);
                roll = CtrlData.Roll_Cube_L;
                break;
            case TypeShape.L_4_0:
                shape = Shape.Clone(CtrlData.Cube_L4_0);
                roll = CtrlData.Roll_Cube_L;
                break;
            case TypeShape.L4_90:
                shape = Shape.Clone(CtrlData.Cube_L4_90);
                roll = CtrlData.Roll_Cube_L;
                break;

            case TypeShape.three_cube:
                shape = Shape.Clone(CtrlData.Cube_3);
                roll = CtrlData.Roll_Cube_L;
                break;

        }
        shape_Clone = standardizedMatrix(matrix);
        shape_Clone = Shape.SplitMatrix(shape_Clone);
        for (int i = 0; i < roll; i++)
        {

            string s  = Render(Shape.SplitMatrix(SimulateRoll(i, TypeShape,false)));
            string s1 = Render(shape_Clone);
            Debug.Log(s + "\n" + s1);

            if (s == s1)
            {
               
                Debug.Log("Roll Lan thu : " + i);
                return i;
            }
            
        }
        Debug.Log("KHong Co Type Shape !!!!");

        return 0;
    }
    public  static  int[,] standardizedMatrix(int[,] matrix)
    {
        int[,] clone = Shape.Clone(matrix);
        clone =  RemoveRow(clone);
        clone =  Shape.extendMatrix(clone);
        return clone;
    }
        
    public static int[,] SimulateRoll(int roll,TypeShape type,bool isRandom)
    {
       
        int r = 0;
        int[,] shape = null;
        int[,] Clone = null;
      
        switch (type)
        {
            case TypeShape.crossBar_1:
                shape = Shape.Clone(CtrlData.Cube_Cross_1);
             
                break;
            case TypeShape.crossBar_2:

                shape = Shape.Clone(CtrlData.Cube_Cross_2);
                r = Random.Range(0, CtrlData.Roll_Cross);
                break;
            case TypeShape.crossBar_3:


                shape = Shape.Clone(CtrlData.Cube_Cross_3);
                r = Random.Range(0, CtrlData.Roll_Cross);
                break;
            case TypeShape.crossBar_4:
                shape = Shape.Clone(CtrlData.Cube_Cross_4);
                r = Random.Range(0, CtrlData.Roll_Cross);
                break;
            case TypeShape.square:
                shape = Shape.Clone(CtrlData.Cube_Quare);
                break;
            case TypeShape.L_4_0:
                shape = Shape.Clone(CtrlData.Cube_L4_0);
                r = Random.Range(0, CtrlData.Roll_Cube_L);
                break;
            case TypeShape.L4_90:
                shape = Shape.Clone(CtrlData.Cube_L4_90);
                r = Random.Range(0, CtrlData.Roll_Cube_L);
                break;
            case TypeShape.L3_0:
                shape = Shape.Clone(CtrlData.Cube_L3_0);
                r = Random.Range(0, CtrlData.Roll_Cube_L);
                break;
            case TypeShape.L3_90:
                shape = Shape.Clone(CtrlData.Cube_L3_90);
                r = Random.Range(0, CtrlData.Roll_Cube_L);
                break;

            case TypeShape.three_cube:
                shape = Shape.Clone(CtrlData.Cube_3);
                r = Random.Range(0, CtrlData.Roll_Cube_L);
                break;

        }
     //   Debug.Log("Init : " + Render(shape));
        if (isRandom)
        {
           // Debug.Log("Roll  : " + Render(Shape.RotationMaxtrix(shape, roll)));
            return Shape.RotationMaxtrix(shape, r);
        }
        else
        {
         //   Debug.Log("Roll  : " + Render(Shape.RotationMaxtrix(shape, roll)));
            return  Shape.RotationMaxtrix(shape, roll);
        }
        
       
        return Clone;
       

       

    }
    public static int[,] SimulateRoll(int roll, TypeShape type, bool isRandom,out int indexroll)
    {
      
        int r = 0;
        int[,] shape = null;
        switch (type)
        {
            case TypeShape.crossBar_1:
                shape = CtrlData.Cube_Cross_1;

                break;
            case TypeShape.crossBar_2:

                shape = CtrlData.Cube_Cross_2;
                r = Random.Range(0, CtrlData.Roll_Cross);
                break;
            case TypeShape.crossBar_3:

                shape = CtrlData.Cube_Cross_3;
                r = Random.Range(0, CtrlData.Roll_Cross);
                break;
            case TypeShape.crossBar_4:
                shape = CtrlData.Cube_Cross_4;
                r = Random.Range(0, CtrlData.Roll_Cross);
                break;
            case TypeShape.square:
                shape = CtrlData.Cube_Quare;
                break;
            case TypeShape.L_4_0:
                shape = CtrlData.Cube_L4_0;
                r = Random.Range(0, CtrlData.Roll_Cube_L);
                break;
            case TypeShape.L4_90:
                shape = CtrlData.Cube_L4_90;
                r = Random.Range(0, CtrlData.Roll_Cube_L);
                break;
            case TypeShape.L3_0:
                shape = CtrlData.Cube_L3_0;
                r = Random.Range(0, CtrlData.Roll_Cube_L);
                break;
            case TypeShape.L3_90:
                shape = CtrlData.Cube_L3_90;
                r = Random.Range(0, CtrlData.Roll_Cube_L);
                break;

            case TypeShape.three_cube:
                shape = CtrlData.Cube_3;
                r = Random.Range(0, CtrlData.Roll_Cube_L);
                break;

        }
     
        indexroll = 0;
        if (isRandom)
        {
       //     Debug.Log("isRandom");
            indexroll = r;
            shape = Shape.RotationMaxtrix(shape, r);
        

        }
        else
        {
            Debug.Log("isRandom");
            shape = Shape.RotationMaxtrix(shape, roll);
         
        }
     //   Debug.Log("Render : " + Render(shape));
        return shape;




    }


    public static int[,] CloneBoard(int[,] board) 
    {
        int[,] clone = new int[board.GetLength(0), board.GetLength(1)];
        for(int i = 0; i < clone.GetLength(0); i++)
        {
            for(int j = 0; j < clone.GetLength(1); j++)
            {
                int x = board[i, j];
                clone[i, j] = x;
            }
        }
        return clone;

    }

    public void Reset_Game()
    {
        for(int i = 0; i < List_Shape.Count; i++)
        {
            List_Shape[i].DestroyAllCubeAndShape();
        }

    }
   
    public class InforShape
    {
        public TypeShape type;
        public Vector3 pos;
        public int[,] shape;
        public int roll;
        public int color;
       public InforShape(TypeShape type, Vector3 pos, int[,] shape,int roll,int color)
        {
            this.roll = roll;
            this.type = type;
            this.pos = pos;
            this.shape = shape;
            this.color = color;
        }
    }
    public bool isGameOver()
    {
        for(int i = 0; i < Board.GetLength(1);i++)
        {
            if (Board[0,i] == 1)
            {
                return true;
            }
        }
        return false;
    }
    public void CloneShape(Shape shape)
    {
        var a = Instantiate(shape.gameObject, shape.gameObject.transform.position, Quaternion.identity, null);
        string nameSpriteUse = a.GetComponent<Shape>().nameSpriteUse;
        a.GetComponent<Shape>().enabled = false;
        Color color = new Color(255, 255, 255, 100);
        Debug.Log(nameSpriteUse);
        a.transform.Find(nameSpriteUse).GetComponent<SpriteRenderer>().color = color;

      
        a.gameObject.AddComponent<DestroySelf1>();
        ShapeClone = a.GetComponent<Shape>();
        a.gameObject.layer = 11;


    }








    #endregion

    #region MoveDown

   
    public  void SortShape()
    {
        List<Shape> NewList = new List<Shape>();
        List<Shape> List = List_Shape;
        List<List<Shape>> ListRowShape = new List<List<Shape>>();
        for(int i = Row-1; i >= 0; i--)
        {
            List<Shape> Row = new List<Shape>();
            List<Shape> RowIsCheck = new List<Shape>();
            // Get Row 
            for (int j = 0; j < List.Count; j++)
            {
                if (List[j].Point.x == i)
                {
                    Row.Add(List[j]);
                    RowIsCheck.Add(List[j]);
                }
            }
           
           for(int z = 0; z < RowIsCheck.Count; z++)
            {
                List.Remove(RowIsCheck[z]);
            }
            ListRowShape.Add(Row);

        }
        for(int i = 0; i < ListRowShape.Count ; i++)
        {
           for(int j = 0; j < ListRowShape[i].Count; j++)
            {
                NewList.Add(ListRowShape[i][j]);
            }

        }

        List_Shape = NewList;



    }
    public void ContinueGame()
    {
        DestroyRow(8);
        DestroyRow(9);
        DestroyRow(10);

    }

    #endregion

}






