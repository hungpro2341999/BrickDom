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
    #region localVariable
    bool initPos = false;
    public  Vector2 PosInit;
    public  Vector2 PosCurr;

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
            ClampShape(ShapeClick);
        }
        Matrix.text = Render(Board);
        if (Input.GetKeyDown(KeyCode.S))
        {
         
            SpawnShape();
          
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            RefershBoard();
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            IsCubeCorrect(xx, yy);
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Debug.Log("Start Destroy");
            DestroyRow(9);
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
                ShapeClick.InitPoint();
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
          
            isClick = false;
        
        }
        if (isClick)
        {
            int Direct =  UpdatePoint(Input.mousePosition);
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
            if (ShapeClick!=null) 
            {
                ShapeClick.ResetStatus();
            }
          
            ShapeClick = null; 
         // Reset Staus
            ActiveShape(true);
            initPos = false; 
        }
    }
   
    
   public static Vector3 ScreenToWord(Vector2 posScreen)
    {
       Vector3 pos =   Camera.main.ScreenToWorldPoint(Input.mousePosition);
        pos.z = 0;
        return pos;
    }
    public  Vector2 PointToMatrix(Vector2 Pos)
    {
        int x = Mathf.FloorToInt((Pos.x - CtrlGamePlay.Ins.initPoint.x) / CtrlGamePlay.Ins.offsetX);
        int y = Mathf.FloorToInt((CtrlGamePlay.Ins.initPoint.y - Pos.y) / CtrlGamePlay.Ins.offsetY);
      //  Debug.Log(x + "  " + y);
        if (x<0 ||  x >= countX || y <0 || y >countY)
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
        return  (int)Mathf.Sign(PosCurr.x - PosInit.x);
        
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

        int x = Random.Range(0,countX);
        var  a =  Instantiate(PrebShape,null);
        a.name = "Shape : " + Random.Range(0, 10);
        List_Shape.Add(a.GetComponent<Shape>());
        a.GetComponent<Shape>().AddCubeToBoard();
        
    }

    public static  Vector3 RandomPosShape()
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
        
        Vector2 point = cube.GetComponent<DestroySelf>().Point;
        Board[(int)point.x,(int)point.y] = 1;  
    }
    public bool isCubeCorrect(GameObject cube)
    {
        Vector2 point = cube.GetComponent<DestroySelf>().Point;
        for (int i = 0; i < countY; i++)
        {
            for (int j = 0; j < countX; j++)
            {
                if (j == point.x && i == point.y && Board[j, i] == 1 )
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
    
    public bool IsCubeCorrect(int x,int y)
    {
        string s = "";
        for (int j = 0; j < countY; j++)
        {
            s += "\n";
            for (int i = 0; i < countX; i++)
            {
           
                try
                {
                    s += Board[i, j] + "  ";
                }
                catch(System.Exception e)
                {
                  
                    Debug.Log(s+ "GotError");

                }
                if (i == x && j == y && Board[i, j] == 1)
                {
                    Debug.Log(i + " " + j);
                    Debug.Log("D Hop Ly");
                    return false;
                }
              

            }
        }
        Debug.Log(s);
        Debug.Log("Hop Ly");
        return true;

    }
    public bool isShapeCorrect(Shape shape,int x,int y)
    {
        for(int i = 0; i < shape.ListShape.Count; i++)
        {
            Vector2 point =   shape.ListShape[i].GetComponent<DestroySelf>().Point;
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
       
      for(int i = 0; i < countX; i++)
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
            Debug.Log("List_Shape : " + count);
            if (count == 0)
            {
                Debug.Log("Destroy_Shape : " + count);
                Debug.Log("Remove");
                List_Shape.Remove(List_Shape[i]);
                //     List_Shape[i].CheckDestroy();
            }
        }
    }
    #endregion


    public static bool  CheckPointCorrect(float PosX, float PosY)
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
    public static bool isInMatrix(int x,int y)
    {
        if (x < 0 || x >= CtrlGamePlay.Ins.countX || y <0 || y > CtrlGamePlay.Ins.countY)
        {
            return false;
        }
        return true;
    }
    public static Vector2 PositonToMatrix(float PosX,float PosY)
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
    public static Vector2 MatrixToPoint(int x,int y)
    {
        Vector2 pos = Vector2.zero;
        pos.x = CtrlGamePlay.Ins.PosInit.x + x * CtrlGamePlay.Ins.offsetX;
        pos.y = CtrlGamePlay.Ins.PosInit.y - y * CtrlGamePlay.Ins.offsetY;
        return pos;
    }
    public void ActiveShape(bool active) 
    {
      for(int i = 0; i < List_Shape.Count; i++)
         {
            List_Shape[i].GetComponent<Rigidbody2D>().isKinematic = !active;

         }
    }
   
    public void ClampShape(Shape shape)
    {
       
        RefershBoard();
        int minX = 0;
        int maxX = 0;
        List<Vector2> Point = new List<Vector2>();
        //for(int i = 0; i < shape.ListShape.Count; i++)
        //{
        //    Point.Add(shape.ListShape[i].GetComponent<DestroySelf>().Point);
            
        //}
       
        //Move_left
         
        bool isMove =true;

        while (isMove)
        {
            minX++;
            for(int i = 0; i < shape.ListShape.Count; i++)
            {
                int x = (int)shape.ListShape[i].GetComponent<DestroySelf>().Point.x - minX;
                int y = (int)shape.ListShape[i].GetComponent<DestroySelf>().Point.y;
                if (IsCubeCorrect(x, y))
                {
                    isMove = false;
                    minX--;
                    minX = Mathf.Clamp(minX, 0, countX);
                    break;
                }
            }
          

        }

        //Move_Right
        isMove = true;

        while (isMove)
        {
            maxX++;
            for (int i = 0; i < shape.ListShape.Count; i++)
            {
                int x = (int)shape.ListShape[i].GetComponent<DestroySelf>().Point.x + maxX;
                int y = (int)shape.ListShape[i].GetComponent<DestroySelf>().Point.y;
                if (IsCubeCorrect(x, y))
                {
                    isMove = false;
                    maxX--;
                    maxX = Mathf.Clamp(maxX, 0, countX);
                    break;
                }
            }


        }

        MoveSelect.text = "X : " + minX + "---Y : " + maxX; 
       


    }










}
   
    




