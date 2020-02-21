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
        if (Input.GetKeyDown(KeyCode.C))
        {
        
        }
        Vector2 pos = CtrlGamePlay.PositonToPointMatrix(transform.position.x, transform.position.y);
        row = (int)pos.x;
        column = (int)pos.y;
       // isCorrect = CtrlGamePlay.IsPushShapeCorrect(CtrlGamePlay.Ins.Board,row, column);

    }


}
