using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public enum TypeShape {crossBar_1,crossBar_2,crossBar_3, crossBar_4, square,three_cube,L_4_0,L4_90,L3_0,L3_90,T,None}
public class Shape : PoolItem
{

    // 0 Green
    // 1 Blue
    // 2 plow
    // 3 red
    // 4 yellow
    public SpriteRenderer Img_Cube_1x1;
    public SpriteRenderer Img_Cube_Cross_1x1;
    public SpriteRenderer Img_Cube_Cross_2_0;
    public SpriteRenderer Img_Cube_Cross_2_90;
    public SpriteRenderer Img_Cube_Cross_3_Horizontal;
    public SpriteRenderer Img_Cube_Cross_3_Vertical;
    public SpriteRenderer Img_Cube_Cross_4_Horizontal;
    public SpriteRenderer Img_Cube_Cross_4_Vertical;

    public SpriteRenderer ImgCube_quare;
    public SpriteRenderer ImgCube_L2_0;
    public SpriteRenderer ImgCube_L2_90;
    public SpriteRenderer ImgCube_L4_0;
    public SpriteRenderer ImgCube_L4_90;
    public SpriteRenderer ImgCube_L3_0;
    public SpriteRenderer ImgCube_L3_90;
    public SpriteRenderer ImgCube_T90;
    public SpriteRenderer ImgCube_T0;
    public List<SpriteRenderer> List_Image = new List<SpriteRenderer>();

    public SpriteRenderer SpriteUse;
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
    public Vector2 PointInitCheck;
    public int IDColor = 0;
    public bool isCubeStart = false;
    public GameObject SimulateColumn;
    public Vector2 Point;
    public string nameSpriteUse;
    public bool isEff = false;
    public int CubeStart;
    public List<GameObject> Eff = new List<GameObject>();

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
    public float ClampMoveDown = 0;
    private void Awake()
    {

    }
    #endregion
    // Start is called before the first frame update
    void Start()
    {

        transform.parent = CtrlGamePlay.Ins.transGameplay;
    }
    public void Init_List_Image()
    {

        if (!List_Image.Contains(Img_Cube_Cross_1x1))
            List_Image.Add(Img_Cube_Cross_1x1);
        if (!List_Image.Contains(Img_Cube_Cross_2_0))
            List_Image.Add(Img_Cube_Cross_2_0);
        if (!List_Image.Contains(Img_Cube_Cross_2_90))
            List_Image.Add(Img_Cube_Cross_2_90);
        if (!List_Image.Contains(Img_Cube_Cross_3_Horizontal))
            List_Image.Add(Img_Cube_Cross_3_Horizontal);
        if (!List_Image.Contains(Img_Cube_Cross_3_Vertical))
            List_Image.Add(Img_Cube_Cross_3_Vertical);
        if (!List_Image.Contains(Img_Cube_Cross_4_Horizontal))
            List_Image.Add(Img_Cube_Cross_4_Horizontal);
        if (!List_Image.Contains(Img_Cube_Cross_4_Vertical))
            List_Image.Add(Img_Cube_Cross_4_Vertical);
        if (!List_Image.Contains(ImgCube_quare))
            List_Image.Add(ImgCube_quare);
        if (!List_Image.Contains(ImgCube_L2_0))
            List_Image.Add(ImgCube_L2_0);
        if (!List_Image.Contains(ImgCube_L2_90))
            List_Image.Add(ImgCube_L2_90);
        if (!List_Image.Contains(ImgCube_L4_0))
            List_Image.Add(ImgCube_L4_0);
        if (!List_Image.Contains(ImgCube_L4_90))
            List_Image.Add(ImgCube_L4_90);
        if (!List_Image.Contains(ImgCube_L3_0))
            List_Image.Add(ImgCube_L3_0);
        if (!List_Image.Contains(ImgCube_L3_90))
            List_Image.Add(ImgCube_L3_90);
        if (!List_Image.Contains(ImgCube_T0))
            List_Image.Add(ImgCube_T0);
        if (!List_Image.Contains(ImgCube_T90))
            List_Image.Add(ImgCube_T90);
    }
    public void Restor_Img()
    {
        if (List_Image.Count <= 0)
        {
            Init_List_Image();
        }





    }

