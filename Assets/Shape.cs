using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public enum TypeShape {crossBar_1,crossBar_2,crossBar_3, crossBar_4, square,three_cube,L_3,None}
public class Shape : MonoBehaviour
{

    public SpriteRenderer ImgCube;
    public Color ColorShape;
    public int idShape;
    public int roll = 0;
    public int CountRoll = 0;
    public TypeShape TypeShape;
    public int[,] shape;
    public int MaxLength;
    public GameObject PrebShape;
    public List<GameObject> ListShape = new List<GameObject>();
   
    public Vector2 point;
    bool initPos = false;
    public Rigidbody2D Body;
    
    #region Move

    public Vector3 posInit;
    public Vector3 PosTarget;
    public float Speed = 0;
    public bool m_isMove = false;
    public bool isClick = false;
    public float ClampMoveMinX = 0;
    public float ClampMoveMaxX = 0;

    #endregion
    // Start is called before the first frame update
    void Start()
    {
     
        Body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {


        }

        if (!isClick)
        {
            PushToBoard();
        }
      
        if (Input.GetKeyDown(KeyCode.W))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
      
        if (isClick)
        {
             
            Vector3 pos = PosTarget;

            pos.x = Mathf.Clamp(pos.x, ClampMoveMinX, ClampMoveMaxX);

            PosTarget = pos;
           
            transform.position = Vector3.MoveTowards(transform.position, PosTarget, Time.deltaTime * Speed);
        }

    }
    public void isMove(int vertical)
    {

    }

    public void AddCubeToBoard()
    {
       
           init();
           TypeShape = RandomShape();
        // initShape(TypeShape);
        initShape(TypeShape.crossBar_2);
        for (int i = 0; i < ListShape.Count; i++)
        {
            CtrlGamePlay.Ins.AddCubeIntoBoard(ListShape[i]);
        }
    }
    public void AddCubeToBoard(int[,] type,Vector2 pos,Color color)
    {
        init();
        SetShape(type);
        for (int i = 0; i < ListShape.Count; i++)
        {
            CtrlGamePlay.Ins.AddCubeIntoBoard(ListShape[i]);
        }
        CtrlGamePlay.Ins.List_Shape.Add(this);
    }

    #region InitShape



