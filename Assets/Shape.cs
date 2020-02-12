using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public enum TypeShape {crossBar_1,crossBar_2,crossBar_3, crossBar_4, square,three_cube}
public class Shape : MonoBehaviour
{
    public TypeShape TypeShape;
    public int[,] shape;
    public int MaxLength;
    public GameObject PrebShape;
    public List<GameObject> ListShape = new List<GameObject>();
    public float offsetX=0.6f;
    public float offsetY=0.6f;
    public Vector2 point;
    // Start is called before the first frame update
    void Start()
    {
        init();
        
        initShape(TypeShape);
    }

    // Update is called once per frame
    void Update()
    {
         PushToBoard();
        if (Input.GetKeyDown(KeyCode.W))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
          for(int i = 0; i < ListShape.Count; i++)
            {
                CtrlGamePlay.Ins.AddCubeIntoBoard(ListShape[i]);
                Debug.Log("Push");
            }
        }
    }
    #region InitShape
    public void init()
    {
        
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
                RotationMaxtrix(Random.Range(0, 7));
                break;
            case TypeShape.crossBar_2:
                shape = new int[4, 4]
                {
                    { 1, 1, 0, 0} ,
                    { 0, 0, 0, 0} ,
                    { 0, 0, 0, 0} ,
                    { 0, 0, 0, 0} ,
                };
                RotationMaxtrix(Random.Range(0, 7));
                break;
            case TypeShape.crossBar_3:
                shape = new int[4, 4]
              {
                    { 1, 1, 1, 0} ,
                    { 0, 0, 0, 0} ,
                    { 0, 0, 0, 0} ,
                    { 0, 0, 0, 0} ,
              };
                RotationMaxtrix(Random.Range(0, 7));
                break;
            case TypeShape.crossBar_4:
                shape = new int[4, 4]
              {
                    { 1, 1, 1, 1} ,
                    { 0, 0, 0, 0} ,
                    { 0, 0, 0, 0} ,
                    { 0, 0, 0, 0} ,
              };
                RotationMaxtrix(Random.Range(0, 7));
                SplitMatrix(shape);
                Render(shape);
                
                break;
            case TypeShape.square:
                shape = new int[4, 4]
            {
                    { 1, 1, 0, 0} ,
                    { 1, 1, 0, 0} ,
                    { 0, 0, 0, 0} ,
                    { 0, 0, 0, 0} ,
            };
                RotationMaxtrix(Random.Range(0, 7));
                SplitMatrix(shape);
                break;
            case TypeShape.three_cube:
                shape = new int[4, 4]
            {
               
                    { 1, 1, 0, 0} ,
                    { 1, 0, 0, 0} ,
                    { 0, 0, 0, 0} ,
                    { 0, 0, 0, 0} ,
            };
                RotationMaxtrix(Random.Range(0,7));
                Render(shape);
                SplitMatrix(shape);
                break;
        }
        SetShape();
        
       
    }
    public int[,] SplitMatrix(int[,] matrix)
    {
        int x = -1;
        int y = -1;
        int xMatrixSlip = 0;
        int yMatrixSlip = 0;
        int xSlip = 0;
        int ySlip = 0;
        // Check Width
        int width=0;
        int height = 0;
        string s = "";
        for (int j=0;j<matrix.GetLength(1); j++)
        {
            s += "\n";
          
            if (width >= xSlip)
            {
                xSlip = width;
                Debug.Log("Chance :" + xSlip);
            }
            width = 0;
        //    Debug.Log("HANG " + j + " :" + xSlip);
          
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                s += matrix[i,j];
                if (matrix[i, j] == 1)
                {
                   xMatrixSlip =  Mathf.Min(xMatrixSlip, i);
                    width++;
                 //   Debug.Log("HANG " + "[" + j + "," + "]" + " :: " + width);
                 //   xSlip = Mathf.Min(xSlip, j);
                }
            }
        }
     
        //   Debug.Log(s);

        // Check Height
        for (int j = 0; j < matrix.GetLength(0); j++)
        {

            ySlip = Mathf.Max(ySlip, height);
            height = 0;
            for (int i = 0; i < matrix.GetLength(1); i++)
            {
                if (matrix[j, i] == 1)
                {
                    height++;
                  
                    //  ySlip = Mathf.Min(xSlip, j);
                }
            }
            Debug.Log("COT " + j + " :" + height);
        }
          
        int[,] MatrixSlip = new int[xMatrixSlip,yMatrixSlip];
        Debug.Log(xMatrixSlip + "   " + yMatrixSlip);
        Debug.Log(xSlip + "   " + ySlip);
        //Get Slip Matrix
        for (int j = ySlip; j < ySlip+yMatrixSlip; j++)
        {
            y++;
            x = 0;
            for (int i = xSlip; i < xSlip + xMatrixSlip; i++)
            {
                MatrixSlip[x, y] = matrix[i, j];
                    x++;
            }
        }
        Render(MatrixSlip);
        return MatrixSlip;
                        
            
    }
    public int[,] RotationMaxtrix(int countRoll)
    {
      
        int[,] cloneMatrix = Clone(shape);
     
        int count = 4;
        for(int x = 0; x <countRoll; x++)
        {
            count = 4;
            for (int i = 0; i < 4; i++)
            {
                count--;
                for (int j = 0; j < 4; j++)
                {

                    shape[i, j] = cloneMatrix[j,count];
                }
            }
            cloneMatrix = Clone(shape);
           

        }
        return cloneMatrix;
        
       





    }
    public int[,] Clone(int[,] matrix)
    {
        int[,] matrixs = new int[4, 4];
        for(int i = 0; i < 4; i++)
        {
          
            for(int j = 0; j < 4; j++)
            {
                int x = matrix[j, i];
                matrixs[j, i] = x;
            }
        }
        return matrixs;
        
    }
    public void Render(int[,] matrix)
    {
        string s = "";
        for(int i = 0; i < 4; i++)
        {
            s+="\n";
            for (int j = 0; j < 4; j++)
            {
               s+= matrix[j, i] + "  ";    
            }
        }
        Debug.Log(s);
    }
    private void SetShape()
    {
        int[,] matrixs = new int[4,4];
        int i = 0;
        for(int y = 0; y < 4; y++)
        {
            
            for(int x = 0; x < 4; x++)
            {
                if (shape[y,x] != 0)
                {
                    matrixs[y, x] = 1;
                   var a =  Instantiate(PrebShape, transform);
                    a.transform.localPosition = new Vector3(y * offsetX,- x * offsetY);
                    i++;
                    a.name = "Shape " + i;
                    ListShape.Add(a);
                }
                else
                {
                    matrixs[y, x] = 0;
                }
            }
        }
        Render(matrixs);
    }
    #endregion

    #region PushToBoard

    public Vector2[] PushToBoard()
    {
        List<Vector2> ListPoint = new List<Vector2>();
        for(int i = 0; i < ListShape.Count; i++)
        {
            int x = Mathf.Abs(Mathf.RoundToInt((ListShape[i].transform.position.x - CtrlGamePlay.Ins.initPoint.x)/CtrlGamePlay.Ins.offsetX));
            int y = Mathf.Abs(Mathf.RoundToInt((ListShape[i].transform.position.y - CtrlGamePlay.Ins.initPoint.y)/CtrlGamePlay.Ins.offsetY));
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
    public void Destroy()
    {
        Destroy(gameObject);
    }

    #endregion
}
