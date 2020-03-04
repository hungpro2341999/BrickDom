using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CtrlGamePlay : MonoBehaviour
{

    //Score:
    
    public int Round = 0;
    public int Combo = 0;
    public int HasCubeEff = 0;
    //
    public float timeSuggest = 10;
    public GameObject BoxSuggestRight;
    public GameObject BoxSuggestLeft;
    public GameObject DirectSuggset;
    public Transform transGameplay;
    public float TimeTurnToGray = 0.2f;
    public float TimeDestroy = 0.2f;
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
    public GameObject PrebCube;
    public List<GameObject> Cubes = new List<GameObject>();
    public List<Shape> List_Shape = new List<Shape>();
    public List<Shape> ListSplitShape = new List<Shape>();
    public Vector2 Destroy;
    public LayerMask LayerShape;
    public Shape ShapeClick = null;
 
    public float ClampY;
    public bool IsRandom = false;
    public List<Shape> ShapeClone = new List<Shape>();
    public GameObject SimulateColumn;
    public float SpeedMoveDown = 0;
    public Vector2 Point;
    public bool Watting = false;
    public float WaitTime = 0.4f;
    private float TimeWait;
    public float time;
    public bool Moving = false;
    public Vector2[] Direct = new Vector2[4]
    {
        new Vector2(0,1),
         new Vector2(1,0),
          new Vector2(0,-1),
           new Vector2(-1,0),
    };
    public bool isFinding = false;
    public List<GameObject> Suggest = new List<GameObject>();
    public List<Shape> CloneListDestroy = new List<Shape>();
    #region localVariable
    bool initPos = false;
    public Vector2 PosInit;
    public Vector2 PosCurr;
private float timeSugg = 0;
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
    public delegate void Revice();
    #endregion
    #region Action
    public event ClickDownHander Event_Click_Down;
    public event ClickUpHander Event_Click_Up;
    public event MoveDownComplete Event_Completed_Move_Down;
    public event StartGame Event_Start_Game;
    public event MoveDownComplete Event_Completed_Change;
    public event GameOver Event_Game_Over;
    public event Revice Event_Revice;

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
    public int[] rowDestroy;
    public int rotaion;
    public int farme = 1;
  
    public void SpawnInit()
    {
        for(int i = 0; i < 10; i++)
        {
          var a =   Poolers.Ins.GetObject(PrebShape,Vector2.zero,Quaternion.identity);
            a.gameObject.SetActive(true);
         
            Debug.Log("" + i);
          
        }
        for (int i = 0; i < Row*Column; i++)
        {
            var a = Poolers.Ins.GetObject(PrebCube, Vector2.zero, Quaternion.identity);
            a.gameObject.SetActive(true);

            Debug.Log("" + i);

        }
        Poolers.Ins.ClearAll();
    }
    // Start is called before the first frame update
     public void DestroyAll()
    {
     
        for (int i = 0; i < Suggest.Count; i++)
        {
            
            Suggest[i].GetComponent<DestroySelf1>().DestroyObj();
            
        }
        Suggest = new List<GameObject>();
    }
    public void DestroyShapeClone()
    {
        for (int i = 0; i < ShapeClone.Count; i++)
        {

          ShapeClone[i].GetComponent<DestroySelf1>().DestroyObj();

        }
        ShapeClone = new List<Shape>();
    }

    private void Awake()
    {
        Application.targetFrameRate = 60;
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
        Event_Start_Game += RefershBoard;
       
        Event_Start_Game += SpawnShape;
        Event_Completed_Change += SetUpMoveDown;
        Event_Game_Over += Reset_Game;




    }
    

    

    #region StatusGame



    public void Start_Game()
    {
        
        Event_Start_Game();
    }
    public void Over_Game()
    {
       
        //Event_Game_Over();
        GameManager.Ins.OpenWindow(TypeWindow.OverGame);
    }
   
    public void Rest_Game()
    {
        GameManager.Ins.isGameOver = false;
        GameManager.Ins.isGamePause = false;
        Board = new int[Row, Column];

        DestroyAll_Ver_1();
        GameManager.Ins.OpenWindow(TypeWindow.GamePlay);
      
        
    }
    
    public void ReviceGame()
    {


        int a = Random.Range(2, 5);
        int start = Random.Range(6, 9);
        for (int i = 0; i < a; i++)
        {

            DestroyRow(start);

            start--;
        }
        DestroyAndSplitShape();
        TimeWait = 0.4f;
      

        isClick_up = true;



        GameManager.Ins.isGameOver = false;
        GameManager.Ins.isGamePause = false;
        GameManager.Ins.OpenWindow(TypeWindow.GamePlay);




    }
   
    
    public void SetUpDestroyAllCube()
    {
        GameManager.Ins.CloseWindow(TypeWindow.Continue);
        List<List<Shape>> ShapeRow = SortListByRow(List_Shape);
        float totalTime = List_Shape.Count * TimeTurnToGray;
        for(int i = 0; i < ShapeRow.Count; i++)
        {
            float timedelay = i* TimeTurnToGray;
            for (int j = 0; j < ShapeRow[i].Count; j++)
            {
                ShapeRow[i][j].GrayShape(timedelay);
            }
          
        }
        Invoke("StartDestroyAllCube", totalTime);

    }

    

    public void StartDestroyAllCube()
    {
      
        List<List<Shape>> ShapeRow = SortListByRow(List_Shape);
        float totalTime = List_Shape.Count * TimeTurnToGray;
        for (int i = 0; i < ShapeRow.Count; i++)
        {
            float timedelay = i * TimeTurnToGray;
            for (int j = 0; j < ShapeRow[i].Count ; j++)
            {
                for(int z = 0; z < ShapeRow[i][j].ListShape.Count; z++)
                {
                    ShapeRow[i][j].ListShape[z].GetComponent<DestroySelf>().StartDestroy(timedelay);
                }
            }

        }

        Invoke("Over_Game", totalTime);
      
    }
    public void StartDestroyAllCube_Ver1()
    {
        //List<List<DestroySelf>> CubeSort = SortCube();
        //float totaltime = CubeSort.Count * TimeDestroy;
        //for(int i = 0; i < CubeSort.Count; i++)
        //{
        //    float delay = i * TimeDestroy;
        //    for(int j = 0; j < CubeSort[i].Count; j++)
        //    {
        //        CubeSort[i][j].StartDestroy(delay);
        //    }
        //}

        List<List<Shape>> ShapeRow = SortListByRow(List_Shape);
        float totalTime = List_Shape.Count * TimeTurnToGray;
        for (int i = 0; i < ShapeRow.Count; i++)
        {
            float timedelay = i * TimeTurnToGray;
            for (int j = 0; j < ShapeRow[i].Count; j++)
            {
                for (int z = 0; z < ShapeRow[i][j].ListShape.Count; z++)
                {
                    ShapeRow[i][j].ListShape[z].GetComponent<DestroySelf>().StartDestroy(timedelay);
                }
            }

        }

        Invoke("Start_Game", totalTime);

    }

    public void Reset()
    {
        GameManager.Ins.isGameOver = false;
        GameManager.Ins.CloseWindow(TypeWindow.Continue);
        List<List<Shape>> ShapeRow = SortListByRow(List_Shape);
        float totalTime = List_Shape.Count * TimeTurnToGray;
        for (int i = 0; i < ShapeRow.Count; i++)
        {
            float timedelay = i * TimeTurnToGray;
            for (int j = 0; j < ShapeRow[i].Count; j++)
            {
                ShapeRow[i][j].GrayShape(timedelay);
            }

        }
        Invoke("StartDestroyAllCube_Ver1", totalTime);
    }

    public List<List<DestroySelf>> SortCube()
    {
        List<List<DestroySelf>> SortShape = new List<List<DestroySelf>>(); 
        for (int i = 0; i < Board.GetLength(0); i++)
        {
            List<DestroySelf> RowCube = new List<DestroySelf>(); 
            for (int j = 0; j < Cubes.Count; j++) 
            {
                if (i == Cubes[j].GetComponent<DestroySelf>().Point.y)
                {
                    RowCube.Add(Cubes[j].GetComponent<DestroySelf>());
                }
            }
            if (RowCube.Count != 0)
            {
                SortShape.Add(RowCube);
            }
        }
        return SortShape;
       
    }
    

    #endregion
    void Start()
    {
        Application.targetFrameRate = 60;
    }



    // Update is called once per frame
    void Update()
    {
        if (GameManager.Ins.isGameOver || GameManager.Ins.isGamePause)
            return;

        Time.timeScale = 0.4f;

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
      
        if (Input.GetKeyDown(KeyCode.A))
        {
          
            bool FindOut = false;
            if (!StartSuggestionsLeft(FindOut))
            {
                SuggestRight(FindOut);
            }
           
        

        }
        if (Input.GetKeyDown(KeyCode.Q))
        {

            for(int i = 0; i < rowDestroy.Length; i++)
            {
                DestroyRow(rowDestroy[i]);
            }
          
            DestroyAndSplitShape();

         

        }
      
        if (ShapeClick != null)
        {

        }
        Matrix.text = Render((Board));

        if (Input.GetKeyDown(KeyCode.L))
        {



          Test();
           
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            
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

        if (timeSugg >= 0)
        {
            
            isFinding = false;
           
            timeSugg -= Time.deltaTime;

        }
        else
        {
            if (!isFinding)
            {
                bool FindOut = false;
                DestroyAll();
                if (!StartSuggestionsLeft(FindOut))
                {
                    SuggestRight(FindOut);
                    
                }
                isFinding = true;
            }
          
        }
        if (isClick_down)
        {
            DestroyAll();
            timeSugg = timeSuggest;
            RefershBoard();
           
            float dis = 0;
            // RefershBoard();
            if (ShapeClick != null)
            {
                Time.timeScale = 1;



                                   //Reset
                ShapeClick.PushToBoard();
                Event_Completed_Change();
                SimulateColumn.gameObject.SetActive(true);
                SimulateColumn.GetComponent<MoveFollowCube>().shape = ShapeClick;
                SimulateColumn.GetComponent<MoveFollowCube>().ScaleX(ScaleShape(ShapeClick));
                dis = PosMouseInit.x - Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
                ShapeClick.MoveTo(dis);
             
            }
            else
            {


              
            }
          

        }

        if (Input.GetMouseButtonUp(0))
        {
            
            if (!isClick_up)
            {
                if (ShapeClone != null)
                {
                    DestroyShapeClone();
                }
              
                Event_Click_Up();
                isClick_down = false;
              
                if (ShapeClick != null)
                {
                    timeSugg = timeSuggest;
                    ShapeClick.isClick = false;
                    Vector3 PointSnap = ShapeClick.transform.position;
                    int offset = Mathf.RoundToInt((PointSnap.x - CtrlGamePlay.Ins.initPoint.x) / offsetX);
                  
                    PointSnap.x = CtrlGamePlay.Ins.initPoint.x + offset * offsetX;
               //     Debug.Log(" Offset :" + offset +"  " + PointSnap.x);
                    ShapeClick.transform.position = PointSnap;
                    RefershBoard();
                    ShapeClick.ResetStatus();
                    if (offset != ShapeClick.PointInitCheck.y)
                    {

                        timeSugg = timeSuggest;
                        SimulateDown();
                        SetUpAll();
                        isClick_up = true;
                    }
                    else
                    {
                        isClick_down = false;
                        isClick_up = false;
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

            DestroyAll();
            if (IsListShapeMove())
            {
                AudioManganger.Ins.PlaySound("CompleteMove");
                RefershBoard();


                Debug.Log("Complete_Move");

                initPos = false;

                if (HasScore())
                {
                    if (HasEff())
                    {
                        AudioManganger.Ins.PlaySound("Electric");

                        TimeWait = 0.9f;
                        Invoke("DestroyAndSplitShape", 0.8f);
                        return;
                    }
                    else
                    {
                        DestroyAndSplitShape();
                        return;




                    }

                }
                else
                {

                    Round = 0;

                    isClick_up = false;
                    Combo = 0;
                }

                if (isGameOver())
                {
                    ResetStatus();
                    CtrlData.CountGame++;
                    CtrlData.CountPlay++;
                    GameManager.Ins.isGameOver = true;
                    if (CtrlData.CountGame % 3 == 0 || CtrlData.Score>150)
                    {
                        ManagerAds.Ins.ShowInterstitial();
                    }
                    if (CtrlData.CountPlay % 2 != 0)
                    {
                       

                        if (CtrlData.Ins.Set_High_Score(CtrlData.Score))
                        {
                            
                            GameManager.Ins.OpenWindow(TypeWindow.HighScore);
                        }
                        else
                        {
                           
                            GameManager.Ins.OpenWindow(TypeWindow.Continue);
                        }
                      
                    }
                    else
                    {
                      
                        if (CtrlData.Ins.Set_High_Score(CtrlData.Score))
                        {
                           
                            GameManager.Ins.OpenWindow(TypeWindow.HighScore);
                        }
                        else
                        {
                           
                            SetUpDestroyAllCube();
                        }
                      
                    }
                  
                  
                }
                else
                {
                    Event_Completed_Move_Down();
                }







            }

        }
        //////////////                MOVE DOWN  ///////////////////////
        ///

     



        
       
    }
    public void ResetStatus()
    {
      
            if (ShapeClone != null)
            {
                DestroyShapeClone();
            }

            isClick_down = false;

            if (ShapeClick != null)
            {
                timeSugg = timeSuggest;
                ShapeClick.isClick = false;
                Vector3 PointSnap = ShapeClick.transform.position;
                int offset = Mathf.RoundToInt((PointSnap.x - CtrlGamePlay.Ins.initPoint.x) / offsetX);

                PointSnap.x = CtrlGamePlay.Ins.initPoint.x + offset * offsetX;

                ShapeClick.transform.position = PointSnap;
                ShapeClick.ResetStatus();
               

              
                ShapeClick = null;



            }
        SimulateColumn.gameObject.SetActive(false);
    }
    
    public void DestroyAndSplitShape()
    {
        CheckDestroyRow();
        RefershBoard();
        SplitShape();
      
        TimeWait = WaitTime;
    }

    public bool HasScore()
    {
        Round++;
        
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
            List_Shape[i].NormlizeColor();
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

        ReflectShape();
        Debug.Log("-----------SPLIT SHAPE----------");
       
        List<Shape> listSplit = GetListShapeSplit(List_Shape);
        for (int i = 0; i < listSplit.Count; i++)
        {
            listSplit[i].ReflectShape();
            SplitShape(listSplit[i]);
          
        }
       // ReflectShape();






    }
    public  List<Shape> GetListShapeSplit(List<Shape> Lshape)
    {
        List<Shape> split = new List<Shape>();
        for(int i = 0; i < Lshape.Count; i++)
        {
            if (!Lshape[i].HasCut())
            {
                split.Add(Lshape[i]);
            }
        }
        return split;
    }

    public void Init_Mouse_Down()
    {
        DestroyAll();
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
             CloneShape(ShapeClick);
             DestroyAll();

        }
    }
    public bool HasEff()
    {
        bool eff = false;
        int[] row;
        DestroyAtRow(out row);
        

        
        for(int i = 0; i < row.Length; i++)
        {
            if (RowHasEff(row[i]))
            {
                eff = true;
            }
        }
        return eff;
     
    }
    public int CountEff()
    {
        int count = 0;
        bool eff = false;
        int[] row;
        DestroyAtRow(out row);



        for (int i = 0; i < row.Length; i++)
        {
            if (RowHasEff(row[i]))
            {
                count++;
            }
        }
        return count;
    }

    public bool RowHasEff(int row)
    {
        bool eff = false;
        Shape shape;
        for (int i = 0; i < Column; i++)
        {
            for (int x = 0; x < Cubes.Count; x++)
            {

                if (Cubes[x].GetComponent<DestroySelf>().Point == new Vector2((float)i, row))
                {
                    shape = Cubes[x].GetComponent<DestroySelf>().shape;

                    if (shape.isEff)
                    {
                        Cubes[x].GetComponent<DestroySelf>().StartSuperEff();
                        HasCubeEff++;
                        eff = true;
                    }


                }
            }
        }
        return eff;
    }

    public void CheckDestroyRow()
    {
        int[] row;
        int Score;
        if (DestroyAtRow(out row))
        {

            for (int i = 0; i < row.Length; i++)
            {
                Debug.Log("DESTROY ROW :" + i);
                DestroyRow(row[i]);
            }
            AudioManganger.Ins.PlaySound("Breaker");

        }
        else
        {
            Debug.Log(" NOTTHING ");
        }
    
        if(row !=null)
        {

          

            if (row.Length>0)
            {
                Combo += row.Length;
                if ( Combo > 1)
                {
                    int x = Mathf.Clamp(Combo, 1, 4);
                    string audio = "Score_"+x.ToString();

                    AudioManganger.Ins.PlaySound(audio);

                    CtrlData.Ins.VisibleCombo(Combo);

                    CtrlData.Ins.VisibleScore(14*(Combo+HasCubeEff));

                    CtrlData.Score += 14 * (Combo+HasCubeEff);

                    CtrlData.Ins.SetScore();

                }
                else
                {
                    
                    CtrlData.Ins.VisibleScore(14+(HasCubeEff*7));
                    CtrlData.Score += 14+(HasCubeEff*7);
                    CtrlData.Ins.SetScore();
                }
              
            }
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
              //  ShapeSelect.text = shape.gameObject.name;
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
        HasCubeEff = 0;

        if (CtrlData.Score > (150 * (int)CtrlData.Level))
        {
            CtrlData.Level++;
        }

        GenerateStartGame(Random.Range(2,3+Mathf.Clamp(Random.Range(0,CtrlData.Level),0,5)),false);
      //  GenerateStartGame(2, false);
        RefershBoard();
    }
    public void SpawnStartGame()
    {
        timeSuggest = 10;
        GenerateStartGame(Random.Range(2,4), true);
        //GenerateStartGame(2, true);
        RefershBoard();
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
                  

                 
                    Cubes[x].GetComponent<DestroySelf>().Destroy();
                  
                      


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

      //  MoveSelect.text = "LEFT : " + minX + "RIGHT :  " + maxX + "\n";
        float ClampMinX = minX * offsetX;
        float ClampMaxX = maxX * offsetY;

     //   MoveSelect.text += ClampMinX + " :: " + ClampMaxX;
        shape.SetUpClamp(ClampMinX, ClampMaxX);

      //  MoveSelect.text = s;
       // Debug.Log(s);


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


     
    //    Debug.Log("INIT_1 : " +shape.name+ "\n" + Render(shape.shape));
      
     
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
       
            if (hasCut)
            {
                Debug.Log("CUT : " + shape.name);

                ShapeSplit.Add(GrounpRow);
      //          Debug.Log(RenderList(GrounpRow));
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

               //     s +="\n" +  Render(ListToMatrix(shape_Split)) ;
               //     Debug.Log(s);
                    SpawnShape(ListToMatrix(shape_Split), pos,Color);
                 
                      
                }
                  shape.DestroyAllCubeAndShape();
               

            }
            else
            {
                Debug.Log("NOT  CUT : " + shape.name);
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

        InforShape infor = new InforShape(type, pos, Type, roll,color);

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
    public void DestroyAll_Ver_1()
    {

        for (int i = 0; i < Board.GetLength(0); i++)
        {
            DestroyRow(i);
        }
        List_Shape = new List<Shape>();
        Cubes = new List<GameObject>();
        Board = new int[Row, Column];
        Event_Start_Game();
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
            string s  =  CtrlData.Ins.GetSimulateRoll(type,i);
           //  Render(Shape.SplitMatrix(SimulateRoll(i,type,false)));
           //  Debug.Log(s1 + "\n" + s);
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
    //    Debug.Log("Nornal : \n" + Render(Board));
      //  Debug.Log("RESULT MOVE : \n" + Render(shape));
        for (int i = 0; i < List_Shape.Count; i++)
        {
          
           MoveDownOneCube(List_Shape[i],ListMove[i]);
        }

        SetUpAll();


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
        if(!StartMove(shape, Board))
        {
            return false;
        }
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

    public List<List<Vector2>> ShapeNotActive(List<Shape> list_shape)
    {
        List<List<Vector2>> ShapeNotActive = new List<List<Vector2>>();
        for (int i = 0; i < list_shape.Count; i++)
        {
            if (list_shape[i].gameObject.layer != 8)
            {
                List<Vector2> shape = new List<Vector2>();
                for (int j = 0; j < list_shape[i].ListShape.Count; j++)
                {
                   shape.Add(list_shape[i].ListShape[j].GetComponent<DestroySelf>().Point);
                }
                if (shape.Count != 0)
                {
                    ShapeNotActive.Add(shape);
                }
            }
           
        }
      //  Debug.Log(ShapeNotActive.Count);
        return ShapeNotActive;

    }


    public List<List<Shape>> SortListByRow(List<Shape> list_shape) 
    {
        List<List<Shape>> ListShape = new List<List<Shape>>();

        for (int i = 0; i < list_shape.Count; i++)
        {
            List<Shape> shape = new List<Shape>();
            for (int j = 0; j < list_shape.Count; j++)
            {
                if (i == list_shape[j].Point.x)
                {
                    shape.Add(list_shape[j]);
                }


            }
            ListShape.Add(shape);
        }
        return ListShape;
    }
    public bool StartMove(List<Vector2> Shape,int[,] Board)
    {
        for(int i = 0; i < Shape.Count; i++)
        {
            
            Vector2 point = Shape[i];
            if (!isInMatrixBoard((int)point.y,(int)point.x,Board))
            {
                return false;
            }
                Board[(int)point.y, (int)point.x] = 0;
        }
        return true;
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
    public void MoveDownOneCube(Shape shape, int Space)
    {

        //float dis = 0;
        //float space = offsetX / 3;

        //float clampy = shape.transform.position.y - (Space * offsetX);
        //if (Space != 0)
        //{



        //    while (shape.transform.position.y >= clampy + (offsetX * 0.1f))
        //    {
        //        Debug.Log("isMoving");
        //        shape.isMovingDown = true;
        //        Vector3 pos = shape.transform.position;

        //        pos.y = Mathf.Lerp(pos.y, clampy, Time.deltaTime * SpeedMoveDown);

        //        shape.transform.position = pos;



        //        yield return new WaitForSeconds(0);
        //    }

        //    shape.isMovingDown = false;



        //    int offset = Mathf.RoundToInt((initPoint.y - shape.transform.position.y) / offsetY);
        //    shape.transform.position = new Vector2(shape.transform.position.x, initPoint.y - offset * offsetY);
        //    shape.isMovingDown = false;
        //    yield return new WaitForSeconds(0);


        //}
    
            MovingDown(shape, shape.transform.position.y - (Space * offsetX));
     
          

        
           
    }
   

    public void MovingDown(Shape shape, float ClampY)
    {
        shape.isMovingDown = true;
        shape.ClampMoveDown = ClampY;
        
    }


    #endregion

    #region StartGame
    public void GenerateStartGame(int index,bool Start)
    {
        int[,] backup = CtrlGamePlay.CloneBoard(Board);
        int[,] CloneBoard = CtrlGamePlay.CloneBoard(Board);
    //    Debug.Log("Board Clone : "+"\n"+ Render(CloneBoard));

        bool isCorrect = false;
        List<InforShape> InforShape = new List<InforShape>();
        int cont = 0;
       
       //     Debug.Log("Spawn Again ");
        
            
           
              
                    Vector3 pos;
                    TypeShape type =Shape.RandomShape();
                    int[,] shape = null;
                    int roll = 0;

                if (Start)
                {
                    
                    bool spawn = false;
                    int count = 0;
                    while (count<index)
                    {
                if (time % 7 == 0)
                {
                    type = Shape.RandomShape();
                }
                        type = Shape.RandomShape();
                        pos = CtrlGamePlay.Ins.RandomPosStart();
                        if (isSpawnCorrect_For_Start_Game(ref CloneBoard, type, pos, out shape, out roll))
                        {
                                   
                            int color = CtrlData.RandomColor(type, roll);
                            InforShape infor = new InforShape(type, pos, shape, roll, color);
                      //          Debug.Log("Spawn SS : " + count);
                     //           Debug.Log("Board SS : " + "\n" + Render(CloneBoard));

                            InforShape.Add(infor);
                            count++;
                    time = 0;
                        }
                else
                {
                    time++;
                    ////InforShape.Clear();
                    //CloneBoard = CtrlGamePlay.CloneBoard(Board);
                    ////isCorrect = false;
                }
            }


        }
                else
                {
                   int timeSpawn=0;
                     int count = 0;
                     int time = 0;
            while (count < index)
            {
                timeSpawn++;
                if (time % 7 == 0)
                {
                    type = Shape.RandomShape();
                }
              
                pos = CtrlGamePlay.RandomPosShape();
                if (isSpawnCorrect(ref CloneBoard, type, pos, out shape, out roll))
                {
                    int color = CtrlData.RandomColor(type, roll);
                    InforShape infor = new InforShape(type, pos, shape, roll, color);
                    //    Debug.Log("Spawn SS : " + i);
                    //     Debug.Log("Board SS : " + "\n" + Render(CloneBoard));
                    InforShape.Add(infor);
                    count++;
                    time = 0;
                }
                else
                {
                    time++;
                    //InforShape.Clear();
                    //CloneBoard = CtrlGamePlay.CloneBoard(Board);
                    //isCorrect = false;
                }
                if (timeSpawn >= 200)
                {
                    break;
                }
                
            }

                }


          
            isCorrect = true;


        
        //   Debug.Log("SPAWWN :" +InforShape.Count);
       
        SpawnShape(InforShape,Start);
    }
    public void SpawnShape(List<InforShape> ListInfor,bool isActive)
    {

        for(int i = 0; i < ListInfor.Count; i++)
        {
            var a = Poolers.Ins.GetObject(CtrlGamePlay.Ins.PrebShape, ListInfor[i].pos, Quaternion.identity);

            a.GetComponent<Shape>().OnSpawn();
          //  var a = Instantiate(PrebShape, ListInfor[i].pos, Quaternion.identity,transGameplay);
            a.GetComponent<Shape>().isCubeStart = isActive;
            
            a.GetComponent<Shape>().SetTypeShape(ListInfor[i].type, ListInfor[i].shape);
          
            a.GetComponent<Shape>().Set_Up_Corrs(ListInfor[i].roll, ListInfor[i].type,ListInfor[i].color);

            a.GetComponent<Shape>().PushToBoard();
            a.GetComponent<Shape>().InitImageUse();
            if (isActive)
            {
                a.GetComponent<Shape>().NormlizeColor();
            }
            else
            {
                a.GetComponent<Shape>().Fade();
            }

            if(ListInfor[i].type == TypeShape.crossBar_1 || ListInfor[i].type == TypeShape.crossBar_2 || ListInfor[i].type == TypeShape.crossBar_3)
            {
                int x = Random.Range(0, 100);
                if(x>=0 && x <= 3)
                {
                    if (ListInfor[i].roll == 0)
                    {
                        
                        a.GetComponent<Shape>().isEff = true;
                        var aa = a.GetComponent<Shape>().SpriteUse.gameObject.GetComponent<_2dxFX_GoldFX>();
                        if (aa != null)
                        {
                            aa.enabled = true;
                        }
                        else
                        {
                            a.GetComponent<Shape>().SpriteUse.gameObject.AddComponent<_2dxFX_GoldFX>();
                        }
                           
                    }
                   
                }
               



            }

            idShape++;
         //   a.GetComponent<Shape>().GetComponent<Rigidbody2D>().mass = (100000 - idShape*2);
            a.name = "Shape : " + idShape;
            List_Shape.Add(a.GetComponent<Shape>());
            if (isActive)
            {
              
                a.gameObject.layer = 8;
              
                for (int j = 0; j<Cubes.Count; j++)
                {
                
                    Cubes[i].layer = 8;
                }

            }
            else
            {
                a.GetComponent<Shape>().FadeShape();
            }
          
        }
     
    }
    public void SpawnShape(InforShape ListInfor)
    {
        //  var a = Instantiate(PrebShape, ListInfor.pos, Quaternion.identity, null);
        var a = Poolers.Ins.GetObject(PrebShape, ListInfor.pos, Quaternion.identity);
        a.OnSpawn();
        a.GetComponent<Shape>().SetTypeShape(ListInfor.type, ListInfor.shape);
        a.GetComponent<Shape>().Set_Up_Corrs(ListInfor.roll, ListInfor.type,ListInfor.color);
        idShape++;
        a.name = "Shape : " + idShape;
        List_Shape.Add(a.GetComponent<Shape>());
        a.GetComponent<Shape>().ActiveShape();
        a.GetComponent<Shape>().NormlizeColor();
    }

    public void Test()
    {

        //int i = 3;
        int i = 3;
        int[,] shape = Shape.RotationMaxtrix(CtrlData.Cube_3, i);
        shape = Shape.SplitMatrix(shape);
        int color = CtrlData.RandomColor(TypeShape.three_cube , i);
        InforShape infor = new InforShape(TypeShape.three_cube, CtrlGamePlay.RandomPosShape(),shape, i,color);
        List<InforShape> ListInfor = new List<InforShape>();
        ListInfor.Add(infor);
      
        SpawnShape(ListInfor,false);
    }


    
    public bool isSpawnCorrect(ref int[,] Board,TypeShape type,Vector3 pos,out int[,] shapeCorrect,out int Roll)
    {
        int indexRoll = 0;
        int[,] Clone =  CloneBoard(Board);
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
                    if (IsPushShapeCorrect(Clone,(int)point.x, (int)point.y))
                    {
                        Clone[(int)point.x, (int)point.y] = 1;
                      
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
        Board = Clone;
        shapeCorrect = shape;
        return true;
                   
             
    }
  
    public bool isSpawnCorrect_For_Start_Game(ref int[,] Board, TypeShape type, Vector3 pos, out int[,] shapeCorrect, out int Roll)
    {
        int[,] Clone = CloneBoard(Board);
        int indexRoll = 0;
        int[,] shape = SimulateRoll(0, type, true, out indexRoll);
        Roll = indexRoll;
        shape = Shape.SplitMatrix(shape);
    //      Debug.Log("Board :::: " +Render(Clone));
     //       Debug.Log(Render(shape));
        int Ground = 0;
        for (int i = 0; i < shape.GetLength(0); i++)
        {
            for (int j = 0; j < shape.GetLength(1); j++)
            {
                if (shape[i, j] != 0)
                {
                    Vector3 posCurr = new Vector3(pos.x + j * CtrlGamePlay.Ins.offsetX, pos.y - i * CtrlGamePlay.Ins.offsetY);

                    Vector2 point = CtrlGamePlay.PositonToPointMatrix(posCurr.x, posCurr.y);
                   //    Debug.Log(point);
                    if (IsPushShapeCorrect(Clone, (int)point.x, (int)point.y))
                    {
                        Clone[(int)point.x, (int)point.y] = 1;
                        if(isGround((int)point.x))
                        {
                            Ground++;
                        }

                    }
                    else
                    {
                        shapeCorrect = null;
                     //     Debug.Log("Spawn False : " + point.x +" :: "+point.y);
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
            Board = Clone;
      //      Debug.Log("CLONE : " + Render(Clone));
        //    Debug.Log("CURR  : " + Render(Board));
            shapeCorrect = shape;
            return true;
        }
      
       

    }
    //public void  PutToBoard(int[,] Board,int[,] shape)

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

        string s1 = Render(shape_Clone);
        for (int i = 0; i < roll; i++)
        {

            //  string s  = Render(Shape.SplitMatrix(SimulateRoll(i, TypeShape,false)));
            string s = CtrlData.Ins.GetSimulateRoll(TypeShape, i);
            //  Debug.Log(s + "\n" + s1);

            if (s == s1)
            {
               
             //   Debug.Log("Roll Lan thu : " + i);
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
            case TypeShape.T:
                shape = CtrlData.T;
                r = Random.Range(0, CtrlData.Roll_Cube_L);

                // YOU NEVER SPAWn
                if (r == 0)
                {
                    r = 1;
                }
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
        Reset();

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
        a.GetComponent<Shape>().FadeShape();
       
      
      for(int i = 0; i < a.GetComponent<Shape>().ListShape.Count; i++)
        {
            a.GetComponent<Shape>().ListShape[i].gameObject.layer = 11;
        }
        a.GetComponent<Shape>().enabled = false;
        a.gameObject.AddComponent<DestroySelf1>();
        ShapeClone.Add(a.GetComponent<Shape>());
        a.gameObject.layer = 11;



    }
    public void CloneShape(Shape shape,float time)
    {
        //var a = Instantiate(shape.gameObject, shape.gameObject.transform.position, Quaternion.identity, null);
        //a.GetComponent<Shape>().FadeShape();


        //for (int i = 0; i < a.GetComponent<Shape>().ListShape.Count; i++)
        //{
        //    a.GetComponent<Shape>().ListShape[i].gameObject.layer = 11;
           
        //}
        //a.GetComponent<Shape>().enabled = false;
        //a.gameObject.AddComponent<DestroySelf1>();
       
        //ShapeClone.Add(a.GetComponent<Shape>());
        //a.gameObject.layer = 11;

        //a.gameObject.GetComponent<DestroySelf1>().StartDelayDestroy(0.1f);


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
            Row = SortRowShape(Row);
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


    public List<Shape> SortRowShape(List<Shape> shape)
    {
        List<Shape> SortList = new List<Shape>();
        List<Shape> Sort_0 = new List<Shape>();
        List<Shape> Sort_1 = new List<Shape>();
      
       
        List<List<Vector2>> Shapes = PushListCubeInList(shape);
        for(int i = 0; i < Shapes.Count; i++)
        {
            if (shape[i].shape.GetLength(0)!=1)
            {
                Sort_1.Add(shape[i]);
            }
            else
            {
                Sort_0.Add(shape[i]);
            }

        }

        for(int i = 0; i < Sort_1.Count; i++)
        {
            SortList.Add(Sort_1[i]);
        }
        for (int i = 0; i < Sort_0.Count; i++)
        {
            SortList.Add(Sort_0[i]);
        }
        return SortList;

    }

    
   

    
    public bool StartSuggestionsLeft(bool FindOut)
    {
      

        SortShape();

        List<Shape> ListShapeMove = new List<Shape>();
        int[,] shape = CloneBoard(Board);
        List<List<Vector2>> Shapes = PushListCubeInList(List_Shape);
        List<List<Vector2>> ShapesNotActive = ShapeNotActive(List_Shape);


      
            for (int i=0;i< ShapesNotActive.Count;i++)
            {
            string s = "";
            s += RenderListShape(ShapesNotActive[i]) + "\n";
           //     Debug.Log(s);
            }
        Debug.Log("///////////////////");
        for (int i = 0; i < Shapes.Count; i++)
        {
            string s = "";
            s += RenderListShape(Shapes[i]) + "\n";
         //   Debug.Log(s);
        }

     //   Debug.Log("LEFT  MOVE "+ ShapesNotActive.Count);
        for (int i = 0; i < Shapes.Count; i++)
        {
            if (IsInList(ShapesNotActive, Shapes[i]))
            {
            //    Debug.Log("yes");
            }
            else
            {
             //   Debug.Log("no");
            }


        }


        for (int i = 0; i < Shapes.Count; i++)
        {

            if (IsInList(ShapesNotActive, Shapes[i]))
                continue;
            if (FindOut)
                continue;   
            List<List<Vector2>> ListShapeGame = CloneAllListShape(Shapes);

            int[,] BoardClone = CloneBoard(Board);
            //Debug.Log("_________________________________");
            //Debug.Log("BOARD_CURR : \n " + Render(BoardClone));
            //Debug.Log("_________________________________");
            List<Vector2> Cube = ListShapeGame[i];

            Vector2 point = SimulateMoveLeftRight(CloneShapeList(Cube), CloneBoard(BoardClone));
            string s = "";

            //for (int z = 0; z < Cube.Count; z++)
            //{
            //    s += "  " + Cube[z].x + "   " + Cube[z].y + " \n";
            //}
            //Debug.Log("Shape Curennt : " + s);
            int count = 1;
            for (int left = 1; left <= point.x; left++)
            {
              

                InputToBoard(Cube, BoardClone, -1);
                //Debug.Log("CHAGE _ BOARD : \n " + Render(BoardClone));
                if (Suggestions(CloneBoard(BoardClone), CloneAllListShape(ListShapeGame)))
                {
                    Debug.Log("FIND OUNT LEFT: " + left);
                    if (FindOut)
                    {
                        continue;
                    }
                    Suggeset_Ver_2(List_Shape[i],left,false);
                    
                 
               //     Debug.Log("FIND OUTTTTTTTTTTTTT !!!!!!!!!!!!");
                    return true;
                   

                }
            }

        }
        return false;

     

      


    }
    public void SuggestRight(bool FindOut)
    {
      //  Debug.Log("RIGHT MOVE ");

        SortShape();

        List<Shape> ListShapeMove = new List<Shape>();
        int[,] shape = CloneBoard(Board);
        List<List<Vector2>> Shapes = PushListCubeInList(List_Shape);
        List<List<Vector2>> ShapesNotActive = ShapeNotActive(List_Shape);



        for (int i = 0; i < ShapesNotActive.Count; i++)
        {
            string s = "";
            s += RenderListShape(ShapesNotActive[i]) + "\n";
            //     Debug.Log(s);
        }
     //   Debug.Log("///////////////////");
        for (int i = 0; i < Shapes.Count; i++)
        {
            string s = "";
            s += RenderListShape(Shapes[i]) + "\n";
            //   Debug.Log(s);
        }

      //  Debug.Log("LEFT  MOVE " + ShapesNotActive.Count);
        for (int i = 0; i < Shapes.Count; i++)
        {
            if (IsInList(ShapesNotActive, Shapes[i]))
            {
                //    Debug.Log("yes");
            }
            else
            {
                //   Debug.Log("no");
            }


        }


        for (int i = 0; i < Shapes.Count; i++)
        {

            if (IsInList(ShapesNotActive, Shapes[i]))
                continue;
            if (FindOut)
                continue;
            List<List<Vector2>> ListShapeGame = CloneAllListShape(Shapes);

            int[,] BoardClone = CloneBoard(Board);
            //Debug.Log("_________________________________");
            //Debug.Log("BOARD_CURR : \n " + Render(BoardClone));
            //Debug.Log("_________________________________");
            List<Vector2> Cube = ListShapeGame[i];

            Vector2 point = SimulateMoveLeftRight(CloneShapeList(Cube), CloneBoard(BoardClone));
            string s = "";

            //for (int z = 0; z < Cube.Count; z++)
            //{
            //    s += "  " + Cube[z].x + "   " + Cube[z].y + " \n";
            //}
            //Debug.Log("Shape Curennt : " + s);
            int count = 1;
            for (int left = 1; left <=point.y; left++)
            {
                if (FindOut)
                {
                    continue;
                }
                InputToBoard(Cube, BoardClone, 1);
              //  Debug.Log("CHAGE _ BOARD : \n " + Render(BoardClone));
                if (Suggestions(CloneBoard(BoardClone), CloneAllListShape(ListShapeGame)))
                {
                    if (FindOut)
                    {
                        continue;
                    }
                    FindOut = true;
                    Suggeset_Ver_2(List_Shape[i],left,true);
             //       Debug.Log("FIND OUNT RIGHT : " + left);
               //     Debug.Log("FIND OUTTTTTTTTTTTTT !!!!!!!!!!!!");
                    count++;
                   

                }
            }

        }
    }
   
    
    public string  RenderListShape(List<Vector2> shape)
    {
        string s = "";
        for(int i = 0; i < shape.Count; i++)
        {
            s+=shape[i].x +"  "+shape[i].y+"\n";
        }
        return s;
        
    }

    public bool IsInList(List<List<Vector2>> ListShape, List<Vector2> shape)
    {
        bool HasIn = false;
        for(int i = 0; i < ListShape.Count; i++)
        {
         if(shape.Count == ListShape[i].Count)
            {
                for (int j = 0; j < shape.Count; j++)
                {
                    if (shape[j].x == ListShape[i][j].x && shape[j].y == ListShape[i][j].y)
                    {
                        return true;
                    }
                }
            }
            
        }
        return false;
       
    }

    public  List<List<Vector2>> CloneListShape(List<List<Vector2>> listShape)
    {
        List<List<Vector2>> ListClone = new List<List<Vector2>>(listShape);

        return ListClone;
    }
   public void InputToBoard(List<Vector2> shape, int[,] Board,int Space)
    {
        for (int i = 0; i < shape.Count; i++)
        {
          

            Vector2 point = shape[i];

           
          //  Debug.Log(point.y + "  " + point.x);
            Board[(int)point.y, (int)point.x] = 0;
            
        }
        for (int i = 0; i < shape.Count; i++)
        {
            Vector2 Point = shape[i];
            Point.x  += Space;

            shape[i] = Point;
        }


        for (int i = 0; i < shape.Count; i++)
        {

            Vector2 Point = shape[i];

            Board[(int)Point.y, (int)Point.x] = 1;
           
        }
      
    }
   

    public bool Suggestions(int[,] Board,List<List<Vector2>> List_Shape)
    {
       

    //    Debug.Log("Nornal : \n" + Render(Board));
        SortShape();
      
        while (isCheckDown(CloneBoard(Board), 1,List_Shape))
            for (int i = 0; i < List_Shape.Count; i++)
            {
                //     Debug.Log(" : " + i + "  :" + Render(shape));
                if (isMoveDown(Board, 1, List_Shape[i]))
                {
                  
                    SimulateMoveDown(List_Shape[i], 1, Board);
                    //  ListShapeMove.Add(List_Shape[i]);
                    //  Debug.Log(" : " + i + "  :" + Render(shape));
                }
                else
                {
                    ResetMove(List_Shape[i], Board);
                }


            }

       
        //  Debug.Log("CANVAS : " + Render(shape));
      
    //    Debug.Log("RESULT MOVE : \n" + Render(Board));
        

        return IsHasSuggest(Board);


    }

    public List<List<Vector2>> CloneAllListShape(List<List<Vector2>> ListShape)
    {
        List<List<Vector2>> list = new List<List<Vector2>>();

        for(int i = 0; i < ListShape.Count; i++)
        {
            List<Vector2> Shape = new List<Vector2>();
            for(int j = 0; j < ListShape[i].Count; j++)
            {
                Vector2 Point = ListShape[i][j];
                Shape.Add(Point);
            }
            list.Add(Shape);
            
        }
        return list;
    }
  
    private bool InputToBoard(List<Vector2> shape, int[,] Board)
    {
        for (int i = 0; i < shape.Count; i++)
        {
            int row = (int)shape[i].y;
            int col = (int)shape[i].x;
           if(isInMatrix(row,col,Board))
            {
              
                if (Board[row, col] == 1)
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

    

 
    public Vector2 SimulateMoveLeftRight(List<Vector2> shape,int[,] Board)
    {
        int left  = 0;
        int right = 0;
        string s = "";

        for(int i = 0; i < shape.Count; i++)
        {
            s += "  " + shape[i].x + "   " + shape[i].y + " \n";
        }

        List<Vector2> BackUp = CloneShapeList(shape);
         
        for (int i = 0; i < shape.Count; i++)
        {
            Vector2 point = shape[i];
            Board[(int)point.y, (int)point.x] = 0;

        }
        // Right
        while (InputToBoard(shape, Board))
        {
            for(int i = 0; i < shape.Count; i++)
            {
                Vector2 Point = shape[i];
                Point.x++;
                shape[i] = Point;
            }
            right++;
        }
        right--;
        shape = BackUp;
        for (int i = 0; i < shape.Count; i++)
        {
            Vector2 point = shape[i];
            Board[(int)point.y, (int)point.x] = 0;

        }
        while (InputToBoard(shape, Board))
        {
            for (int i = 0; i < shape.Count; i++)
            {
                Vector2 Point = shape[i];
                Point.x--;
                shape[i] = Point;
            }
            left++;
        }
        left--;
        // Left

        shape = BackUp;
        s+="\n" +" " + left + "  " + right;
    //    Debug.Log(s);
        return new Vector2(left,right);

      
    }
    public List<Vector2> CloneShapeList(List<Vector2> shape)
    {
        List<Vector2> list = new List<Vector2>();
        for(int i = 0; i < shape.Count; i++)
        {
            Vector2 Point = shape[i];
            list.Add(Point);
        }
        return list;
    }


    public bool IsHasSuggest(int[,] Board)
    {
        for(int i = 0; i < Board.GetLength(0); i++)
        {
            int count = Board.GetLength(1);
            for (int j = 0; j < Board.GetLength(1); j++)
            {

                if (Board[i, j] == 1)
                {
                    count--;
                }

            }
            if (count == 0)
            {
                return true;
            }
        }
        return false;
    }
    public void Suggeset(Shape shape,int end,bool Right)
    {
        int Pointend;
        if (Right)
        {
            Pointend  = (int)shape.Point.y + end;
        }
        else
        {

            Pointend = (int)shape.Point.y - end;
        }
        
        Vector2 PosStart = shape.transform.position;
        float x = CtrlGamePlay.Ins.initPoint.x + Pointend * CtrlGamePlay.Ins.offsetX;
        

        Vector2 PosEnd = new Vector2(x, PosStart.y);
        
        var a = Instantiate(shape.gameObject, shape.transform.position, Quaternion.identity, transGameplay);
        a.gameObject.layer = 11;
        for(int i = 0; i < a.GetComponent<Shape>().ListShape.Count; i++)
        {
            a.GetComponent<Shape>().ListShape[i].gameObject.layer = 11;
            a.GetComponent<Shape>().ListShape[i].GetComponent<BoxCollider2D>().enabled = false;
        }
        a.GetComponent<Shape>().enabled = false;
        a.AddComponent<LoopMove>();
        a.AddComponent<DestroySelf1>();
        Suggest.Add(a);
        a.GetComponent<LoopMove>().SetUp(PosStart, PosEnd,Mathf.Abs(end));
        





    }
    public void Suggeset_Ver_2(Shape shape, int end, bool Right)
    {
        int Pointend;
        if (Right)
        {
            Pointend = (int)shape.Point.y + end;
        }
        else
        {

            Pointend = (int)shape.Point.y - end;
        }

        Vector2 PosStart = shape.transform.position;
        float x = CtrlGamePlay.Ins.initPoint.x + Pointend * CtrlGamePlay.Ins.offsetX;


        Vector2 PosEnd = new Vector2(x, PosStart.y);

        var a = Instantiate(shape.gameObject, shape.transform.position, Quaternion.identity, transGameplay);
        if (Right)
        {
            var b = Instantiate(BoxSuggestRight, shape.transform.position, Quaternion.identity, null);
       //   b.transform.localScale = new Vector2(shape.shape.GetLength(1) + end, shape.shape.GetLength(0));
            b.transform.Find("Box/box_hint").GetComponent<SpriteRenderer>().size = new Vector2(-(shape.shape.GetLength(1) + end), shape.shape.GetLength(0));
            b.AddComponent<DestroySelf1>();
            b.gameObject.layer = 11;
           
            Suggest.Add(b);
            var c = Instantiate(DirectSuggset,GetPosition(shape.ListShape,shape.shape,true), Quaternion.identity, null);
            c.AddComponent<DestroySelf1>();
            c.GetComponent<SpriteRenderer>().flipX = true;
            c.AddComponent<LoopMove>();
            c.GetComponent<LoopMove>().SetUp(c.transform.position, (Vector2)c.transform.position + Vector2.right * 0.3f, 0.8f);
            Suggest.Add(c);

         
        }
        else
        {
            Vector2 pos = shape.transform.position;
            pos = new Vector2(shape.transform.position.x + shape.shape.GetLength(1) * offsetX,pos.y);
            var b = Instantiate(BoxSuggestLeft,pos, Quaternion.identity, null);
      //    b.transform.localScale = new Vector2(shape.shape.GetLength(1) + end, shape.shape.GetLength(0));
            b.transform.Find("Box/box_hint").GetComponent<SpriteRenderer>().size = new Vector2((shape.shape.GetLength(1) + end), shape.shape.GetLength(0));
            b.gameObject.layer = 11;
            b.AddComponent<DestroySelf1>();
            Suggest.Add(b);
            var c = Instantiate(DirectSuggset, GetPosition(shape.ListShape,shape.shape, false), Quaternion.identity, null);
            c.AddComponent<DestroySelf1>();
            c.AddComponent<LoopMove>();
            c.GetComponent<LoopMove>().SetUp(c.transform.position, (Vector2)c.transform.position + Vector2.left*0.3f ,0.8f);
            Suggest.Add(c);

          
        }
        
     
        a.gameObject.layer = 11;
        for (int i = 0; i < a.GetComponent<Shape>().ListShape.Count; i++)
        {
            a.GetComponent<Shape>().ListShape[i].gameObject.layer = 11;
            a.GetComponent<Shape>().ListShape[i].GetComponent<BoxCollider2D>().enabled = false;
        }

        a.GetComponent<Shape>().FadeShape();
        a.GetComponent<Shape>().enabled = false;
        a.AddComponent<LoopMove>();
        a.AddComponent<DestroySelf1>();
        Suggest.Add(a);
        a.gameObject.SetActive(false);
        a.GetComponent<LoopMove>().SetUp(PosStart, PosEnd, Mathf.Abs(end));






    }

    public Vector2 GetPosition(List<GameObject> Cube, int[,] shape,bool max)
    {
        int p = 0;
        if (max)
        {
            p = shape.GetLength(0)-1;
        }
        else
        {
            p = 0;
        }
      

        for(int i = 0; i < shape.GetLength(0); i++) 
        {
            int x = shape.GetLength(1) * i + p;

           for(int j = 0; j < Cube.Count; j++)
            {
                if(x.ToString() == Cube[j].gameObject.name)
                {
                    return new Vector2((initPoint.x + offsetX * Cube[j].GetComponent<DestroySelf>().Point.x) + offsetX / 2,(initPoint.y - offsetY * Cube[j].GetComponent<DestroySelf>().Point.y) - offsetY / 2);
                }
            }
        }

            //for(int j = 0;j < Cube.Count; j++)
            //{
            //  int index = 
            ////if (shape[j, p] != 0)
            ////{
            ////    return new Vector2((initPoint.x + offsetX * p)+offsetX/2,(initPoint.y-offsetY*j)-offsetY/2);
            ////}
             
            //}
        return Vector2.zero;
    }

    


    public void PauseGame()
    {
        GameManager.Ins.isGamePause = true;
    }
    public void RemuseGame()
    {
        GameManager.Ins.isGamePause = false;
    }




    #endregion


   

}






