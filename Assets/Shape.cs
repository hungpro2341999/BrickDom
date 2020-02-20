using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public enum TypeShape {crossBar_1,crossBar_2,crossBar_3, crossBar_4, square,three_cube,L_3,None}
public class Shape : MonoBehaviour
{

    public SpriteRenderer Img_Cube_Cross_1;
    public SpriteRenderer Img_Cube_Cross_2;
    public SpriteRenderer Img_Cube_Cross_3_Horizontal;
    public SpriteRenderer Img_Cube_Cross_3_Vertical;
    public SpriteRenderer Img_Cube_Cross_4_Horizontal;
    public SpriteRenderer Img_Cube_Cross_4_Vertical;
    public SpriteRenderer Img_Cube_3;
    public SpriteRenderer ImgCube_quare;
    public SpriteRenderer ImgCube_L3_0;
    public SpriteRenderer ImgCube_L3_90;
    public Color ColorShape;
    public int idShape;
    public int roll = 0;
    public int indexType = 0;
    public int CountRoll = 0;
    public TypeShape TypeShape;
    public int[,] shape;
    public int MaxLength;
    public GameObject PrebShape;
    public List<GameObject> ListShape = new List<GameObject>();
    public int type = 0;
    public Vector2 point;
    bool initPos = false;
    public Rigidbody2D Body;
    public int BackTo = 0;


    #region Move

    public Vector3 posInit;
    public Vector3 PosTarget;
    public Vector3 PosDown;
    public float Speed = 0;
    public bool m_isMove = false;
    public bool isClick = false;
    public float ClampMoveMinX = 0;
    public float ClampMoveMaxX = 0;
    public float ClampMoveMinY = 0;
    public bool isMovingDown = false;

    #endregion
    // Start is called before the first frame update
    void Start()
    {

        Body = GetComponent<Rigidbody2D>();
    }


    // Update is called once per frame
    void Update()
    {



        PushToBoard();

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
    private void FixedUpdate()
    {
        {
            Vector3 pos1 = transform.position;
            pos1 = new Vector2(pos1.x, Mathf.Clamp(pos1.y, CtrlGamePlay.Ins.ClampY, Mathf.Infinity));
            transform.position = pos1;
        }
    }


    public void isMove(int vertical)
    {

    }

    public void AddCubeToBoard()
    {

        init();
        if (CtrlGamePlay.Ins.IsRandom)
        {


        }
        else
        {
            TypeShape = RandomShape();
        }

        initShape(TypeShape);
        //  initShape(TypeShape.three_cube);

        //  initShape(TypeShape);
        for (int i = 0; i < ListShape.Count; i++)
        {
            CtrlGamePlay.Ins.AddCubeIntoBoard(ListShape[i]);
        }
    }
    public void AddCubeToBoard(int[,] type, Vector2 pos, Color color, int back)
    {
        BackTo = back;
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
                Img_Cube_Cross_1.gameObject.SetActive(true);

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
                Img_Cube_Cross_2.gameObject.SetActive(true);
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

                GenerateRandom();

                break;
        }



    }
    public void Reset()
    {
        Img_Cube_Cross_1.gameObject.SetActive(false);
        Img_Cube_Cross_2.gameObject.SetActive(false);
        Img_Cube_Cross_3_Horizontal.gameObject.SetActive(false);
        Img_Cube_Cross_3_Vertical.gameObject.SetActive(false);
        Img_Cube_Cross_4_Horizontal.gameObject.SetActive(false);
        Img_Cube_Cross_4_Vertical.gameObject.SetActive(false);
        Img_Cube_3.gameObject.SetActive(false);
        ImgCube_quare.gameObject.SetActive(false);
        ImgCube_L3_0.gameObject.SetActive(false);
        ImgCube_L3_90.gameObject.SetActive(false);
    }