    public void SetNextId()
    {

    }
    public void init()
    {
        ColorShape = RandomColor();
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
                GenerateRandom();

                break;
            case TypeShape.crossBar_2:
                shape = new int[4, 4]
                {
                    { 1, 1, 0, 0} ,
                    { 0, 0, 0, 0} ,
                    { 0, 0, 0, 0} ,
                    { 0, 0, 0, 0} ,
                };
                GenerateRandom();

                break;
            case TypeShape.crossBar_3:
                shape = new int[4, 4]
              {
                    { 1, 1, 1, 0} ,
                    { 0, 0, 0, 0} ,
                    { 0, 0, 0, 0} ,
                    { 0, 0, 0, 0} ,
              };
                GenerateRandom();


                break;
            case TypeShape.crossBar_4:
                shape = new int[4, 4]
              {
                    { 1, 1, 1, 1} ,
                    { 0, 0, 0, 0} ,
                    { 0, 0, 0, 0} ,
                    { 0, 0, 0, 0} ,
              };
                GenerateRandom();


                break;
            case TypeShape.square:
                shape = new int[4, 4]
            {
                    { 1, 1, 0, 0} ,
                    { 1, 1, 0, 0} ,
                    { 0, 0, 0, 0} ,
                    { 0, 0, 0, 0} ,
            };
                GenerateRandom();

                break;
            case TypeShape.three_cube:
               
                shape = new int[4, 4]
                {

                    { 1, 0, 0, 0} ,
                    { 1, 1, 0, 0} ,
                    { 0, 0, 0, 0} ,
                    { 0, 0, 0, 0} ,
                };

                //while (!Spawn)
                //{
                  
                ////    shape = extendMatrix(shape);
                ////    RotationMaxtrix(Random.Range(0, 8));
                ////    // Debug.Log(Render(shape));
                  
                ////    shape = SplitMatrix(shape);
                ////    //   Debug.Log(Render(shape));
                  
                ////    Debug.Log(Render(shape));
                ////    Spawn = SetShape();
                ////}
                ////GenerateShape();
                GenerateRandom();

                break;
            case TypeShape.L_3:
                shape = new int[4, 4]
            {

                    { 1, 1, 0, 0} ,
                    { 1, 0, 0, 0} ,
                    { 1, 0, 0, 0} ,
                    { 1, 0, 0, 0} ,
            };
                GenerateRandom();

                break;
        }
     


    }
    public void GenerateRandom()
    {
        int[,] backup = Clone(shape);
        bool Spawn = false;
        Debug.Log(Render(shape));
        while (!Spawn)
        {
            shape = backup;
            shape = extendMatrix(shape);
            switch (TypeShape)
            {
                case TypeShape.three_cube:
                    CountRoll = CtrlData.Ins.Img_Cube_3.Count;
                    int i = Random.Range(0, CountRoll);
                    roll = i;
                    ImgCube.sprite = CtrlData.Ins.Img_Cube_3[roll];

                    shape = RotationMaxtrix(roll);
                    break;
                case TypeShape.crossBar_1:

                    break;
                case TypeShape.crossBar_2:

                    CountRoll = CtrlData.Ins.Img_Cube_Cross_2.Count;
                    int i1 = Random.Range(0, CountRoll);
                    roll = i1;
                    ImgCube.sprite = CtrlData.Ins.Img_Cube_Cross_2[roll];

                    shape = RotationMaxtrix(roll);
                    break;
                case TypeShape.crossBar_3:
                    break;
                case TypeShape.crossBar_4:
                    break;
                case TypeShape.square:
                    break;
                case TypeShape.L_3:
                    break;

            }
         
            
          //   Debug.Log(Render(shape));

            shape = SplitMatrix(shape);
            Debug.Log( Render(shape));

       //     Debug.Log(Render(shape));
            Spawn = SetShape();
        }
        GenerateShape();
    }
   
    public void SetShape(int[,] shape)
    {
       this.shape = shape;
        SetGenerateShape();
    }
    public void InitShapeRandom()
    {
        // Init
        RotationMaxtrix(Random.Range(0, 8));
        shape = SplitMatrix(shape);
        shape = extendMatrix(shape);


    }
    


    public static int[,]  SplitMatrix(int[,] matrix)
    {
        Render(matrix);
        int x = -1;
        int y = -1;
        int xMatrixSlip = 100;
        int yMatrixSlip = 100;
        int xSlip = 0;
        int ySlip = 0;
        // Check Width
        int width = 0;
        int height = 0;
        string s = "";
        for (int j = 0; j < matrix.GetLength(1); j++)
        {
            s += "\n";



            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                s += matrix[i, j];
                if (matrix[i, j] != 0)
                {
                    xMatrixSlip = Mathf.Min(xMatrixSlip, j);
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
                if (matrix[j, i] != 0)
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

        int[,] MatrixSlip = new int[xSlip, ySlip];
        //   Debug.Log(xMatrixSlip + "  Size   " + yMatrixSlip);
        //  Debug.Log(xSlip + "   " + ySlip);
        //Get Slip Matrix
        string ss = "";
        ss += "Colum : " + xMatrixSlip + "  " + "WIDTH :" + yMatrixSlip + "\n";
        for (int j = xMatrixSlip; j < ySlip + xMatrixSlip; j++)
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
        //   Debug.Log(ss);
     //    Debug.Log(Render(MatrixSlip));
        
        return MatrixSlip;


    }
    public int[,] RotationMaxtrix(int countRoll)
    {

        int[,] cloneMatrix = Clone(shape);

        int count = 4;
        for (int x = 0; x < countRoll; x++)
        {
            count = 4;
            for (int i = 0; i < 4; i++)
            {
                count--;
                for (int j = 0; j < 4; j++)
                {

                    shape[i, j] = cloneMatrix[j, count];
                }
            }
            cloneMatrix = Clone(shape);


        }
        return cloneMatrix;


    }
    public static int[,] extendMatrix(int[,] Matrix)
    {
        int[,] array = new int[4, 4];
        for(int j= 0 ; j<Matrix.GetLength(1);j++)
        {
          for(int i = 0; i < Matrix.GetLength(0); i++)
            {
                array[i, j] = Matrix[i, j];

            }
        }
        Matrix = array;
        return Matrix;
    }
    public static int[,] Clone(int[,] matrix)
    {
        int[,] matrixs = new int[matrix.GetLength(0), matrix.GetLength(1)];
        for (int i = 0; i < matrix.GetLength(1); i++)
        {

            for (int j = 0; j < matrix.GetLength(0); j++)
            {
                int x = matrix[j, i];
                matrixs[j, i] = x;
            }
        }
        return matrixs;

    }
    public static string Render(int[,] matrix)
    {
        string s = "";
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            s += "\n";
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                s += matrix[i, j] + "  ";
            }
        }
        return s;
        //    Debug.Log(s);
    }
    private bool SetShape()
    {


        transform.position = CtrlGamePlay.RandomPosShape();
        Render(shape);
        
        for(int y = 0; y < shape.GetLength(1); y++)
        {
            
            for(int x = 0; x < shape.GetLength(0); x++)
            {
                if (shape[x,y] != 0)
                {
                    //  PointShape[x,y] = new Vector3(y * offsetX, -x * offsetY);

                    //var a = Instantiate(PrebShape, transform);
                    //a.transform.localPosition = new Vector3(x * offsetX, -y * offsetY);
                    //Vector2 point = CtrlGamePlay.PositonToMatrixRound(a.transform.position.x, a.transform.position.y);
                    //Debug.Log(point.ToString() + "   " + CtrlGamePlay.isInMatrix((int)point.x, (int)point.y));

                    //i++;
                    //a.name = "Shape " + i;

                    //ListShape.Add(a);

                   //    var a = Instantiate(PrebShape, transform);
                    Vector3 pos = new Vector3(transform.position.x + x * CtrlGamePlay.Ins.offsetX,transform.position.y - y * CtrlGamePlay.Ins.offsetY);
                    Vector2 point = CtrlGamePlay.PositonToMatrixRound(pos.x,pos.y);
                    if(!CtrlGamePlay.isInMatrix((int)point.x, (int)point.y))
                    {
                        return false;
                    }

                    
                  //  Debug.Log(point.ToString() + "   " + CtrlGamePlay.isInMatrix((int)point.x, (int)point.y));

                  

                  
                }

            }
        }
        
        return true;
        
    }

    public void SetGenerateShape()
    {

        for (int y = 0; y < shape.GetLength(0); y++)
        {

            for (int x = 0; x < shape.GetLength(1); x++)
            {
                if (shape[y,x] != 0)
                {

                    var a = Instantiate(PrebShape, transform);
                    a.transform.localPosition = new Vector3(x * CtrlGamePlay.Ins.offsetX, -y * CtrlGamePlay.Ins.offsetY);
                    a.name = idShape.ToString();
                    ListShape.Add(a);

                }
                this.idShape++;
            }
        }
    }

    public void GenerateShape()
    {
       
        for (int y = 0; y < shape.GetLength(0); y++)
        {

            for (int x = 0; x < shape.GetLength(1); x++)
            {
                if (shape[y, x] != 0)
                {

                      var a = Instantiate(PrebShape, transform);
                      a.transform.localPosition = new Vector3(y * CtrlGamePlay.Ins.offsetY, -x *CtrlGamePlay.Ins.offsetX);
                      a.name =  idShape.ToString();
                      ListShape.Add(a);
                   
                }
                this.idShape++;
            }
        }
    }
    public TypeShape RandomShape()
    {
    
        System.Array values = System.Enum.GetValues(typeof(TypeShape));

        TypeShape typeShape = (TypeShape)values.GetValue(UnityEngine.Random.Range(0,values.Length));
        return typeShape;
    } 
  
    #endregion

    #region PushToBoard


    public Vector2[] PushToBoard()
    {
        List<Vector2> ListPoint = new List<Vector2>();
        for(int i = 0; i < ListShape.Count; i++)
        {
            try
            {
                int x = Mathf.Abs(Mathf.RoundToInt((ListShape[i].transform.position.x - CtrlGamePlay.Ins.initPoint.x) / CtrlGamePlay.Ins.offsetX));
                int y = Mathf.Abs(Mathf.RoundToInt((ListShape[i].transform.position.y - CtrlGamePlay.Ins.initPoint.y) / CtrlGamePlay.Ins.offsetY));
                Vector2 point = new Vector2(x, y);
                //   ListPoint.Add(point);
                ListShape[i].GetComponent<DestroySelf>().Point = point;
            }
            catch(System.Exception e)
            {
                
            }
           
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
    public void CheckDestroy()
    {
        if (ListShape.Count == 0)
        {
            CtrlGamePlay.Ins.List_Shape.Remove(this);

            Destroy(gameObject);
        }
      
       
    }
    public void MoveTo(float distance)
    {
        Vector3 pos = posInit;
        pos.x -= distance;
        PosTarget = pos;
    }
   
    public void ActiveRigidBody2D(bool active)
    {

      
        

    }

 

    #endregion
    public void InitPoint()
    {
        isClick = true;
        m_isMove = true;
        posInit = transform.position;
    }
    public void ResetStatus()
    {
        isClick = false;
        m_isMove = false;
        posInit = Vector3.zero;
    }
    public void Snap()
    {
        //for(int i = 0; i < ListShape.Count; i++)
        //{

        //   var a = ListShape[i].GetComponent<DestroySelf>();
        //   Vector2 point  = a.Point;
        //   Vector2 pos = CtrlGamePlay.MatrixToPoint((int)point.x, (int)point.y);
        //   a.transform.position = pos;
        //}
        Vector2 pos = transform.position;
        Vector2 point =  CtrlGamePlay.PositonToMatrixRound(pos.x, pos.y);
      //  Debug.Log("POS SNAP "+point.ToString());
        transform.position = new Vector3(CtrlGamePlay.Ins.initPoint.x + point.x * CtrlGamePlay.Ins.offsetX,pos.y);
      //  Debug.Log("POSITON SNAP : "+ CtrlGamePlay.MatrixToPoint((int)point.x, (int)point.y));
      //  Debug.Log("POSITON SNAP : "+ CtrlGamePlay.MatrixToPoint((int)point.x, (int)point.y));



    }
  
   public void SetUpClamp(float min,float max)
    {
        ClampMoveMinX = transform.position.x - min;

        ClampMoveMaxX = transform.position.x + max;




    }
    public void ReflectShape() 
    {
        string s = "";
    
        
        Render(shape);
        ResetMatrix(shape);
     //   Debug.Log(Render(shape));
     //   Debug.Log(shape.GetLength(0) + "  " + shape.GetLength(1));
        for (int i = 0; i < shape.GetLength(1); i++)
        {
            s += "\n";
            for (int j = 0; j < shape.GetLength(0); j++)
            {
                int x = j + i * shape.GetLength(0);

                s += x.ToString()+"  ";
            for(int z = 0; z < ListShape.Count; z++)
                {
                    if (ListShape[z].name == x.ToString())
                    {
                        shape[j,i] = 1; 
                    }
                }
            
            }
        }
         
      //   shape = SplitMatrix(shape);
    //    Debug.Log(Render(shape));
      
    
           
      
    }
   
    public static void ResetMatrix(int[,] matrix)
    {
        for(int i=0;i< matrix.GetLength(1); i++)
        {
            for(int j=0;j< matrix.GetLength(0); j++)
            {
                matrix[j, i] = 0;
            }
        }
    }
    
  
    public int CountCube()
    {
        return ListShape.Count;
    }
    public Color RandomColor()
    {
    Color background = new Color(
    (float)Random.Range(0, 255),
    (float)Random.Range(0, 255),
    (float)Random.Range(0, 255)
    );
        return background;
    }
    public void DestroyAllCubeAndShape()
    {
        for(int i=0;i< ListShape.Count; i++)
        {
            CtrlGamePlay.Ins.Cubes.Remove(ListShape[i]);
        }
        CtrlGamePlay.Ins.List_Shape.Remove(this);
        Destroy(gameObject);
    }
    public void RenderShape()
    {

    }

   
    
   
}
