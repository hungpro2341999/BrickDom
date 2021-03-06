﻿using System.Collections;
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
    public int index = 0;
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

    public List<int> ColumnSplit = new List<int>();
    public List<int> RowSplit = new List<int>();
    public void SpawnInit()
    {
        for (int i = 0; i < 10; i++)
        {
            var a = Poolers.Ins.GetObject(PrebShape, Vector2.zero, Quaternion.identity);
            a.gameObject.SetActive(true);

            Debug.Log("" + i);

        }
        for (int i = 0; i < Row * Column; i++)
        {
            var a = Poolers.Ins.GetObject(PrebCube, Vector2.zero, Quaternion.identity);
            a.gameObject.SetActive(true);

         //   Debug.Log("" + i);

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
        Event_Start_Game += SpawnStartGame;
        Event_Start_Game += RefershBoard_Ver_2;

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

        Board = new int[Row, Column];

        DestroyAll_Ver_1();
        GameManager.Ins.OpenWindow(TypeWindow.GamePlay);
     


    }
    public void Rest_Game_1()
    {
        if (!GameManager.Ins.isGameOver)
        {
            if (CtrlData.CountPlay % 2 == 0)
            {
                ManagerAds.Ins.ShowInterstitial();
                CtrlData.CountPlay++;
            }
            GameManager.Ins.isGameOver = false;

            Board = new int[Row, Column];

            DestroyAll_Ver_1();
            GameManager.Ins.OpenWindow(TypeWindow.GamePlay);
            AnimSetting.Ins.ChangeStatus();
            CtrlGamePlay.Ins.RemuseGame();
        }

      


    }

    public void ReviceGame()
    {

        ManagerAds.Ins.ShowRewardedVideo((done) =>
        {
            if (done)
            {
                //int a = Random.Range(2, 5);
                int a = 4;
                int start = Random.Range(6, 9);
                for (int i = 0; i < a; i++)
                {

                    DestroyRow(start);

                    start--;
                }
                DestroyAndSplitShape();
                TimeWait = 0.6f;


                isClick_up = true;



                GameManager.Ins.isGameOver = false;

                GameManager.Ins.OpenWindow(TypeWindow.GamePlay);
            }
          


        });




    }


    public void SetUpDestroyAllCube()
    {
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
        Invoke("StartDestroyAllCube", totalTime);

    }



    public void StartDestroyAllCube()
    {

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
        AudioManganger.Ins.PlaySound("Destroy");
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
        Invoke("OffSound", totalTime);

    }
    public void OffSound()
    {
        AudioManganger.Ins.OffSound("Destroy");
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
        if (ClickUI.clickPause)
            return;
        if (GameManager.Ins.isGameOver || GameManager.Ins.isGamePause)
            return;



        if (TimeWait > 0)
        {
            TimeWait -= Time.deltaTime;
            if (TimeWait <= 0)
            {
                SimulateDown();

            }
            return;
        }


        AnimSetting.Ins.Btn_HighScore.gameObject.SetActive(true);
        #region

        if (Input.GetKeyDown(KeyCode.A))
        {

            //bool FindOut = false;
            //if (!StartSuggestionsLeft(FindOut))
            //{
            //    SuggestRight(FindOut);
            //}

            int[,] matrix = new int[2, 4] {
                 {0,0,0,1},
                {1,1,1,1}

            };
            //int[] coll = new int[1] { 2 };

            //List<int[,]> matrixx = SplitColumnMatrix(matrix, coll);

            //for (int i = 0; i < matrixx.Count; i++)
            //{
            //    Debug.Log(i + "\n" + Render(matrixx[i]));
            //}


            ////////////////////////////////////////////////////////////

            //List<List<int>> column = new List<List<int>>();
            //List<int> col1 = new List<int> { 0, 0, 1 };
            //List<int> col2 = new List<int> { 0, 0, 1 };
            //List<int> col3 = new List<int> { 0, 0, 2 };
            //column.Add(col1);
            //column.Add(col2);
            //column.Add(col3);
            //Debug.Log(Render(ListColumToMatrix(column)));





            //for (int i = 0; i < Shapess.Count; i++)
            //{
            //    Debug.Log(i + " \n " + Render(Shapess[i]));
            //    //   SpawnShape(Shapess[i],Vector2.zero,1);
            //}


            //List<List<int>> List = CutRow(matrix);
            //for (int i = 0; i < List.Count; i++)
            //{
            //    Debug.Log(RenderList(List[i]));
            //}
         


        }
        if (Input.GetKeyDown(KeyCode.Q))
        {

            for (int i = 0; i < rowDestroy.Length; i++)
            {
                DestroyRow(rowDestroy[i]);
            }

            DestroyAndSplitShape();



        }

        if (ShapeClick != null)
        {

        }
       // Matrix.text = Render((Board));

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

            if (!isClick_up)
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
               // RefershBoard_Ver_2();
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
          //  RefershBoard_Ver_2();

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
                 

                    if (offset != ShapeClick.PointInitCheck.y)
                    {

                        timeSugg = timeSuggest;
                        SimulateDown();
                        SetUpAll();
                        isClick_up = true;
                        ShapeClick.ResetStatus();
                    }
                    else
                    {
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
              


              //  Debug.Log("Complete_Move");

                initPos = false;

                if (HasScore())
                {
                    if (HasEff())
                    {
                        AudioManganger.Ins.PlaySound("Electric");
                      
                        TimeWait = 0.75f;

                        Invoke("StartSplitShape", 0.25f);

                      
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
                    if (CtrlData.CountGame % 3 == 0 || CtrlData.Score > 150)
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
            else
            {
               
            }

        }
        //////////////                MOVE DOWN  ///////////////////////
        ///







    }
    public void StartSplitShape()
    {
        GFG gg = new GFG();
        RowSplit.Sort(gg);
        ColumnSplit.Sort(gg);

        int[] row = RowSplit.ToArray();
        int[] column = ColumnSplit.ToArray();

          EffSplitShape(RowSplit.ToArray(), ColumnSplit.ToArray());




        //List<Shape> ShapeSplit = new List<Shape>();

        //List<Shape> listClone = new List<Shape>(List_Shape);
        //int Update = 0;
        //for (int i = 0; i < listClone.Count; i++)
        //{
        //    if (listClone[i].TypeShape != TypeShape.crossBar_1 && !listClone[i].isEff)
        //    {
        //        if (SplitShape(listClone[i], row, column))
        //        {

        //            ShapeSplit.Add(listClone[i]);
        //        }


        //    }


        //}
        //for (int i = 0; i < ShapeSplit.Count; i++)
        //{
        //    ShapeSplit[i].DestroyAllCubeAndShape();
        //}

        //RowSplit = new List<int>();
        //ColumnSplit = new List<int>();
        //Invoke("DestroyAndSplitShape", 0.35f);

    }
    class GFG : IComparer<int>
    {
        public int Compare(int x, int y)
        {
            //if (x == 0 || y == 0)
            //{
            //    return 0;
            //}

            // CompareTo() method 
            return x.CompareTo(y);

        }
    }

    public void Sort(List<int> list)
    {
        
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
        

        SplitShape();

        RefershBoard_Ver_2();
        TimeWait = WaitTime;


        
    }

    public void EffSplitShape(int[] row,int[] column)
    {

        //List<Shape> shapeSplit = new List<Shape>();
        //List<Shape> isCheck = new List<Shape>();

        //for(int i = 0; i < List_Shape.Count; i++)
        //{
        //    bool find = false;
        //    for(int j = 0; j < List_Shape[i].ListShape.Count; j++)
        //    {
        //        if (!find)
        //            break;

        //        for (int k = 0; k < row.Length; k++)
        //        {
        //            if (!find)
        //                break;

        //            DestroySelf Cubes = List_Shape[i].ListShape[j].GetComponent<DestroySelf>();
        //            if(Cubes.Point.y == row[k])
        //            {
        //                if(!shapeSplit.Contains(Cubes.shape) && Cubes.shape.gameObject.layer!=0 && !Cubes.shape.isEff)
        //                {
        //                    isCheck.Add(Cubes.shape);
        //                    shapeSplit.Add(Cubes.shape);
        //                    find = true;

        //                }
        //            }

        //        }



        //    }
        //}



       
        List<Shape> ShapeSplit = new List<Shape>();
        GFG gg = new GFG();
        RowSplit.Sort(gg);
        ColumnSplit.Sort(gg);

        for (int i = 0; i < List_Shape.Count; i++)
        {
            if (List_Shape[i].shape.GetLength(1) <= 1)

                continue;
           
                bool isFind = false;

            for (int j = 0; j < List_Shape[i].ListShape.Count; j++)
            {
             

                if (isFind)
                    break;
                DestroySelf Cubes = List_Shape[i].ListShape[j].GetComponent<DestroySelf>();

                for (int k = 0; k < column.Length; k++)
                {
                   

                    if (isFind)
                        break;

                    if (Cubes.Point.x == column[k])
                    {
                        if (Cubes.shape.gameObject.layer != 0 && !Cubes.shape.isEff)
                        {
                            ShapeSplit.Add(Cubes.shape);
                            isFind = true;
                        }
                    }
                }
            }


        }

        List<Shape> ListShapeDestroy = new List<Shape>();


        for (int i = 0; i < ShapeSplit.Count; i++)
        {
            if (ShapeSplit[i].TypeShape != TypeShape.crossBar_1)
            {
                if(SplitShape(ShapeSplit[i], null, column))
                {
                    ListShapeDestroy.Add(ShapeSplit[i]);

                }
              
               
               
            }
           
        }
        for(int i = 0; i <ListShapeDestroy.Count; i++)
        {
            ListShapeDestroy[i].DestroyAllCubeAndShape();
        }

        RowSplit = new List<int>();
        ColumnSplit = new List<int>();
        Invoke("DestroyAndSplitShape", 0.35f);

    }

    public bool HasScore()
    {
        Round++;

        int has = 0;
        for (int i = 0; i < Board.GetLength(0); i++)
        {
            int count = 0;
            for (int j = 0; j < Board.GetLength(1); j++)
            {
                if (Board[i, j] == 1)
                {
                    count++;
                }
            }
            if (count == Board.GetLength(1))
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
        if (shape != null)
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

      
        Debug.Log("-----------SPLIT SHAPE----------");

        List<Shape> listSplit = GetListShapeSplit(List_Shape);
        for (int i = 0; i < listSplit.Count; i++)
        {
            listSplit[i].ReflectShape();
            SplitShape(listSplit[i]);


        }
      

    }



    public List<Shape> GetListShapeSplit(List<Shape> Lshape)
    {
        List<Shape> split = new List<Shape>();
        for (int i = 0; i < Lshape.Count; i++)
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
            RefershBoard_Ver_2();
            ClampShape(ShapeClick);
         //   Debug.Log(Render(ShapeClick.shape));
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



        for (int i = 0; i < row.Length; i++)
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
        List<int> column = new List<int>();
     
        for (int i = 0; i < Column; i++)
        {
            
            for (int x = 0; x < Cubes.Count; x++)
            {

                if (Cubes[x].GetComponent<DestroySelf>().Point == new Vector2((float)i, row))
                {

                  
                    shape = Cubes[x].GetComponent<DestroySelf>().shape;

                    if (shape.isEff)
                    {
                        if (!RowSplit.Contains(row))
                        {
                            RowSplit.Add(row);
                        }


                        Cubes[x].GetComponent<DestroySelf>().StartSuperEff();
                        column.Add((int)Cubes[x].GetComponent<DestroySelf>().Point.x);
                        HasCubeEff++;
                        if (!ColumnSplit.Contains((int)Cubes[x].GetComponent<DestroySelf>().Point.x))
                        {
                            ColumnSplit.Add((int)Cubes[x].GetComponent<DestroySelf>().Point.x);
                        }
                      
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

        if (row != null)
        {



            if (row.Length > 0)
            {
                Combo += row.Length;
                if (Combo > 1)
                {
                    int x = Mathf.Clamp(Combo, 1, 4);
                    string audio = "Score_" + x.ToString();

                    AudioManganger.Ins.PlaySound(audio);

                    CtrlData.Ins.VisibleCombo(Combo);

                    CtrlData.Ins.VisibleScore(14 * (Combo + HasCubeEff));

                    CtrlData.Score += 14 * (Combo + HasCubeEff);

                    CtrlData.Ins.SetScore();

                }
                else
                {

                    CtrlData.Ins.VisibleScore(14 + (HasCubeEff * 7));
                    CtrlData.Score += 14 + (HasCubeEff * 7);
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

      //  Select.text = PosInit + "::" + PosCurr + " ::: " + CheckPointCorrect(pos.x, pos.y);

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

        GenerateStartGame(Random.Range(2, 3 + Mathf.Clamp(Random.Range(0, CtrlData.Level), 0, 5)), false);
        //  GenerateStartGame(2, false);
        RefershBoard_Ver_2();
    }
    public void SpawnStartGame()
    {
        DestroyAll();
        SimulateColumn.SetActive(false); 
        CtrlData.Level = 1;
        timeSuggest = 10;
        GenerateStartGame(Random.Range(2, 4), true);
        //GenerateStartGame(2, true);
        RefershBoard_Ver_2();
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
        for (int i = 0; i < Column; i++)
        {
            for (int x = 0; x < Cubes.Count; x++)
            {

                if (Cubes[x].GetComponent<DestroySelf>().Point == new Vector2((float)i, row))
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

    public void RefershBoard_Ver_2()
    {
        ReflectShape();
        Board = new int[Row, Column];
        for (int j = 0; j < List_Shape.Count; j++)
        {

            for (int i = 0; i < List_Shape[j].ListShape.Count; i++)
            {
                PushToBoard(List_Shape[j].ListShape[i]);
            }
        }

    }
    //public void RefershBoard_Ver_2()
    //{
      

    //    //Board = new int[Row, Column];
    //    //List<Shape> ListShapeMove = new List<Shape>();
      
    //    //// Debug.Log("INIT  : " + Render(shape));
    //    //List<List<Vector2>> Shapes = PushListCubeInList(List_Shape);
    //    //for (int i = 0; i < Shapes.Count; i++)
    //    //{
    //    //    for (int j = 0; j < Shapes[i].Count; j++)
    //    //    {
    //    //        Vector2 point = Shapes[i][j];
    //    //        Board[(int)point.y, (int)point.x] = 1;
    //    //    }
    //    }
      
    //}

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

    public Vector2 RandomPosStart()
    {



        int y = Random.Range(Row - Random.Range(0, 4), Row);
        int x = Random.Range(0, CtrlGamePlay.Ins.Column);


        return new Vector3(CtrlGamePlay.Ins.initPoint.x + x * CtrlGamePlay.Ins.offsetX, CtrlGamePlay.Ins.initPoint.y - y * CtrlGamePlay.Ins.offsetX);

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

            for (int i = 0; i < Split_Row.Length; i++)
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
                //      Debug.Log("CUT : " + shape.name);

                ShapeSplit.Add(GrounpRow);
                //          Debug.Log(RenderList(GrounpRow));
                List<int> SplitRow = new List<int>();
                int indexSplit = 0;
                List<List<int>> shapeSplit = Split(GrounpRow);
                for (int i = 0; i < shapeSplit.Count; i++)
                {
                    //   Debug.Log("LIST : " + i);
                    // Debug.Log(RenderList(shapeSplit[i]));
                }

                for (int i = 0; i < shapeSplit.Count; i++)
                {
                    List<List<int>> shape_Split = new List<List<int>>();
                    Vector3 pos = shape.transform.position;
                    pos -= new Vector3(0, shapeSplit[i][0] * CtrlGamePlay.Ins.offsetX);
                    for (int j = 0; j < shapeSplit[i].Count; j++)
                    {
                        shape_Split.Add(Split_Row[shapeSplit[i][j]]);
                    }
                    string s = "Shape Split : ";

                    //     s +="\n" +  Render(ListToMatrix(shape_Split)) ;
                    //     Debug.Log(s);
                    SpawnShape(ListToMatrix(shape_Split), pos, Color);


                }
                shape.DestroyAllCubeAndShape();


            }
            else
            {
                Debug.Log("NOT  CUT : " + shape.name);
            }

        }
    }

    public bool SplitShape(Shape shape, int[] row, int[] col)
    {

        List<List<int>> RowAndColumSplit = ConvertToSplitShape(shape,null, col);

      int[]  row1 = RowAndColumSplit[0].ToArray();
       int[] col1 = RowAndColumSplit[1].ToArray();

        if(row1.Length == 0 && col1.Length == 0)
        {
            return false;
        }
        /////////////////////////
        string key_row = "[";
        //for (int i = 0; i < row1.Length; i++)
        //{
        //    key_row += row1[i];
        //}
        key_row += "]";

        string key_col = "[";
        for (int i = 0; i < col1.Length; i++)
        {
            key_col += col1[i];
        }
        key_col += "]";

        

        int roll = CtrlGamePlay.RollShape(shape.TypeShape, shape.shape);

        string key = "ROLL : " +roll+ " " +shape.TypeShape.ToString()+ " : " + key_row + key_col;

        //   Debug.Log("KEY : "+key);

        //List<int[,]> matrix = InforShapeSplit.InforShape[key];
        //List<Vector2> ListPoint = InforShapeSplit.List_Point[key];
      try 
        {

            if(shape.TypeShape == TypeShape.T)
            {
                return SplitShape_Raw(shape, null, col);
            }

            //InforForSplitShape Infor = InforShapeSplit.Directory[shape.TypeShape];
            //List<int[,]> matrix = Infor.InforShape[key];
            //List<Vector2> ListPoint = Infor.List_Point[key];
           
            
            int index = InforShapeSplit.DirectKey[key];

            List<int[,]> matrix = InforShapeSplit.ArrayDataGameShape[index].GetListMatrix();
            if (matrix.Count <=1)
            {
                return false;
            }
            List<Vector2> ListPoint = InforShapeSplit.ArrayPositonShape[index].Matrix;

            List<Vector2> List_Position = ConvertToPostionBoard(ListPoint, shape.Point);
            for (int i = 0; i < matrix.Count; i++)
            {

                // Debug.Log("SHAPE : " + Render(matrix[i]));
                if (CountInCube(matrix[i]) == 0)
                    return false;
           //     Debug.Log("SPAWN");
               SpawnShape(matrix[i], List_Position[i], shape.IDColor);



            }

            //  shape.DestroyAllCubeAndShape();

            return true;


        }
        catch (System.Exception e)
        {
            return SplitShape_Raw(shape, null, col);
        }

        return true;

    //    List<GameObject> ListShape = shape.ListShape;
    //    int[,] type = shape.shape;

        //    Debug.Log(Render(shape.shape));



        //    Debug.Log(RenderList(RowAndColumSplit[0]));
        //    Debug.Log(RenderList(RowAndColumSplit[1]));
        //    int[] row1 = null;
        //    int[] col1 = null;
        //    List<int> ListPointRow = new List<int>();
        //    List<int> ListPointColumn = new List<int>();

        //    if (RowAndColumSplit[0].Count != 0)
        //    {
        //        row1 = RowAndColumSplit[0].ToArray();
        //        Debug.Log(row1.Length);


        //    }




        //    if (RowAndColumSplit[1].Count != 0)
        //    {

        //        col1 = RowAndColumSplit[1].ToArray();

        //        Debug.Log(col1.Length);




        //    }

        //    ListPointRow.Add(0);
        //    ListPointColumn.Add(0);
        ////    List<Vector2> Grounp = GrounpPoint(ListPointRow, ListPointColumn);


        //    List<int[,]> ListRowtMatrix = new List<int[,]>();
        //    List<int[,]> ListColumnMatrix = new List<int[,]>();
        //    List<int[,]> TotalMatrix = new List<int[,]>();



        //    List<List<int[,]>> ListColumMatrix_1 = new List<List<int[,]>>();

        //    if (row1 != null)
        //    {

        //        Debug.Log("Split Row");

        //        ListRowtMatrix = SplitRowMatrix(type, row1,ref ListPointRow);

        //        for (int i = 0; i < ListRowtMatrix.Count; i++)
        //        {
        //            Debug.Log(Render(ListRowtMatrix[i]));
        //        }

        //    }

        //    if (ListRowtMatrix.Count != 0 && col1 != null)
        //    {
        //        Debug.Log("Split Col");
        //        for (int i = 0; i < ListRowtMatrix.Count; i++)
        //        {
        //            ListColumMatrix_1.Add(SplitColumnMatrix(ListRowtMatrix[i], col1,ref ListPointColumn));


        //        }
        //        for (int j = 0; j < ListColumMatrix_1.Count; j++)
        //        {
        //            for (int k = 0; k < ListColumMatrix_1[j].Count; k++)
        //            {
        //                Debug.Log(Render(ListColumMatrix_1[j][k]));
        //            }
        //        }


        //        if (ListColumMatrix_1.Count != 0)
        //        {
        //            for (int j = 0; j < ListColumMatrix_1.Count; j++)
        //            {
        //                for (int k = 0; k < ListColumMatrix_1[j].Count; k++)
        //                {
        //                    ListColumnMatrix.Add(ListColumMatrix_1[j][k]);
        //                }
        //            }
        //        }
        //        for (int i = 0; i < ListColumnMatrix.Count; i++)
        //        {
        //            TotalMatrix.Add(ListColumnMatrix[i]);
        //        }



        //    }
        //    else if (col1 != null)
        //    {
        //        ListColumnMatrix = SplitColumnMatrix(type, col1,ref ListPointColumn);
        //        for (int i = 0; i < ListColumnMatrix.Count; i++)
        //        {
        //            TotalMatrix.Add(ListColumnMatrix[i]);
        //        }
        //        List<Vector2> Points = new List<Vector2>();





        //    }
        //    else if (row1 != null)
        //    {
        //        for (int i = 0; i < ListRowtMatrix.Count; i++)
        //        {
        //            TotalMatrix.Add(ListRowtMatrix[i]);
        //        }
        //    }
        //    else
        //    {

        //        TotalMatrix.Add(shape.shape);
        //    }

        //    if (TotalMatrix.Count > 1)
        //    {
        //        shape.DestroyAllCubeAndShape();
        //    }
        //    else
        //    {
        //        return;
        //    }

       
       
       

        
       
    }
    
    
    public List<Vector2> GrounpPoint(List<int> Row, List<int> Column)
    {
        List<Vector2> point = new List<Vector2>();
        for (int i = 0; i < Row.Count; i++)
        {
            Vector2 p = Vector2.zero;
            p.x = Row[i];
            for (int j = 0; j < Column.Count; j++)
            {
                p.y = Column[j];
                point.Add(p);
            }

        }

        for (int i = 0; i < point.Count; i++)
        {
            Debug.Log(point[i].x + " :::: " + point[i].y);

        }
        return point;


    }

    public List<Vector2> ConvertToPostionBoard(List<Vector2> point,Vector2 PosInit)
    {
        Debug.Log("POINT INIT : " + PosInit.x + "  " + PosInit.y);
        List<Vector2> L_Vector = new List<Vector2>();
        for(int i = 0; i < point.Count; i++)
        {
            Debug.Log("PosShape : " + point[i].x + "  " + point[i].y);
         int x =    (int)PosInit.x + (int)point[i].x;
         int y = (int)PosInit.y + (int)point[i].y;
            Debug.Log(x + ": " + y);
            float posX = CtrlGamePlay.Ins.initPoint.x + y * offsetY;
            float posY = CtrlGamePlay.Ins.initPoint.y - x * offsetX;

            Vector2 pos = new Vector2(posX, posY);
            L_Vector.Add(pos);
          //  Debug.Log("PosConvert : " + pos.x+"   "+pos.y);
        }
        return L_Vector; 
       
    }

  
   

    //public List<int[,]> SplitMatrix(int[,] matrix, int[] row, int[] colum)
    //{



    //}


    public List<int[,]> SplitRowMatrix(int[,] matrixs, int[] row,ref List<int> listpoint)
    {
        List<int> point = new List<int>();
        List<List<int>> ListRow = CutRow(matrixs);
        Debug.Log("CUT ROW");
        for(int i = 0; i < ListRow.Count; i++)
        {
            Debug.Log(RenderList(ListRow[i]));
        }
        List<int[,]> ListMatrix = new List<int[,]>();

        List<int> Flag = SetUpFlag(matrixs.GetLength(0), row);
        Debug.Log(ListRow.Count + "  " + Flag.Count);
        Debug.Log(RenderList(Flag));

        List<int[,]> matrix = new List<int[,]>();

        List<List<int>> Row = new List<List<int>>();
        point.Add(0);
        for(int i = 0; i < Flag.Count; i++)
        {
            if (Flag[i] == -1)
            {
                if (Row.Count != 0)
                {
                    ListMatrix.Add(ListToMatrix(Row));
                    Row = new List<List<int>>();
                }

                Row.Add(ListRow[i]);
                ListMatrix.Add(ListToMatrix(Row));
                Row = new List<List<int>>();


                if (!point.Contains(i))
                {
                    point.Add(i);
                    if (i + 1 < Flag.Count)
                    {
                        if (!point.Contains(i + 1))
                        {
                            point.Add(i + 1);
                        }
                    }

                }
                else
                {
                    if (i + 1 < Flag.Count)
                    {
                        if (!point.Contains(i + 1))
                        {
                            point.Add(i + 1);
                        }
                    }
                }
              


            }
            else
            {
                Row.Add(ListRow[i]);
            }    
        }
        if (Row.Count != 0)
        {
           ListMatrix.Add(ListToMatrix(Row));
           
        }
        Debug.Log("RENDER SPLIT POINT ROW : " + RenderList(point));
        listpoint = point;
        return ListMatrix;
      



    }
    public List<int> SetUpFlag(int Row,int[] row)
    {
        List<int> Flag = new List<int>();
        for(int i=0;i<Row; i++)
        {
            bool a = false;
            string s = "";
            for (int j = 0; j < row.Length; j++)
            {
               
                s += i + "  " + row[j];
                if (i == row[j])
                {
                    s += "Find";
                    Flag.Add(-1);
                    a = true;
                    break;
                }
            }
        //    Debug.Log(s);
           if(!a)
            Flag.Add(1);
        }
        return Flag;
    }
  

    public List<List<int>> CutRow(int[,] matrix)
    {
        List<List<int>> ListRow = new List<List<int>>();
        for(int i = 0; i < matrix.GetLength(0); i++)
        {
            List<int> Row = new List<int>();
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                Row.Add(matrix[i, j]);
            }
            ListRow.Add(Row);
        }
        return ListRow;
       
    }



    public List<List<int>> CutColumn(int[,] matrix)
    {
        List<List<int>> ListColumn = new List<List<int>>();
        for (int i = 0; i < matrix.GetLength(1); i++)
        {
            List<int> Column = new List<int>();
            for (int j = 0; j < matrix.GetLength(0); j++)
            {
                Column.Add(matrix[j, i]);
            }
            ListColumn.Add(Column);
        }
        return ListColumn;

    }
    public List<int[,]> SplitColumnMatrix(int[,] matrixs, int[] column,ref List<int> point)
    {
        List<int> ListPoint = new List<int>();
        List<List<int>> ListColumn = CutColumn(matrixs);

        List<int[,]> ListMatrix = new List<int[,]>();

        List<int> Flag = SetUpFlag(matrixs.GetLength(1), column);
        Debug.Log(RenderList(Flag));


        List<int[,]> matrix = new List<int[,]>();

        List<List<int>> Column = new List<List<int>>();

        ListPoint.Add(0);
        for (int i = 0; i < Flag.Count; i++)
        {
            if (Flag[i] == -1)
            {
                if (!ListPoint.Contains(i))
                {
                    ListPoint.Add(i);
                    if (i + 1 < Flag.Count)
                    {
                        if (!ListPoint.Contains(i + 1))
                        {
                            ListPoint.Add(i + 1);
                        }
                       
                    }
                }
                else
                {
                    if (!ListPoint.Contains(i + 1))
                    {
                        if (i + 1 < Flag.Count)
                            ListPoint.Add(i + 1);
                    }
                      
                }

                if (Column.Count != 0)
                {
                     
                    ListMatrix.Add(ListColumToMatrix(Column));
                    Column = new List<List<int>>();

                }
                 
               
                
                Column.Add(ListColumn[i]);

                ListMatrix.Add(ListColumToMatrix(Column));

                Column = new List<List<int>>();

            }
            else
            {
                Column.Add(ListColumn[i]);
            }
        }
        if (Column.Count != 0)
        {
            ListMatrix.Add(ListColumToMatrix(Column));

        }

        Debug.Log("RENDER SPLIT POINT : " + RenderList(ListPoint));
        point = ListPoint;
        return ListMatrix;




    }


    public List<List<int>> ConvertToSplitShape(Shape shape,int[] row,int[] colum)
    {
        List<List<int>> rowAndColum = new List<List<int>>();
        Vector2 point = shape.Point;
        int[,] type = shape.shape;
        List<int> splitColumn = new List<int>();
        List<int> splitRow = new List<int>();
        
        // for(int i = 0; i < type.GetLength(0); i++)
        //{
        //    Debug.Log(i + "  " + (point.x + i));
        //    splitRow.Add((int)(point.x + i));  
        //}
        for (int j = 0; j < type.GetLength(1); j++)
        {
      //      Debug.Log(j + "  " + (point.y + j));
            splitColumn.Add((int)(point.y + j));

        }

        List<int> splitColumn_ver_2 = new List<int>();
        List<int> splitRow_Ver_2 = new List<int>();

        //if (row != null)
        //{
        //    for (int i = 0; i < splitRow.Count; i++)
        //    {
        //        for (int j = 0; j < row.Length; j++)
        //        {
        //            Debug.Log(splitRow[i] + "   " + row[j]);
        //            if (splitRow[i] == row[j])
        //            {
        //                splitRow_Ver_2.Add(i);
        //            }
        //        }
        //    }
        //}
        
        if (colum != null)
        {
            for (int i = 0; i < splitColumn.Count; i++)
            {
                for (int j = 0; j < colum.Length; j++)
                {


                //    Debug.Log(splitColumn[i] + "   " + colum[j]);
                    if (splitColumn[i] == colum[j])
                    {
                        splitColumn_ver_2.Add(i);
                    }
                }
            }
        }

        Debug.Log(splitRow_Ver_2.Count + "   " + splitColumn_ver_2.Count);
        rowAndColum.Add(splitRow_Ver_2);
        rowAndColum.Add(splitColumn_ver_2);
       
      
        return rowAndColum;


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
    public int[,] ListColumnMatrix(List<List<int>> shape)
    {
        int[,] matrix = null;
        if (shape.Count != 0)
        {
            matrix = new int[shape[0].Count, shape.Count];
            int column = shape.Count;
            for(int i = 0; i < shape.Count; i++)
            {
                for(int j = 0; j < shape[0].Count; j++)
                {
                    matrix[i, j] = shape[i][j];
                }
            }
            



        }
        else
        {
            Debug.LogError("LIST MATRIX NULL");
        }

        return matrix;
    }

    public IEnumerator IESpawnShape(int[,] Type, Vector2 pos, int color,float time)
    {
        yield return new WaitForEndOfFrame();
        int back = BackTo(Type);
        int forward = BackTo_Top(Type);
        pos = new Vector2(pos.x += back * offsetX, pos.y -= forward * offsetY);
        TypeShape type = CtrlGamePlay.Ins.MatrixToType(Type);
        int roll = RollShape(type, Type);
        Type = Shape.SplitMatrix(CtrlGamePlay.standardizedMatrix(Type));
        //   Debug.Log("INFOR SHAPE SPLIT : " + back + " :: " + type.ToString() + " :: " + roll);

        InforShape infor = new InforShape(type, pos, Type, roll, color);

        SpawnShape(infor);


    }
    public void  SpawnShape(int[,] Type,Vector2 pos,int color)
    {

        int back = BackTo(Type);
        int forward = BackTo_Top(Type);
        pos = new Vector2(pos.x += back * offsetX, pos.y-=forward*offsetY);
        TypeShape type = CtrlGamePlay.Ins.MatrixToType(Type);
        int roll = RollShape(type, Type);
        Type =  Shape.SplitMatrix(CtrlGamePlay.standardizedMatrix(Type));
     //   Debug.Log("INFOR SHAPE SPLIT : " + back + " :: " + type.ToString() + " :: " + roll);

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
    public int BackTo_Top(int[,] Type)
    {
        int x = 0;
        int Colum = Type.GetLength(1);
        int Row = Type.GetLength(0);
        for (int i = 0; i < Row; i++)
        {
            for (int j = 0; j < Colum; j++)
            {
                if (Type[i, j] != 0)
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
    public static string RenderList(List<int> list)
    {
        string s = "";
        for(int i = 0; i < list.Count; i++)
        {
            s += "  " + list[i];
        }
        return s;
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
        if (ShapeClick != null)
        {
            ShapeClick.DestroyAllCubeAndShape();
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

     //   MoveSelect.text = "DOWN : " + minY +"\n";
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
        RefershBoard_Ver_2();
        SetUpAll();
        //  SimulateDown_Ver_2();
        SortShape();
       
        List<Shape> ListShapeMove = new List<Shape>();
        int[,] shape = CloneBoard(Board);
        // Debug.Log("INIT  : " + Render(shape));
        List<List<Vector2>> Shapes = PushListCubeInList(List_Shape);
      
        List<int> ListMove = GenerateList(Shapes.Count);

       
            while (isCheckDown(shape, 1, Shapes))
                for (int i = 0; i < Shapes.Count; i++)
                {
                    //   Debug.Log(List_Shape[i].name " : " + i + "  :" + Render(shape));
                    if (isMoveDown(shape, 1, Shapes[i]))
                    {
                        ListMove[i]++;
                        SimulateMoveDown(Shapes[i], 1, shape);
                        MoveDownOneCube(List_Shape[i], ListMove[i]);
                        //  ListShapeMove.Add(List_Shape[i]);
                        //  Debug.Log(" : " + i + "  :" + Render(shape));
                    }
                    else
                    {
                        ResetMove(Shapes[i],shape);
                    }


                }

          

        



       
       
            //   Debug.Log("Khong Co");

            Board = shape;
        //  Debug.Log("CANVAS : " + Render(shape));
        //    Debug.Log("Nornal : \n" + Render(Board));
        //  Debug.Log("RESULT MOVE : \n" + Render(shape));
      

      


    }
   


    public bool MoveDown(List<Vector2> shape, int[,] Board,int space)
    {
         for(int i = 0; i < shape.Count; i++)
        {
            if (isInMatrix((int)shape[i].y+space, (int)shape[i].x, Board))
            {
                Debug.Log(((int)shape[i].y + space) + "   " + shape[i].x);
                if(Board[(int)shape[i].y +space, (int)shape[i].x] == 1)
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

    public int isMoveDown_Ver_2(List<Vector2> shape, ref int[,] Board)
    {
       

        for(int i = 0; i < shape.Count; i++)
        {
            Board[(int)shape[i].y, (int)shape[i].x] = 0;
            
        }
        int start = 0;
        while(MoveDown(shape,Board,start))
        {
           
            start++;
        }
        if(!MoveDown(shape, Board, start))
        {
            start--;
        }
        Debug.Log("Start : " + start);
      
        for (int i = 0; i < shape.Count; i++)
        {

            Debug.Log(((int)shape[i].y + start) + "  " + (int)shape[i].x);
            Board[(int)shape[i].y+start, (int)shape[i].x] = 1;

        }
        return start;

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
       for(int i = 0; i < shape.Count; i++)
        {
           Vector2 point = shape[i];
            Board[(int)point.y, (int)point.x] = 0;
        }
       
        for (int i = 0; i < shape.Count; i++)
        {
          
            if (CtrlGamePlay.isInMatrix((int)shape[i].y + space, (int)shape[i].x, Board))
            {
                if (Board[((int)shape[i].y + space), (int)shape[i].x] == 1)
                {
                    for (int j = 0; j < shape.Count; j++)
                    {
                        Vector2 point = shape[j];
                        Board[(int)point.y, (int)point.x] = 1;
                    }
                    return false;
                }

            }
            else
            {
                for (int j = 0; j < shape.Count; j++)
                {
                    Vector2 point = shape[j];
                    Board[(int)point.y, (int)point.x] = 1;
                }
                return false;
            }
        }
        for (int i = 0; i < shape.Count; i++)
        {
            Vector2 point = shape[i];
            Board[(int)point.y, (int)point.x] = 1;
        }
        return true;
    }

    public bool isMoveDown(int[,] Board,int space,List<Vector2> shape)
    {
        
        for(int i = 0; i < shape.Count; i++)
        {
            Vector2 point = shape[i];
            Board[(int)shape[i].y, (int)shape[i].x] = 0;
        }

        for (int i = 0; i < shape.Count; i++)
        {
         
            if (CtrlGamePlay.isInMatrix((int)shape[i].y+space, (int)shape[i].x,Board))
            {
                if(Board[((int)shape[i].y + space), (int)shape[i].x] == 1)
                {

                    for (int j = 0; j < shape.Count; j++)
                    {
                        Vector2 point = shape[j];
                        Board[(int)shape[i].y, (int)shape[i].x] = 1;
                    }
                    return false;
                }
               
            }
            else
            {

                for (int j = 0; j < shape.Count; j++)
                {
                    Vector2 point = shape[i];
                    Board[(int)shape[j].y, (int)shape[j].x] = 1;
                }
                return false;
            }
        }

        for (int i = 0; i < shape.Count; i++)
        {
            Vector2 point = shape[i];
            Board[(int)shape[i].y, (int)shape[i].x] = 1;
        }
        return true;
    }
   

    public List<List<Vector2>> PushListCubeInList(List<Shape> list_shape)
    {
        int[,] Clone = new int[Row, Column];
        List<List<Vector2>> ListShape = new List<List<Vector2>>();

        for(int i = 0; i < list_shape.Count; i++)
        {
            List<Vector2> shape = new List<Vector2>();
            for(int j = 0; j < list_shape[i].ListShape.Count; j++)
            {
                Vector2 point = list_shape[i].ListShape[j].GetComponent<DestroySelf>().Point;
                shape.Add(point);
                Clone[(int)point.y, (int)point.x] = 1;
               
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
                
             for(int i=0;i<index;i++)
            {
               
             
                 
               
              
                  pos = CtrlGamePlay.RandomPosShape();
                  if(isSpawnCorrect(ref CloneBoard, out type, out pos, out shape, out roll))
                {
                    int color = CtrlData.RandomColor(type, roll);
                    InforShape infor = new InforShape(type, pos, shape, roll, color);
                    //    Debug.Log("Spawn SS : " + i);
                    //     Debug.Log("Board SS : " + "\n" + Render(CloneBoard));
                    InforShape.Add(infor);

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
                if (ReadFile.CompleteCode)
                {
                    int x = Random.Range(0, 100);
                    if (x >= 0 && x <= 3)
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

    //    Debug.Log("INSTANCE ");
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


    
    public bool isSpawnCorrect(ref int[,] Board,out TypeShape type,out Vector3 pos,out int[,] shapeCorrect,out int Roll)
    {
        int count = 0;
        int indexRoll = 0;
        bool isSpawnCorrect = false;
        int[,] shape = null;
        int[,] Clone = CloneBoard(Board);
        int[,] backup = null;
        int[,] backupBoard = CloneBoard(Board);
     //   Debug.Log(Render(Board));
        TypeShape typeshape = TypeShape.None;
        Vector3 position = Vector3.zero;
        while (!isSpawnCorrect)
        {
            if (count > 70)
            {
                isSpawnCorrect = false;
               

                break;
            }

            try
            {
                isSpawnCorrect = true;

             //   Debug.Log(Render(Clone));
                position = RandomPosShape();

                if (count % 32 == 0)
                {
                    typeshape = Shape.RandomShape();
                }

                if (count % 8 == 0)
                {
                    backup = Shape.Clone(SimulateRoll(0, typeshape, true, out indexRoll));

                }
               

                shape = backup;


                shape = Shape.SplitMatrix(shape);
                //  Debug.Log("Board :::: ");
                //    Debug.Log(Render(shape));
                for (int i = 0; i < shape.GetLength(0); i++)
                {
                    for (int j = 0; j < shape.GetLength(1); j++)
                    {
                        if (shape[i, j] != 0)
                        {
                            Vector3 posCurr = new Vector3(position.x + j * CtrlGamePlay.Ins.offsetX, position.y - i * CtrlGamePlay.Ins.offsetY);

                            Vector2 point = CtrlGamePlay.PositonToPointMatrix(posCurr.x, posCurr.y);
                         //   Debug.Log(Render(Clone));
                            if (IsPushShapeCorrect(Clone, (int)point.x, (int)point.y))
                            {
                                Clone[(int)point.x, (int)point.y] = 1;

                            }
                            else
                            {
                                count++;

                                isSpawnCorrect = false;
                                ////       Debug.Log("Spawn False : " + point.x +" :: "+point.y);
                                //return false;

                            }
                        }


                    }
                }
            }catch(System.Exception e)
            {
                Clone = CloneBoard(Board);
            }
          

            if(!isSpawnCorrect)
            Clone = backupBoard;

        }
        if (!isSpawnCorrect)
        {
           
            Roll = indexRoll;
            pos = position;
            type = typeshape;
            Board = backupBoard;
            shapeCorrect = shape;
            return false;
        }
        Roll = indexRoll;
        pos = position;
        type = typeshape;
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
     //   Debug.Log("KHong Co Type Shape !!!!");

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
            case TypeShape.T:
                shape = Shape.Clone(CtrlData.T);
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
                if (r == 3)
                {
                    r = 2;
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

    public bool SplitShape_Raw(Shape shape, int[] row, int[] col)
    {

        List<GameObject> ListShape = shape.ListShape;
        int[,] type = shape.shape;

        Debug.Log(Render(shape.shape));

        List<List<int>> RowAndColumSplit = ConvertToSplitShape(shape, row, col);

        Debug.Log(RenderList(RowAndColumSplit[0]));
        Debug.Log(RenderList(RowAndColumSplit[1]));
        int[] row1 = null;
        int[] col1 = null;
        List<int> ListPointRow = new List<int>();
        List<int> ListPointColumn = new List<int>();

        if (RowAndColumSplit[0].Count != 0)
        {
            row1 = RowAndColumSplit[0].ToArray();
            Debug.Log(row1.Length);


        }




        if (RowAndColumSplit[1].Count != 0)
        {

            col1 = RowAndColumSplit[1].ToArray();

            Debug.Log(col1.Length);




        }

        ListPointRow.Add(0);
        ListPointColumn.Add(0);
        //    List<Vector2> Grounp = GrounpPoint(ListPointRow, ListPointColumn);


        List<int[,]> ListRowtMatrix = new List<int[,]>();
        List<int[,]> ListColumnMatrix = new List<int[,]>();
        List<int[,]> TotalMatrix = new List<int[,]>();



        List<List<int[,]>> ListColumMatrix_1 = new List<List<int[,]>>();

        if (row1 != null)
        {

            Debug.Log("Split Row");

            ListRowtMatrix = SplitRowMatrix(type, row1, ref ListPointRow);

            for (int i = 0; i < ListRowtMatrix.Count; i++)
            {
                Debug.Log(Render(ListRowtMatrix[i]));
            }

        }

        if (ListRowtMatrix.Count != 0 && col1 != null)
        {
            Debug.Log("Split Col");
            for (int i = 0; i < ListRowtMatrix.Count; i++)
            {
                ListColumMatrix_1.Add(SplitColumnMatrix(ListRowtMatrix[i], col1, ref ListPointColumn));


            }
            for (int j = 0; j < ListColumMatrix_1.Count; j++)
            {
                for (int k = 0; k < ListColumMatrix_1[j].Count; k++)
                {
                    Debug.Log(Render(ListColumMatrix_1[j][k]));
                }
            }


            if (ListColumMatrix_1.Count != 0)
            {
                for (int j = 0; j < ListColumMatrix_1.Count; j++)
                {
                    for (int k = 0; k < ListColumMatrix_1[j].Count; k++)
                    {
                        ListColumnMatrix.Add(ListColumMatrix_1[j][k]);
                    }
                }
            }
            for (int i = 0; i < ListColumnMatrix.Count; i++)
            {
                TotalMatrix.Add(ListColumnMatrix[i]);
            }



        }
        else if (col1 != null)
        {
            ListColumnMatrix = SplitColumnMatrix(type, col1, ref ListPointColumn);
            for (int i = 0; i < ListColumnMatrix.Count; i++)
            {
                TotalMatrix.Add(ListColumnMatrix[i]);
            }
            List<Vector2> Points = new List<Vector2>();





        }
        else if (row1 != null)
        {
            for (int i = 0; i < ListRowtMatrix.Count; i++)
            {
                TotalMatrix.Add(ListRowtMatrix[i]);
            }
        }
        else
        {

            TotalMatrix.Add(shape.shape);
        }

        if (TotalMatrix.Count == 0)
        {
            return false;
        }
      

        List<Vector2> List_Position = ConvertToPostionBoard(GrounpPoint(ListPointRow, ListPointColumn), shape.Point);



        for (int i = 0; i < TotalMatrix.Count; i++)
        {

            Debug.Log("SHAPE : " + Render(TotalMatrix[i]));
            if (CountInCube(TotalMatrix[i]) == 0)
                return false;
            Debug.Log("SPAWN");
            SpawnShape(TotalMatrix[i], List_Position[i], shape.IDColor);



        }
       




        Debug.Log("P_ROW : " + RenderList(ListPointRow));
        Debug.Log("P_COL : " + RenderList(ListPointColumn));
        return true;

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
                if (RowIsCheck.Contains(List[j]))
                {
                    continue;
                }

                if (List[j].Point.x == i)
                {
                    Row.Add(List[j]);
                    RowIsCheck.Add(List[j]);
                }
            }
           
           //for(int z = 0; z < RowIsCheck.Count; z++)
            ////{
            ////    List.Remove(RowIsCheck[z]);
            ////}
            //////Row = SortRowShape(Row);
            Row = SortListRown(Row);
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
    public List<Shape> SortListRown(List<Shape> shape)
    {
        int[,] Board_Clone = new int[Row, Column];
        List<Shape> Sort = new List<Shape>();
        List<Shape> Shape_0 = new List<Shape>();
        List<Shape> Shape_1 = new List<Shape>();
        List<Shape> NewSort = new List<Shape>();
        //List<List<Vector2>> ListShape = PushListCubeInList(shape);
        //for (int i = 0; i < ListShape.Count; i++)
        //{
        //    for (int j = 0; j < ListShape[i].Count; j++)
        //    {
        //        Vector2 point = ListShape[i][j];

        //        Board_Clone[(int)point.y, (int)point.x] = 1;
        //    }
        //}
        //for (int i = 0; i < ListShape.Count; i++)
        //{
        //   //if(ListShape[i].shap)
        //}

        for(int i = 0; i < shape.Count; i++)
        {
            if (shape[i].shape.GetLength(0) != 1)
            {
                Shape_1.Add(shape[i]);
            }
            else
            {
                Shape_0.Add(shape[i]);
            }
        }

           
         //   Debug.Log("SORT ROW        ");
        //while (isCheckDown(Board_Clone, 1, ListShape))
        //    for (int i = 0; i < ListShape.Count; i++)
        //    {
        //           Debug.Log(shape[i].name +" : " + i + "  :" + Render(Board_Clone));
        //        if (isMoveDown(Board_Clone, 1, ListShape[i]))
        //        {

        //            SimulateMoveDown(ListShape[i], 1, Board_Clone);


        //              Debug.Log(" : " + i + "  :" + Render(Board_Clone));
        //        }
        //        else
        //        {
        //            Sort.Add(shape[i]);                    
        //            ResetMove(ListShape[i], Board_Clone);
        //        }


        //    }


        //for(int i = Sort.Count - 1; i >= 0; i--)
        //{
        //    NewSort.Add(Sort[i]);
        //}





        for (int i = 0; i < Shape_1.Count; i++)
        {
            Sort.Add(Shape_1[i]);
        }
        for (int i = 0; i < Shape_0.Count; i++)
        {
            Sort.Add(Shape_0[i]);
        }
        return Sort;
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
                    ResetMove(List_Shape[i],  Board);
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
        if (CtrlData.CountPlay % 2 == 0)
        {
            CtrlData.CountPlay++;
            ManagerAds.Ins.ShowInterstitial();
        }
        GameManager.Ins.isGamePause = true;
    }
    public void RemuseGame()
    {
        GameManager.Ins.isGamePause = false;
    }

   
   


    #endregion


   

}