    public void Reset()
    {
        Img_Cube_1x1.gameObject.SetActive(false);


        Img_Cube_Cross_1x1.gameObject.SetActive(false);
        Img_Cube_Cross_2_0.gameObject.SetActive(false);
        Img_Cube_Cross_2_90.gameObject.SetActive(false);
        Img_Cube_Cross_3_Horizontal.gameObject.SetActive(false);
        Img_Cube_Cross_3_Vertical.gameObject.SetActive(false);
        Img_Cube_Cross_4_Horizontal.gameObject.SetActive(false);
        Img_Cube_Cross_4_Vertical.gameObject.SetActive(false);
        ImgCube_quare.gameObject.SetActive(false);
        ImgCube_L2_0.gameObject.SetActive(false);
        ImgCube_L2_90.gameObject.SetActive(false);
        ImgCube_L4_0.gameObject.SetActive(false);
        ImgCube_L4_90.gameObject.SetActive(false);
        ImgCube_L3_0.gameObject.SetActive(false);
        ImgCube_L3_90.gameObject.SetActive(false);
        ImgCube_T0.gameObject.SetActive(false);
        ImgCube_T90.gameObject.SetActive(false);
    }


    // Update is called once per frame
    void Update()
    {
        if (GameManager.Ins.isGamePause || GameManager.Ins.isGameOver)
            return;
            Point = CtrlGamePlay.PositonToPointMatrix(transform.position.x, transform.position.y);

        if (SpriteUse != null)
        {
            if (!SpriteUse.enabled)
            {
                SpriteUse.enabled = true;
            }
        }

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
        if (ListShape.Count <= 0)
        {
            DestroyAllCubeAndShape();
        }
        if (isMovingDown)
        { 
          
            Vector3 pos = transform.position;

            pos.y = Mathf.Lerp(pos.y, ClampMoveDown , Time.deltaTime * CtrlGamePlay.Ins.SpeedMoveDown);

            transform.position = pos;
            if(transform.position.y <= ClampMoveDown + (CtrlGamePlay.Ins.offsetX * 0.1f))
            {
                int offset = Mathf.RoundToInt((CtrlGamePlay.Ins.initPoint.y - transform.position.y) / CtrlGamePlay.Ins.offsetY);
                transform.position = new Vector2(transform.position.x, CtrlGamePlay.Ins.initPoint.y - offset * CtrlGamePlay.Ins.offsetY);
                    

                isMovingDown = false;
            }
        }


    }
    
