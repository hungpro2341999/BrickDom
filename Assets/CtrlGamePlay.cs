using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CtrlGamePlay : MonoBehaviour
{
    public static CtrlGamePlay Ins;
    public int countX;
    public int countY;
    public int[,] Board;
    public Vector2 initPoint;
    public float offsetX;
    public float offsetY;
    public GameObject PrebShape;
    public List<GameObject> Cubes = new List<GameObject>();
    public List<Shape> List_Shape = new List<Shape>();
    public Vector2 Destroy;
    
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
        if (Input.GetKeyDown(KeyCode.S))
        {
            SpawnShape();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            DestroyCube(Vector2.zero);
        }
    }
    public void SpawnShape()
    {
        int x = Random.Range(0,countX);
        var  a =  Instantiate(PrebShape, new Vector3(initPoint.x + x * offsetX, initPoint.y),Quaternion.identity,null);
        List_Shape.Add(a.GetComponent<Shape>());
        
    }
    public void AddCubeIntoBoard(GameObject cube)
    {
        Cubes.Add(cube);
        Vector2 point = cube.GetComponent<DestroySelf>().Point;
        for(int i = 0; i < countY; i++)
         {
            for (int j = 0; j < countX; j++)
            {
                if (j == point.x && i == point.y)
                {
                    Board[j,i] = 1;
                }
             
            }
         }
        Render(Board);

    }


    public void DestroyCube(Vector2 point)
    {
       
        for(int i = 0; i < Cubes.Count; i++)
        {
            for(int x = 0; x < countX; i++)
            {
                if (Cubes[i].GetComponent<DestroySelf>().Point.y == Destroy.y)
                {
                    Cubes[i].GetComponent<DestroySelf>().Destroy();
                }
            }
           
          
        }
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
    
    private void Render(int[,] matrix)
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
        Debug.Log(s);
    }
    
   

}