    public void GenerateShapeByType()
    {

    }
    public void GenerateRandom()
    {
        Reset();
        Vector3 angle;
        int[,] backup = Clone(shape);
        bool Spawn = false;
        //    Debug.Log(Render(shape));
        while (!Spawn)
        {

            if (TypeShape == TypeShape.L_3)
            {
                //     Debug.Log("Hinh dã biet");
                indexType = Random.Range(0, 2);
                if (indexType == 0)
                {
                    shape = new int[4, 4]
          {

                    { 1, 1, 0, 0} ,
                    { 1, 0, 0, 0} ,
                    { 1, 0, 0, 0} ,
                    { 1, 0, 0, 0} ,
          };
                }
                else
                {
                    shape = new int[4, 4]
         {

                    { 1, 1, 0, 0} ,
                    { 0, 1, 0, 0} ,
                    { 0, 1, 0, 0} ,
                    { 0, 1, 0, 0} ,
         };
                }
                ImgCube_L3_0.gameObject.SetActive(false);
                ImgCube_L3_90.gameObject.SetActive(false);
                ImgCube_L3_0.flipX = false;
                ImgCube_L3_0.flipY = false;
                ImgCube_L3_90.flipX = false;
                ImgCube_L3_90.flipY = false;
                backup = Clone(shape);

            }
            shape = backup;
            shape = extendMatrix(shape);
            //   Debug.Log("BACK UP : " + Render(shape));
            switch (TypeShape)
            {
                case TypeShape.three_cube:
                    Img_Cube_3.gameObject.SetActive(true);

                    roll = Random.Range(0, 4);

                    angle = Img_Cube_3.transform.rotation.eulerAngles;
                    shape = RotationMaxtrix(roll);
                    angle.z = (-90 * roll);
                    Img_Cube_3.transform.rotation = Quaternion.Euler(angle);
                    //   ImgCube.sprite = CtrlData.Ins.Img_Cube_3[roll];


                    break;
                case TypeShape.crossBar_1:
                    Img_Cube_Cross_1.gameObject.SetActive(true);
                    break;
                case TypeShape.crossBar_2:
                    Img_Cube_Cross_2.gameObject.SetActive(true);
                    int i1 = Random.Range(0, 2);
                    roll = i1;
                    shape = RotationMaxtrix(roll);
                    angle = Img_Cube_Cross_2.transform.rotation.eulerAngles;
                    if (i1 == 0)
                    {
                        angle.z = 90;
                    }
                    if (i1 == 1)
                    {
                        angle.z = 0;
                    }
                    Img_Cube_Cross_2.transform.rotation = Quaternion.Euler(angle);





                    break;
                case TypeShape.crossBar_3:

                    int i2 = Random.Range(0, 2);
                    roll = i2;
                    RotationMaxtrix(roll);
                    angle = Img_Cube_Cross_2.transform.rotation.eulerAngles;
                    if (i2 == 0)
                    {
                        Img_Cube_Cross_3_Horizontal.gameObject.SetActive(false);
                        Img_Cube_Cross_3_Vertical.gameObject.SetActive(true);
                        angle.z = 90;
                    }
                    if (i2 == 1)
                    {
                        Img_Cube_Cross_3_Horizontal.gameObject.SetActive(true);
                        Img_Cube_Cross_3_Vertical.gameObject.SetActive(false) ;
                        angle.z = 0;
                    }
                    Img_Cube_Cross_2.transform.rotation = Quaternion.Euler(angle);

                    break;
                case TypeShape.crossBar_4:

                    int i3 = Random.Range(0, 2);
                    roll = i3;
                    shape = RotationMaxtrix(roll);

                    if (i3 == 0)
                    {
                        Img_Cube_Cross_4_Horizontal.gameObject.SetActive(false);
                        Img_Cube_Cross_4_Vertical.gameObject.SetActive(true);

                    }
                    if (i3 == 1)
                    {
                        Img_Cube_Cross_4_Horizontal.gameObject.SetActive(true);
                        Img_Cube_Cross_4_Vertical.gameObject.SetActive(false);

                    }




                    break;
                case TypeShape.square:
                    ImgCube_quare.gameObject.SetActive(true);
                    break;
                case TypeShape.L_3:
                    if (indexType == 0)
                    {
                        int i4 = Random.Range(0, 4);
                        roll = i4;
                        shape = RotationMaxtrix(roll);
                        SetWith_1(roll);
                    }
                    else
                    {
                        int i4 = Random.Range(0, 4);
                        roll = i4;
                        shape = RotationMaxtrix(roll);
                        SetWith_2(roll);
                    }
                    //   Debug.Log("Start Rotation");
                    //   Debug.Log(Render(shape));

                    //  Debug.Log("Roataion");
                    // Debug.Log(Render(shape));

                    break;

            }


            //   Debug.Log(Render(shape));

            shape = SplitMatrix(shape);
            //    Debug.Log( Render(shape));

            //     Debug.Log(Render(shape));
            if (TypeShape != TypeShape.L_3)
            {
                Spawn = SetShape();
            }
            else
            {
                Spawn = SetShape_1();
            }

        }


        if (TypeShape != TypeShape.L_3)
        {
            GenerateShape();

        }
        else
        {
            GenerateShape_Ver_2();

        }
        Snap();

    }
  
    

   
    public void SetWith_1(int i)
    {
        switch (i)
        {
            case 0:

                ImgCube_L3_90.gameObject.SetActive(true);
                ImgCube_L3_90.flipX = true;
                break;
            case 1:
                ImgCube_L3_0.gameObject.SetActive(true);
                ImgCube_L3_0.flipY = true;
                break;
            case 2:
                ImgCube_L3_90.gameObject.SetActive(true);
                ImgCube_L3_90.flipY = true;
                break;
            case 3:
                ImgCube_L3_0.gameObject.SetActive(true);
                ImgCube_L3_0.flipX = true;
                break;

        }
    }
    public void SetWith_2(int i)
    {
        switch (i)
        {
            case 0:
                ImgCube_L3_90.gameObject.SetActive(true);
                ImgCube_L3_90.flipX = true;
                ImgCube_L3_90.flipY = true;
                break;
            case 1:
                ImgCube_L3_0.gameObject.SetActive(true);
                break;
            case 2:
                ImgCube_L3_90.gameObject.SetActive(true);
                break;
            case 3:
                ImgCube_L3_0.gameObject.SetActive(true);
                ImgCube_L3_0.flipX = true;
                ImgCube_L3_0.flipY = true;
                break;

        }
    }
    public void SetShape(int[,] shape)
    {
        this.shape = shape;
        SetGenerateShape();
        TypeShape = CtrlGamePlay.Ins.MatrixToType(shape);
        RenderShape(shape, TypeShape);
    }
    public void InitShapeRandom()
    {
        // Init
        RotationMaxtrix(Random.Range(0, 8));
        shape = SplitMatrix(shape);
        shape = extendMatrix(shape);


    }



