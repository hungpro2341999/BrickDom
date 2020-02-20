using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMethod : MonoBehaviour
{
    public int roll;
    public TypeShape type;
    public Vector2 point;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        point = CtrlGamePlay.PositonToPointMatrix(transform.position.x, transform.position.y);
        if (Input.GetKeyDown(KeyCode.A))
        {
            CtrlGamePlay.Ins.GenerateStartGame(1, true);
            
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            int[,] matrix = new int[2, 4]
           {

                   {1,1,1,1},
                   {1,0,0,0},
                   //{1,0},
                   //{1,0},




           };
           type =   CtrlGamePlay.Ins.MatrixToType(matrix);
        }
            
    }


}
