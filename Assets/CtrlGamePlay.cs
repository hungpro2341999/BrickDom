using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CtrlGamePlay : MonoBehaviour
{

    public static int idShape = 0;
    public static CtrlGamePlay Ins;
    public int countX;
    public int countY;
    public int[,] Board;
    public int[,] BackUpBoard;
    public Vector2 initPoint;
    public float offsetX;
    public float offsetY;
    public GameObject PrebShape;
    public List<GameObject> Cubes = new List<GameObject>();
    public List<Shape> List_Shape = new List<Shape>();
    public bool isClick = false;
    public Vector2 Destroy;
    public LayerMask LayerShape;
    public Shape ShapeClick = null;
    public List<Vector2> neighbor = new List<Vector2>();
    #region localVariable
    bool initPos = false;
    public Vector2 PosInit;
    public Vector2 PosCurr;

    #endregion
    #region Mouse
    public Vector2 PosMouseInit;
    public Vector2 PosMouseCurr;

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
    }


    void Start()
    {

        Board = new int[countX, countY];
    }

    // Update is called once per frame
    void Update()
    {
        if (ShapeClick != null)
        {

        }
        Matrix.text = Render(Board);
        if (Input.GetKeyDown(KeyCode.S))
        {

            SpawnShape();

        }


        if (Input.GetKeyDown(KeyCode.Z))
        {
            Debug.Log("Start Destroy");


        }
        if (Input.GetMouseButtonDown(0))
        {
            initPos = true;
            Vector3 posInit = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            BackUpBoard = Board;
            PosInit = PointToMatrix(posInit);
            PosMouseInit = Camera.main.ScreenToWorldPoint(Input.mousePosition);


            isClick = true;

            ShapeClick = isShapeClick();
            if (ShapeClick != null)
            {
                ClampShape(ShapeClick);
                ShapeClick.InitPoint();

            }
        }
        if (Input.GetMouseButtonUp(0))
        {

            isClick = false;

        }
        if (isClick)
        {
            int Direct = UpdatePoint(Input.mousePosition);
            float dis = 0;

            if (ShapeClick != null)
            {

                ActiveShape(false);
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


            }
            RefershBoard();
            CheckDestroyRow();
            // Reset Staus
            ActiveShape(true);
            initPos = false;

            // Destroy Row



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
        if (x < 0 || x >= countX || y < 0 || y > countY)
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

        int x = Random.Range(0, countX);
        var a = Instantiate(PrebShape, null);
        a.name = "Shape : " + CtrlGamePlay.idShape;
        idShape++;
        List_Shape.Add(a.GetComponent<Shape>());
        a.GetComponent<Shape>().AddCubeToBoard();

    }

    public static Vector3 RandomPosShape()
    {
        int x = Random.Range(0, CtrlGamePlay.Ins.countX);
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
            Board[(int)point.x, (int)point.y] = 1;
        }
        catch
        {
            Error.text = " ERROR : " + point.x + ":" + point.y;
        }

    }
    public bool isCubeCorrect(GameObject cube)
    {
        Vector2 point = cube.GetComponent<DestroySelf>().Point;
        for (int i = 0; i < countY; i++)
        {
            for (int j = 0; j < countX; j++)
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
        for (int j = 0; j < countY; j++)
        {
            s += "\n";
            for (int i = 0; i < countX; i++)
            {


                s += Board[i, j] + "  ";

                if (i == x && j == y && Board[i, j] == 1)
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
        for (int i = 0; i < countX; i++)
        {
            for (int x = 0; x < Cubes.Count; x++)
            {

                if (Cubes[x].GetComponent<DestroySelf>().Point == new Vector2((float)i, row))
                {
                    shape = Cubes[x].GetComponent<DestroySelf>().shape;
                    Cubes[x].GetComponent<DestroySelf>().Destroy();
                    shape.ReflectShape();
                    //    Debug.Log(shape.name);


                }
            }
        }

        RefershBoard();


    }

    private string Render(int[,] matrix)
    {
        string s = "";
        for (int i = 0; i < matrix.GetLength(1); i++)
        {
            s += "\n";
            for (int j = 0; j < matrix.GetLength(0); j++)
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
        Board = new int[countX, countY];
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
        if (point.x < 0 || point.x > CtrlGamePlay.Ins.countX || point.y < 0 || point.y > CtrlGamePlay.Ins.countY)
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
        if (x < 0 || x >= CtrlGamePlay.Ins.countX || y < 0 || y >= CtrlGamePlay.Ins.countY)
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
    public void ActiveShape(bool active)
    {
        for (int i = 0; i < List_Shape.Count; i++)
        {
            List_Shape[i].GetComponent<Rigidbody2D>().isKinematic = !active;

        }
    }

    public bool DestroyAtRow(out int[] row)
    {
        List<int> Row = new List<int>();
        bool isDestroy = false;
        for (int i = 0; i < countY; i++)
        {
            int count = countX;
            for (int j = 0; j < countX; j++)
            {

                if (Board[j, i] == 1)
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
                    minX = Mathf.Clamp(minX, 0, countX);
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
                    maxX = Mathf.Clamp(maxX, 0, countX);
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

        //MoveSelect.text = s;
        //Debug.Log(s);


    }
    public bool IsCanMove(DestroySelf Cube, int x, int y)
    {
        if (!isInMatrix(x, y))
        {
            return false;
        }


        string s = "";
        for (int j = 0; j < countY; j++)
        {
            s += "\n";
            for (int i = 0; i < countX; i++)
            {


                s += Board[i, j] + "  ";

                if (i == x && j == y && Board[i, j] == 1 && !isCubeInShape(Cube, new Vector2(i, j)))
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
        List<Shape> splitShape = new List<Shape>();
        List<int> LenghtRow = new List<int>();
        List<int> LenghtColumn = new List<int>();
        int[,] matrixshape = shape.shape;
        for (int j = 0; j < matrixshape.GetLength(0); j++)
        {
            bool Split = false;
            for (int i = 0; i < matrixshape.GetLength(1); i++)
            {
             
              int row = j* matrixshape.GetLength(1) + i;
              LenghtRow.Add(row);
                
                
            }
            if()
        }

    }
    public static bool isConnect(int[] row1, int[] row2)
    {
       // bool isConnect = false;
        for (int i = 0; i < row1.Length; i++)
        {
            if (row1[i] != 1 || row2[i]!=1)
            {
                return false;
            }
        }
        return true;
    }
    public static bool isInMatrix(int x, int y, int[,] matrix)
    {
        if (x < 0 || x > matrix.GetLength(0) || y < 0 || y > matrix.GetLength(1))
        {
            return false;
        }
        return true;
    }










}
   
    