    public static int[,] SplitMatrix(int[,] matrix)
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
        for (int j = 0; j < Matrix.GetLength(1); j++)
        {
            for (int i = 0; i < Matrix.GetLength(0); i++)
            {
                array[i, j] = Matrix[i, j];

            }
        }
        Matrix = array;
        return Matrix;
    }
    public static int[,] extendMatrixNotCopy(int[,] Matrix)
    {
        int[,] array = new int[4, 4];
        for (int j = 0; j < Matrix.GetLength(0); j++)
        {
            for (int i = 0; i < Matrix.GetLength(1); i++)
            {
                array[j, i] = Matrix[j, i];

            }
        }

        return array;
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

        for (int y = 0; y < shape.GetLength(0); y++)
        {

            for (int x = 0; x < shape.GetLength(1); x++)
            {
                if (shape[y, x] != 0)
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
                    Vector3 pos = new Vector3(transform.position.x + y * CtrlGamePlay.Ins.offsetX, transform.position.y - x * CtrlGamePlay.Ins.offsetY);
                    Vector2 point = CtrlGamePlay.PositonToMatrixRound(pos.x, pos.y);
                    //    Debug.Log(point.x + "  " + point.y + " : " + CtrlGamePlay.isInMatrix((int)point.x, (int)point.y));
                    if (!CtrlGamePlay.isInMatrix((int)point.x, (int)point.y) || CtrlGamePlay.Ins.Board[y, x] == 1)
                    {
                        return false;
                    }


                    //  Debug.Log(point.ToString() + "   " + CtrlGamePlay.isInMatrix((int)point.x, (int)point.y));




                }

            }
        }

        return true;

    }
    private bool SetShape_1()
    {


        transform.position = CtrlGamePlay.RandomPosShape();
        Render(shape);

        for (int y = 0; y < shape.GetLength(0); y++)
        {

            for (int x = 0; x < shape.GetLength(1); x++)
            {
                if (shape[y, x] != 0)
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
                    Vector3 pos = new Vector3(transform.position.x + x * CtrlGamePlay.Ins.offsetX, transform.position.y - y * CtrlGamePlay.Ins.offsetY);
                    Vector2 point = CtrlGamePlay.PositonToMatrixRound(pos.x, pos.y);
                    //    Debug.Log(point.x + "  " + point.y + " : " + CtrlGamePlay.isInMatrix((int)point.x, (int)point.y));
                    if (!CtrlGamePlay.isInMatrix((int)point.x, (int)point.y))
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
        //   shape = extendMatrix(shape);
        //  shape = SplitMatrix(shape);
        shape = CtrlGamePlay.RemoveRow(shape);
        for (int y = 0; y < shape.GetLength(0); y++)
        {
          

            for (int x = 0; x < shape.GetLength(1); x++)
            {
                if (shape[y, x] != 0)
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
    public void GenerateShape_Ver_2()
    {

        // Debug.Log("SHAPGENERATE ");
        //   Debug.Log(Render(shape));
        for (int y = 0; y < shape.GetLength(0); y++)
        {

            for (int x = 0; x < shape.GetLength(1); x++)
            {
                if (shape[y, x] != 0)
                {

                    var a = Instantiate(PrebShape, transform);
                    a.transform.localPosition = new Vector3(x * CtrlGamePlay.Ins.offsetY, -y * CtrlGamePlay.Ins.offsetX);
                    a.name = idShape.ToString();
                    ListShape.Add(a);

                }
                this.idShape++;
            }
        }
    }
    public void GenerateShape()
    {
        Debug.Log("SHAPGENERATE ");
        Debug.Log(Render(shape));
       
        if (TypeShape == TypeShape.crossBar_4)
        {
            shape = SplitMatrix(shape);
           
            Debug.Log("Matrix has Clip");
            Debug.Log(Render(shape));
        }
        for (int y = 0; y < shape.GetLength(0); y++)
        {

            for (int x = 0; x < shape.GetLength(1); x++)
            {
                Debug.Log(y + " " + x);
                if (shape[y, x] != 0)
                {

                      var a = Instantiate(PrebShape, transform);
                      a.transform.localPosition = new Vector3(y * CtrlGamePlay.Ins.offsetY, -x *CtrlGamePlay.Ins.offsetX);
                      a.name = (y + x * shape.GetLength(0)).ToString();
                      ListShape.Add(a);
                   
                }
                
            }
        }
        if (TypeShape == TypeShape.crossBar_2)
        {
            shape = extendMatrix(shape);
            shape = RotationMaxtrix(1);
            Debug.Log("Matrix has Clip");
            shape = SplitMatrix(shape);
            Debug.Log(Render(shape));

        }
        if (TypeShape == TypeShape.crossBar_3)
        {
            shape = extendMatrix(shape);
            shape = RotationMaxtrix(1);
            Debug.Log("Matrix has Clip");
            shape = SplitMatrix(shape);
            Debug.Log(Render(shape));

        }
        if (TypeShape == TypeShape.crossBar_4)
        {
            shape = extendMatrix(shape);
            shape = RotationMaxtrix(1);
            Debug.Log("Matrix has Clip");
            shape = SplitMatrix(shape);
            Debug.Log(Render(shape));

        }


    }
    public static TypeShape RandomShape()
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
         
                int x = Mathf.Abs(Mathf.RoundToInt((ListShape[i].transform.position.x - CtrlGamePlay.Ins.initPoint.x) / CtrlGamePlay.Ins.offsetX));
                int y = Mathf.Abs(Mathf.RoundToInt((ListShape[i].transform.position.y - CtrlGamePlay.Ins.initPoint.y) / CtrlGamePlay.Ins.offsetY));
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
    public void Snap_1()
    {
        //for(int i = 0; i < ListShape.Count; i++)
        //{

        //   var a = ListShape[i].GetComponent<DestroySelf>();
        //   Vector2 point  = a.Point;
        //   Vector2 pos = CtrlGamePlay.MatrixToPoint((int)point.x, (int)point.y);
        //   a.transform.position = pos;
        //}
        Vector2 pos = transform.position;
        Vector2 point = CtrlGamePlay.PositonToMatrix(pos.x, pos.y);
        //  Debug.Log("POS SNAP "+point.ToString());
        transform.position = new Vector3(CtrlGamePlay.Ins.initPoint.x + point.x * CtrlGamePlay.Ins.offsetX, pos.y);
        //  Debug.Log("POSITON SNAP : "+ CtrlGamePlay.MatrixToPoint((int)point.x, (int)point.y));
        //  Debug.Log("POSITON SNAP : "+ CtrlGamePlay.MatrixToPoint((int)point.x, (int)point.y));



    }


    public void SetUpClamp(float min,float max)
    {
        ClampMoveMinX = transform.position.x - min;

        ClampMoveMaxX = transform.position.x + max;




    }
    public void SetUpClamp(float min)
    {
        PosDown = transform.position;
        PosDown.y -= min;
        isMovingDown = true;
       
        MoveDown();

    }
    public void ContinueMoveDown(int min)
    {
        PosDown = transform.position;
        PosDown.y -= min;

    }
    public void MoveDown()
    {
             Vector3 pos = transform.position;
             transform.position = Vector3.MoveTowards(transform.position, PosDown, Time.deltaTime * Speed);
             pos = transform.position;
               
        
       
    }
       

   
    public void ReflectShape() 
    {
        string s = "";

        Debug.Log(Render(shape));
        ResetMatrix(shape);
      //  shape = extendMatrix(shape);
        Debug.Log(Render(shape));
      //   Debug.Log(shape.GetLength(0) + "  " + shape.GetLength(1));
        for (int z = 0; z < ListShape.Count; z++)
        {
            for (int i = 0; i < shape.GetLength(1); i++)
            {
                s += "\n";
                for (int j = 0; j < shape.GetLength(0); j++)
                {
                    int x = i + j * shape.GetLength(1);

                

                    if (int.Parse(ListShape[z].name) == x)
                    {
                        shape[j, i] = 1;
                        s += x.ToString() + "  : ";
                    }
                    else
                    {
                        s += -1;
                    }


                }
            }
        }
        Debug.Log("ReflectShape");
        Debug.Log(s);
        //    shape = SplitMatrix(shape);
        Debug.Log("Completed ReflectShape");
        Debug.Log(Render(shape));
      
    
           
      
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
    public static int[,] RotationMaxtrix(int[,] Matrix,int countRoll)
    {

        int[,] cloneMatrix = Clone(Matrix);
        int[,] MatrixRotaion = new int[cloneMatrix.GetLength(0), cloneMatrix.GetLength(1)];
        int count = 4;
        for (int x = 0; x < countRoll; x++)
        {
            count = 4;
            for (int i = 0; i < 4; i++)
            {
                count--;
                for (int j = 0; j < 4; j++)
                {

                    MatrixRotaion[i, j] = cloneMatrix[j, count];
                }
            }
            cloneMatrix = Clone(MatrixRotaion);


        }
        return cloneMatrix;


    }
   
    public void RenderShape(int[,] shape, TypeShape type)
    {
        int[,] typeShape = Shape.extendMatrixNotCopy(shape);
        switch (type)
        {


            case TypeShape.crossBar_1:
                Img_Cube_Cross_1.gameObject.SetActive(true);
                if (BackTo!= 0)
                {
                   Vector3 pos =   Img_Cube_Cross_1.transform.localPosition;
                   pos.x += (BackTo)* CtrlGamePlay.Ins.offsetX;
                   Img_Cube_Cross_1.transform.localPosition = pos;
                }
                break;
            case TypeShape.crossBar_2:
                int rotation = 0;
                for(int i = 0; i < 2; i++)
                {
                
                    string s1 = "";
                    s1 += "Shape 1 : \n" + Render(shape) + "  " +"Shape 2 : \n"+  Render(RotationMaxtrix(shape, i)) +"\n" + isMatrixSame(shape, RotationMaxtrix(shape, i)).ToString();
                    Debug.Log(s1);
                    if (isMatrixSame(shape, RotationMaxtrix(shape, i)))
                    {
                        Img_Cube_Cross_2.gameObject.SetActive(true);
                        rotation = i;
                        this.shape = CtrlGamePlay.RemoveRow(shape);
                        Debug.Log("Complete Split : " + Render(shape));
                        break;
                    }
                        
                     

                   
                }
             
         
                Vector3 angle = Img_Cube_Cross_2.transform.rotation.eulerAngles;
                if (rotation == 0)
                {
                    angle.z = 90;
                }
                else if (rotation == 1)
                {
                    angle.z = 0;
                }
               
                Img_Cube_Cross_2.transform.rotation = Quaternion.Euler(angle);
                break;
            case TypeShape.crossBar_3:
                Debug.Log("Back To : " + BackTo);
                Img_Cube_Cross_3_Vertical.gameObject.SetActive(true);
                if (BackTo != 0)
                {
                    Vector3 pos = Img_Cube_Cross_3_Vertical.transform.localPosition;
                    pos.x += (BackTo) * CtrlGamePlay.Ins.offsetX;
                    Img_Cube_Cross_3_Vertical.transform.localPosition = pos;
                }
               
                break;
            case TypeShape.crossBar_4:
                int rotation1 = 0;
                for (int i = 0; i < 2; i++)
                {

                    string s1 = "";
                    s1 += "Shape 1 : \n" + Render(shape) + "  " + "Shape 2 : \n" + Render(RotationMaxtrix(shape, i)) + "\n" + isMatrixSame(shape, RotationMaxtrix(shape, i)).ToString();
                    Debug.Log(s1);
                    if (isMatrixSame(shape, RotationMaxtrix(shape, i)))
                    {
                        rotation1 = i;
                        break;
                    }





                }
                if (rotation1 == 0)
                {
                    Img_Cube_Cross_4_Horizontal.gameObject.SetActive(true);
                }
                else
                {
                    Img_Cube_Cross_4_Vertical.gameObject.SetActive(true);
                }
                

                break;
          
           
            case TypeShape.three_cube:

                break;

        }
    }
    public bool isMove()
    {
        if (Vector3.Magnitude(Body.velocity) == 0 )
        {
            return false;
        }
        return true;
    }
    public static bool isMatrixSame(int[,] matrix1 , int[,] matrix2) 
    {
          if(Render(matrix1) != Render(matrix2))
        {
            return false;
        }
        return true;
    }
   

    public void ActiveShape()
    {
        Body.isKinematic = false;
        gameObject.layer = 8;
        for(int i = 0; i < ListShape.Count; i++)
        {
            ListShape[i].gameObject.layer = 8;
        }
    }






}
