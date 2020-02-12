using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum TypeShape {crossBar_1,crossBar_2,crossBar_3, crossBar_4, square,three_cube}
public class Shape : MonoBehaviour
{
    public TypeShape TypeShape;
    public int[,] shape;
    public int MaxLength;
    public GameObject PrebShape;
    public GameObject ListShape;
    public float offsetX=0.6f;
    public float offsetY=0.6f;
    // Start is called before the first frame update
    void Start()
    {
        init();
        initShape(TypeShape);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
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
                RotationMaxtrix(Random.Range(0, 10));
                break;
            case TypeShape.crossBar_2:
                shape = new int[4, 4]
                {
                    { 1, 1, 0, 0} ,
                    { 0, 0, 0, 0} ,
                    { 0, 0, 0, 0} ,
                    { 0, 0, 0, 0} ,
                };
                RotationMaxtrix(Random.Range(0, 10));
                break;
            case TypeShape.crossBar_3:
                shape = new int[4, 4]
              {
                    { 1, 1, 1, 0} ,
                    { 0, 0, 0, 0} ,
                    { 0, 0, 0, 0} ,
                    { 0, 0, 0, 0} ,
              };
                RotationMaxtrix(Random.Range(0, 10));
                break;
            case TypeShape.crossBar_4:
                shape = new int[4, 4]
              {
                    { 1, 1, 1, 1} ,
                    { 0, 0, 0, 0} ,
                    { 0, 0, 0, 0} ,
                    { 0, 0, 0, 0} ,
              };
                RotationMaxtrix(Random.Range(0, 10));
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
                break;
            case TypeShape.three_cube:
                shape = new int[4, 4]
            {
               
                    { 1, 1, 0, 0} ,
                    { 1, 0, 0, 0} ,
                    { 0, 0, 0, 0} ,
                    { 0, 0, 0, 0} ,
            };
                RotationMaxtrix(Random.Range(0,4));
                Render(shape);
                break;
        }
        SetShape();
        
       
    }
    public void RotationMaxtrix(int countRoll)
    {
      
        int[,] cloneMatrix = Clone(shape);

        for(int x = 0; x < countRoll; x++)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {

                    shape[j, i] = cloneMatrix[i, j];
                }
            }
         
        }
     
     
        
       

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
        for(int x = 0; x < 4; x++)
        {
            for(int y = 0; y < 4; y++)
            {
                if (shape[x,y] != 0)
                {
                   var a =  Instantiate(PrebShape, transform);
                    a.transform.localPosition = new Vector3(x * offsetX, y * offsetY);

                }
            }
        }
    }
}
