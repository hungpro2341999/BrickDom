using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CtrlGamePlay : MonoBehaviour
{
    public Text Matrix;
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

    #region localVariable
    bool initPos = false;
    public  Vector2 PosInit;
    public  Vector2 PosCurr;
    #endregion

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
        Matrix.text = Render(Board);
        if (Input.GetKeyDown(KeyCode.S))
        {
         
            SpawnShape();
          
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            RefershBoard();
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Debug.Log("Start Destroy");
            DestroyCube(9);
        }
        if (Input.GetMouseButtonDown(0))
        {
            initPos = true;
            Vector3 posInit = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            BackUpBoard = Board;
            PosInit = PointToMatrix(posInit);
           
            Debug.Log("Click");
            isClick = true;

            
        }
        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("DOWN MOUSE");
            isClick = false;
        
        }
        if (isClick)
        {
            Debug.Log(UpdatePoint(Input.mousePosition));
            var Shape = isShapeClick();
            if (Shape != null)
            {
             
            }
        }
        else
        {
            initPos = false; 
        }
    }
   
    
    public  Vector2 PointToMatrix(Vector2 Pos)
    {
        int x = Mathf.FloorToInt((Pos.x - CtrlGamePlay.Ins.initPoint.x) / CtrlGamePlay.Ins.offsetX);
        int y = Mathf.FloorToInt((CtrlGamePlay.Ins.initPoint.y - Pos.y) / CtrlGamePlay.Ins.offsetY);
      //  Debug.Log(x + "  " + y);
        if (x<0 ||  x > countX || y <0 || y >countY)
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
       if((PosCurr.x - PosInit.x) == 0)
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
                Debug.Log(rayHit.transform.name);
                if(rayHit.collider.gameObject.layer == LayerShape)
                {
                    Shape shape = rayHit.collider.gameObject.GetComponent<Shape>();
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
        var  a =  Instantiate(PrebShape, new Vector3(initPoint.x + x * offsetX, initPoint.y),Quaternion.identity,null);
        a.name = "Shape : " + Random.Range(0, 100);
        List_Shape.Add(a.GetComponent<Shape>());
        a.GetComponent<Shape>().AddCubeToBoard();
        
    }
   
    public void AddCubeIntoBoard(GameObject cube)
    {
        if (!Cubes.Contains(cube))
        {
            Cubes.Add(cube);
            //Vector2 point = cube.GetComponent<DestroySelf>().Point;
            //for (int i = 0; i < countY; i++)
            //{
            //    for (int j = 0; j < countX; j++)
            //    {
            //        if (j == point.x && i == point.y)
            //        {
            //            Board[j, i] = 1;
            //        }

            //    }
            //}
            //Render(Board);
        }
       

    }
   
    private void PushToBoard(GameObject cube)
    {
        Vector2 point = cube.GetComponent<DestroySelf>().Point;
        for (int i = 0; i < countY; i++)
        {
            for (int j = 0; j < countX; j++)
            {
                if (j == point.x && i == point.y)
                {
                    Board[j, i] = 1;
                }

            }
        }
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

    public void RefershBoard()
    {
        for(int i = 0; i < List_Shape.Count; i++)
        {
         
            for (int j = 0; j < List_Shape[i].ListShape.Count; j++)
            {
                PushToBoard(List_Shape[i].ListShape[j]);
            }
        }
    }
    

    public void DestroyCube(int row)
    {
       
      for(int i = 0; i < countX; i++)
        {
            for (int x = 0; x < Cubes.Count; x++)
            {
                Debug.Log(Cubes[x].GetComponent<DestroySelf>().Point.ToString());
                if (Cubes[x].GetComponent<DestroySelf>().Point == new Vector2((float)i,row))
                {
                    Cubes[x].GetComponent<DestroySelf>().Destroy();
                    Debug.Log("REMOVE : ");
                }
            }
        }
           
           
          
        
    }
    
    public void SimulateBrickDown(Shape brick)
    {

    }
    public void Check_Shape()
    {
        for(int i = 0; i < List_Shape.Count; i++)
        {

            var a = List_Shape[i];
            List_Shape.Remove(a);
            a.Destroy();

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
    public static bool IsCorrect(float x,float y)
    {

    }
    
   

}
