using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMethod : MonoBehaviour
{
    public int roll;
    public TypeShape type;
    public Vector2 point;
    public int row;
    public int column;
    public bool isCorrect = false; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        point = CtrlGamePlay.PositonToPointMatrix(transform.position.x, transform.position.y);
      
        if (Input.GetKeyDown(KeyCode.B))
        {
            string s =  Shape.Render(Shape.RotationMaxtrix(CtrlData.Cube_3, roll));
            Debug.Log(s);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            int[,] type10 = new int[4, 4]
            {
            {1,1,1,0},
            {0,0,0,0},
           {0,0,0,0},
           {0,0,0,0},
            };
         
            //  string ss = Shape.Render(Shape.RotationMaxtrix(type10, i));
           
            string s = Shape.Render(type10);
            Debug.Log("Matrix : " + s);
        
                string ss = CtrlGamePlay.Render(CtrlGamePlay.SimulateRoll(1,TypeShape.crossBar_3,false));
                Debug.Log("SS  " + ss);
          

            //  type =  CtrlGamePlay.Ins.MatrixToType(type10);
            //   isCorrect = CtrlGamePlay.Ins.isTypeOf(TypeShape.crossBar_3,Shape.Clone(type10));







            //  roll = CtrlGamePlay.RollShape(type, type1);

        }
        Vector2 pos = CtrlGamePlay.PositonToPointMatrix(transform.position.x, transform.position.y);
        row = (int)pos.x;
        column = (int)pos.y;
       // isCorrect = CtrlGamePlay.IsPushShapeCorrect(CtrlGamePlay.Ins.Board,row, column);

    }


}