    public SpriteRenderer Img_Curent()
    {
        foreach(SpriteRenderer s in List_Image)
        {
            if (s.gameObject.activeSelf)
            {
                return s;
            }
        }
        return null;
    }
    public void InitImageUse()
    {
        Init_List_Image();
        SpriteUse = Img_Curent();
     

    }
    public void FadeShape()
    {

        SpriteRenderer FadeShape = SpriteUse;
        Color color = FadeShape.color;
     //   Debug.Log(gameObject.name + "  ::  " + FadeShape.name);
     

        SpriteUse.color = new Color(color.r,color.g,color.b, 0.5f);
    }
    public void NormlizeColor()
    {
        SpriteRenderer FadeShape = SpriteUse;
      //  Debug.Log(gameObject.name + "  ::  " + FadeShape.name);
        Color color = FadeShape.color;
      


        SpriteUse.color = new Color(color.r, color.g, color.b,1);
    }
    public void GrayShape(float time)
    {
        SpriteUse.GetComponent<TurnToGray>().Start_Turn_To_Gray(time);
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

 
   
    
    

    public void GenerateShapeByType()
    {

    }
   
    public void SetWith_L3_0(int i,int color)
    {
        switch (i)
        {
            case 0:
                ImgCube_L3_90.flipX = false;
                ImgCube_L3_90.gameObject.SetActive(true);
                ImgCube_L3_90.flipX = true;
                ImgCube_L3_90.sprite = CtrlData.Ins.DataGame.LoadShape("L3_270")[color];
                SpriteUse = ImgCube_L3_90;
                break;
            case 1:
                ImgCube_L3_0.flipX = false;
                ImgCube_L3_0.gameObject.SetActive(true);
                SpriteUse = ImgCube_L3_0;
                ImgCube_L3_0.sprite = CtrlData.Ins.DataGame.LoadShape("L3_180")[color];
                break;
            case 2:
                ImgCube_L3_90.flipX = false;
                ImgCube_L3_90.gameObject.SetActive(true);
                SpriteUse = ImgCube_L3_90;
                ImgCube_L3_90.flipX = true;
                ImgCube_L3_90.sprite = CtrlData.Ins.DataGame.LoadShape("L3_90")[color];
                break;
            case 3:
                ImgCube_L3_0.flipX = false;
                ImgCube_L3_0.gameObject.SetActive(true);
               
                ImgCube_L3_0.flipX = true;
                SpriteUse = ImgCube_L3_0;
                ImgCube_L3_0.sprite = CtrlData.Ins.DataGame.LoadShape("L3_0")[color];
                break;

        }
    }

    public void SetWith_L3_90(int i,int color)
    {
        switch (i)
        {
            case 0:
                ImgCube_L3_90.flipX = false;
                ImgCube_L3_90.gameObject.SetActive(true);
                SpriteUse = ImgCube_L3_90;
                ImgCube_L3_90.sprite = CtrlData.Ins.DataGame.LoadShape("L3_270")[color];

                break;
            case 1:
                ImgCube_L3_0.flipX = false;
                ImgCube_L3_0.gameObject.SetActive(true);
                SpriteUse = ImgCube_L3_0;
                ImgCube_L3_0.sprite = CtrlData.Ins.DataGame.LoadShape("L3_0")[color];

                break;
            case 2:
                ImgCube_L3_90.flipX = false;
                ImgCube_L3_90.gameObject.SetActive(true);

                SpriteUse = ImgCube_L3_90;
                ImgCube_L3_90.sprite = CtrlData.Ins.DataGame.LoadShape("L3_90")[color];
                break;
            case 3:
                ImgCube_L3_0.flipX = false;
                ImgCube_L3_0.gameObject.SetActive(true);
                SpriteUse = ImgCube_L3_0;
                ImgCube_L3_0.flipX = true;
                ImgCube_L3_0.sprite = CtrlData.Ins.DataGame.LoadShape("L3_180")[color];
                break;

        }
    }



    public void SetWith_1(int i,int color)
    {
        switch (i)
        {
            case 0:

                ImgCube_L4_90.flipX = false;
                ImgCube_L4_90.gameObject.SetActive(true);
              
                ImgCube_L4_90.flipX = true;
                ImgCube_L4_90.sprite = CtrlData.Ins.DataGame.LoadShape("L4_270")[color];
                SpriteUse = ImgCube_L4_90;
                break;
            case 1:
                ImgCube_L4_0.flipX = false;
                ImgCube_L4_0.gameObject.SetActive(true);
                ImgCube_L4_0.sprite = CtrlData.Ins.DataGame.LoadShape("L4_0")[color];
                SpriteUse = ImgCube_L4_0;
                break;
            case 2:
                ImgCube_L4_90.flipX = false;
                ImgCube_L4_90.gameObject.SetActive(true);
                ImgCube_L4_90.sprite = CtrlData.Ins.DataGame.LoadShape("L4_90")[color];
                SpriteUse = ImgCube_L4_90;
                break;
            case 3:
                ImgCube_L4_0.flipX = false;
                ImgCube_L4_0.gameObject.SetActive(true);
                ImgCube_L4_0.sprite = CtrlData.Ins.DataGame.LoadShape("L4_180")[color];
                SpriteUse = ImgCube_L4_0;
                ImgCube_L4_0.flipX = true;
                break;

        }
    }
    public void SetWith_2(int i,int color)
    {
        switch (i)
        {
            case 0:

                ImgCube_L4_90.gameObject.SetActive(true);

                ImgCube_L4_90.flipX = false;

                ImgCube_L4_90.sprite = CtrlData.Ins.DataGame.LoadShape("L4_270")[color];
                SpriteUse = ImgCube_L4_90;
                break;
            case 1:
                ImgCube_L4_0.gameObject.SetActive(true);
                ImgCube_L4_0.flipX = false;
                ImgCube_L4_0.sprite = CtrlData.Ins.DataGame.LoadShape("L4_180")[color];
                SpriteUse = ImgCube_L4_0;
                break;
            case 2:
                ImgCube_L4_0.flipX = false;
                ImgCube_L4_90.gameObject.SetActive(true);
                ImgCube_L4_90.flipX = true;
                ImgCube_L4_90.sprite = CtrlData.Ins.DataGame.LoadShape("L4_90")[color];
                SpriteUse = ImgCube_L4_90;
                break;
            case 3:
                ImgCube_L4_90.flipX = false;
                ImgCube_L4_0.gameObject.SetActive(true);
                ImgCube_L4_0.flipX = true;
                ImgCube_L4_0.sprite = CtrlData.Ins.DataGame.LoadShape("L4_0")[color];
                SpriteUse = ImgCube_L4_0;
                break;

        }
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
        for (int i = 0; i < matrix.GetLength(0); i++)
        {

            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                int x = matrix[i, j];
                matrixs[i, j] = x;
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
    
   

   

    public bool HasCut()
    {
        return CubeStart == ListShape.Count;
    }

   
    public void GenerateShape()
    {
    //   Debug.Log("SHAPGENERATE : "+gameObject.name);
   //     Debug.Log(Render(shape));
      
        for (int y = 0; y < shape.GetLength(0); y++)
        {

            for (int x = 0; x < shape.GetLength(1); x++)
            {
              //  Debug.Log(y + " " + x);
                if (shape[y, x] != 0)
                {

                    var a = Poolers.Ins.GetObject(PrebShape);
                    a.OnSpawn();
                    a.transform.parent = transform;
                    a.GetComponent<DestroySelf>().shape = this;
                      a.transform.localPosition = new Vector3(x * CtrlGamePlay.Ins.offsetY, -y *CtrlGamePlay.Ins.offsetX);
                      a.name = (x + y * shape.GetLength(1)).ToString();
                      ListShape.Add(a.gameObject);
                      CtrlGamePlay.Ins.AddCubeIntoBoard(a.gameObject);
                    if (isCubeStart)
                    {
                        a.gameObject.layer = 8;
                    }
                    else
                    {
                        a.gameObject.layer = 0;
                    }
                }
                
            }
         
        }
        for(int i = 0; i < ListShape.Count; i++)
        {
            ListShape[i].gameObject.SetActive(true);
        }
        CubeStart = ListShape.Count;



    }
    public void SetTypeShape(TypeShape type, int[,] shape)
    {
        TypeShape = type;
        this.shape = shape;
        GenerateShape();
    }

    public static TypeShape RandomShape()
    {
        int r = Random.Range(0, 100);
        TypeShape type = TypeShape.None;
        while (type == TypeShape.None || type == TypeShape.L3_0 || type == TypeShape.L3_90)
        {


            if (r >= 0 && r <= 80 - (CtrlData.Level * 2))
            {

                type = CtrlData.Ins.Type_Ver_1[Random.Range(0, CtrlData.Ins.Type_Ver_1.Count)];
            }
            else if (r > 80 - (CtrlData.Level * 2) && r < 100 - (CtrlData.Level * 2))
            {
                type = CtrlData.Ins.Type_Ver_2[Random.Range(0, CtrlData.Ins.Type_Ver_2.Count)];
            }
            else
            {
                type = CtrlData.Ins.Type_Ver_3[Random.Range(0, CtrlData.Ins.Type_Ver_3.Count)];
            }
          

           

            //System.Array values = System.Enum.GetValues(typeof(TypeShape));

            //type = (TypeShape)values.GetValue(UnityEngine.Random.Range(0, values.Length));

        }
        //if (Random.Range(0, 2) != 0)
        //{
        //    type = TypeShape.three_cube;
        //}
        //else
        //{

        //    type = TypeShape.crossBar_2;
        //}
       
        return type;
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
        if (ListShape.Count==0)
        {
            OnDespawn();
        }
  
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

            OnDespawn();
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
        
        PointInitCheck = CtrlGamePlay.PositonToPointMatrix(transform.position.x, transform.position.y);
     //   CtrlGamePlay.Ins.TextPointInit.text = "Init : " + PointInitCheck.x + "  " + PointInitCheck.y;

      


    }
    public void ResetStatus()
    {
        isClick = false;
        m_isMove = false;
        posInit = Vector3.zero;
        PointInitCheck = new Vector2(-1, -1);
    }
    public Vector2 Snap()
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
        transform.position = new Vector3(CtrlGamePlay.Ins.initPoint.x + point.x * CtrlGamePlay.Ins.offsetX, CtrlGamePlay.Ins.initPoint.y-point.y * CtrlGamePlay.Ins.offsetY);
        //  Debug.Log("POSITON SNAP : "+ CtrlGamePlay.MatrixToPoint((int)point.x, (int)point.y));
        //  Debug.Log("POSITON SNAP : "+ CtrlGamePlay.MatrixToPoint((int)point.x, (int)point.y));
        return point;


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
        
     //   Debug.Log(Render(shape));
        ResetMatrix(ref shape);
      //  shape = extendMatrix(shape);
   //     Debug.Log(Render(shape));
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
       // Debug.Log("ReflectShape");
     //   Debug.Log(s);
        //    shape = SplitMatrix(shape);
        //Debug.Log("Completed ReflectShape");
        //Debug.Log(Render(shape));
      
    
           
      
    }
   
    public static void ResetMatrix(ref int[,] matrix)
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
            ListShape[i].transform.parent = null;
            ListShape[i].gameObject.SetActive(false);
        }
        CtrlGamePlay.Ins.List_Shape.Remove(this);
        OnDespawn();
    }
    public static int[,] RotationMaxtrix(int[,] Matrix,int countRoll)
    {

        int[,] cloneMatrix = CtrlGamePlay.CloneBoard(Matrix);
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
            cloneMatrix = CtrlGamePlay.CloneBoard(MatrixRotaion);


        }
        return cloneMatrix;


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
       // Body.isKinematic = false;
        gameObject.layer = 8;
        for(int i = 0; i < ListShape.Count; i++)
        {
            ListShape[i].gameObject.layer = 8;
        }
    }


    public void GenerateShape(int[,] shape, TypeShape type)
    {
        int r = 0;
        switch (type)
        {
            case TypeShape.crossBar_1:
                shape = CtrlData.Cube_Cross_1;
                r = CtrlData.NotRoll;
                break;
            case TypeShape.crossBar_2:

                shape = CtrlData.Cube_Cross_2;
                r = CtrlData.Roll_Cross;
                break;
            case TypeShape.crossBar_3:

                shape = CtrlData.Cube_Cross_3;
                r = CtrlData.Roll_Cross;
                break;
            case TypeShape.crossBar_4:
                shape = CtrlData.Cube_Cross_4;
                r = CtrlData.Roll_Cross;
                break;
            case TypeShape.square:
                shape = CtrlData.Cube_Quare;
                r = CtrlData.NotRoll;
                break;
            case TypeShape.L_4_0:
                shape = CtrlData.Cube_L4_0;
                r = CtrlData.Roll_Cube_L;
                r = Random.Range(0, CtrlData.Roll_Cube_L);
                break;
            case TypeShape.L4_90:
                shape = CtrlData.Cube_L4_90;
                r = CtrlData.Roll_Cube_L;
                break;
            case TypeShape.L3_0:
                shape = CtrlData.Cube_L3_0;
                r = CtrlData.Roll_Cube_L;
                break;
            case TypeShape.L3_90:
                shape = CtrlData.Cube_L3_90;
                r = Random.Range(0, CtrlData.Roll_Cube_L);
                break;

            case TypeShape.three_cube:
                shape = CtrlData.Cube_3;
                r = CtrlData.Roll_Cube_L;
                break;
        }
    }

   
    public void Set_Up_Corrs(int roll,TypeShape type,int Color)
    {
        IDColor = Color;

        if(type == TypeShape.crossBar_2)
        {
            if (roll == 0) 
            {
            //    Debug.Log("21213231");
                Cross_2_Vertical(Color);
            }
            else
            {
                Cross_2_Horizontal(Color);
            }
            return;
        }
        if(type == TypeShape.square)
        {
            Square(Color);
            return;
        }
        if (type == TypeShape.crossBar_1)
        {

            Cross_1(Color);
            return;
        }
        if(TypeShape == TypeShape.three_cube) 
        {
            RollCube_3(roll,Color);
            return;
        }
        if (TypeShape == TypeShape.L4_90)
        {
            SetWith_1(roll,Color);
            return;
        }
        if (TypeShape == TypeShape.L_4_0)
        {
            SetWith_2(roll,Color);
            return;
        }
        if(TypeShape == TypeShape.L3_0)
        {
            SetWith_L3_0(roll,Color);
        }

        if (TypeShape == TypeShape.L3_90)
        {
            SetWith_L3_90(roll,Color);
        }
        if(TypeShape == TypeShape.crossBar_3)
        {
            if (roll == 0)
            {
                Cross_3_Horizontal(Color);
            }
            else
            {
                Cross_3_Vertical(Color);
            }
        }
        if (TypeShape == TypeShape.crossBar_4)
        {
            if (roll == 0)
            {
                Cross_4_Horizontal(Color);
            }
            else
            {
                Cross_4_Vertical(Color);
            }
        }
        if(TypeShape == TypeShape.T)
        {
            Set_Cube_T(roll, Color);
        }


    }
   
    #region Set_Up_Image
    public void Cross_2_Horizontal(int i)
    {

        Img_Cube_Cross_2_90.gameObject.SetActive(true);
        Img_Cube_Cross_2_90.sprite = CtrlData.Ins.DataGame.LoadShape("2x1")[i];
        SpriteUse = Img_Cube_Cross_2_90;
        nameSpriteUse = Img_Cube_Cross_2_90.name;
    }
    public void Cross_2_Vertical(int i)
    {

        Img_Cube_Cross_2_0.gameObject.SetActive(true);
//        Debug.Log("Anh i : "+i);
        Img_Cube_Cross_2_0.GetComponent<SpriteRenderer>().sprite  = CtrlData.Ins.DataGame.LoadShape("1x2")[i];
        SpriteUse = Img_Cube_Cross_2_0;
        nameSpriteUse = Img_Cube_Cross_2_0.name;
    }
    public void Cross_3_Horizontal(int i)
    {
        Img_Cube_Cross_3_Horizontal.gameObject.SetActive(true);
        Img_Cube_Cross_3_Horizontal.sprite = CtrlData.Ins.DataGame.LoadShape("1x3")[i];
        SpriteUse = Img_Cube_Cross_3_Horizontal;
        nameSpriteUse = Img_Cube_Cross_3_Horizontal.name;

    }
    public void Cross_3_Vertical(int i)
    {
   
        Img_Cube_Cross_3_Vertical.gameObject.SetActive(true);
        Img_Cube_Cross_3_Vertical.sprite = CtrlData.Ins.DataGame.LoadShape("3x1")[i];
        SpriteUse = Img_Cube_Cross_3_Vertical;
        nameSpriteUse = Img_Cube_Cross_3_Vertical.name;
    }

    public void Cross_4_Horizontal(int i)
    {
        Img_Cube_Cross_4_Horizontal.gameObject.SetActive(true);
        Img_Cube_Cross_4_Horizontal.sprite = CtrlData.Ins.DataGame.LoadShape("1x4")[i];
        SpriteUse = Img_Cube_Cross_4_Horizontal;
        nameSpriteUse = Img_Cube_Cross_4_Horizontal.name;
    }

    public void Cross_4_Vertical(int i)
    {
       
        Img_Cube_Cross_4_Vertical.gameObject.SetActive(true);
        Img_Cube_Cross_4_Vertical.sprite = CtrlData.Ins.DataGame.LoadShape("4x1")[i];
        SpriteUse = Img_Cube_Cross_4_Vertical;
        nameSpriteUse = Img_Cube_Cross_4_Vertical.name;
    }

    public void Cross_1(int i)
    {


        Img_Cube_Cross_1x1.gameObject.SetActive(true);

     //   Debug.Log(i);
        Img_Cube_1x1.sprite = CtrlData.Ins.DataGame.LoadShape("1x1")[i];
        SpriteUse = Img_Cube_1x1;
        nameSpriteUse = Img_Cube_1x1.name;
    }

    public void Square(int color)
    {
        ImgCube_quare.gameObject.SetActive(true);
        ImgCube_quare.sprite = CtrlData.Ins.DataGame.LoadShape("Square")[color];
        SpriteUse = ImgCube_quare;
        nameSpriteUse = ImgCube_quare.name;
    }

    public void RollCube_3(int roll,int color)
    {
        switch (roll)
        {
            case 0:
                ImgCube_L2_90.flipX = false;
                ImgCube_L2_90.gameObject.SetActive(true);
                ImgCube_L2_90.sprite = CtrlData.Ins.DataGame.LoadShape("L2_90")[color];
                SpriteUse = ImgCube_L2_90;
                nameSpriteUse = ImgCube_L2_90.name;

                break;
            case 1:
                ImgCube_L2_90.flipX = false;
                ImgCube_L2_90.gameObject.SetActive(true);
                ImgCube_L2_90.sprite = CtrlData.Ins.DataGame.LoadShape("L2_90")[color];
                ImgCube_L2_90.flipX = true;
                SpriteUse = ImgCube_L2_90;
                nameSpriteUse = ImgCube_L2_90.name;
                break;
            case 2:
                ImgCube_L2_0.flipX = false;
                ImgCube_L2_0.gameObject.SetActive(true);
                ImgCube_L2_0.flipX = true;
                if(color == 4)
                {
                    ImgCube_L2_0.flipX = false;
                }
                ImgCube_L2_0.sprite = CtrlData.Ins.DataGame.LoadShape("L2_0")[color];
                SpriteUse = ImgCube_L2_0;
                SpriteUse = ImgCube_L2_0;
                nameSpriteUse = ImgCube_L2_0.name;
                break;

            case 3:
                ImgCube_L2_0.flipX = false;
                ImgCube_L2_0.gameObject.SetActive(true);
                if(color == 4)
                {
                    ImgCube_L2_0.flipX = true;
                }
                SpriteUse = ImgCube_L2_0;
                nameSpriteUse = ImgCube_L2_0.name;
                ImgCube_L2_0.sprite = CtrlData.Ins.DataGame.LoadShape("L2_0")[color];
                break;
        }
    }

    public void Set_Cube_T(int roll, int color)
    {
        switch (roll)
        {
            case 0:
                ImgCube_T0.flipX = false;
                ImgCube_T0.gameObject.SetActive(true);
                ImgCube_T0.sprite = CtrlData.Ins.DataGame.LoadShape("T_180")[color];
                SpriteUse = ImgCube_T0;
               

                break;
            case 1:
                ImgCube_T90.flipX = false;
                ImgCube_T90.gameObject.SetActive(true);
                ImgCube_T90.sprite = CtrlData.Ins.DataGame.LoadShape("T_0")[color];
               
                SpriteUse = ImgCube_T90;
              
                break;
            case 2:
                ImgCube_T0.flipX = false;
                ImgCube_T0.gameObject.SetActive(true);
              
               
                ImgCube_T0.sprite = CtrlData.Ins.DataGame.LoadShape("T_90")[color];
                SpriteUse = ImgCube_T0;
              
              
                break;

            case 3:
                ImgCube_T90.flipX = false;
                ImgCube_T90.gameObject.SetActive(true);
                ImgCube_T90.sprite = CtrlData.Ins.DataGame.LoadShape("T_0")[color];
                ImgCube_T90.flipX = true;
                SpriteUse = ImgCube_T90;

                break;
        }
    }




    #endregion


    public bool isShapeMove(Vector2 point)
    {
        Vector2 pos = transform.position;
     
        //int x = Mathf.FloorToInt((pos.x - CtrlGamePlay.Ins.initPoint.x) / CtrlGamePlay.Ins.offsetX);
        //int y = Mathf.FloorToInt((CtrlGamePlay.Ins.initPoint.y - pos.y) / CtrlGamePlay.Ins.offsetY);
      
        if (point.y == PointInitCheck.y)
        {
            return false;
        }
        
        return true;
    }
    public int GetColumn(Shape shape)
    {

        return shape.shape.GetLength(1);
    }
    public void StartFade()
    {
        StartCoroutine(Fade());
    }
    public IEnumerator Fade()
    {
        Color color = SpriteUse.color;
        while (color.a >= 0.1f)
        {
            color = SpriteUse.color;

            color.a -= 0.5f * Time.deltaTime;

            SpriteUse.color = color;
            yield return new WaitForSeconds(0);
        }
       
    }
    
    public override void OnSpawn()
    {
        m_isMove = false;
        isClick = false;
        isMovingDown = false;
        for(int i = 0; i < List_Image.Count; i++)
        {
            List_Image[i].gameObject.GetComponent<TurnToGray>().Reset();
        }
        transform.parent = CtrlGamePlay.Ins.transGameplay;
        gameObject.layer = 0;
        if (isCubeStart)
        {
            if (SpriteUse != null)
            {
                Color c = SpriteUse.color;
                c.a = 1;
                SpriteUse.color = c;
                isCubeStart = false;
            }
        }
        if (isEff)
        {
            if (SpriteUse != null)
            {
                SpriteUse.GetComponent<_2dxFX_GoldFX>().enabled = false;
            }
            isEff = false;
        }
        Reset();
        //for(int i = 0; i < ListShape.Count; i++)
        //{
        //    ListShape[i].SetActive(false);
        //}

        ListShape = new List<GameObject>();
        
       
       


    }
    public override void OnDespawn()
    {
        gameObject.SetActive(false);
       
    }




}
