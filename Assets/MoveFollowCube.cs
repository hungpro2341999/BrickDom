using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveFollowCube : MonoBehaviour
{
    public Shape shape;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (shape != null)
        {
            Vector3 pos = transform.position;
            pos.x = shape.transform.position.x;
            
         
          Vector2  point =   CtrlGamePlay.PositonToMatrixRound(pos.x, pos.y);
           // transform.position = CtrlGamePlay.MatrixToPoint(point.x, point.y);


        }
    }
    public void ScaleX(int x)
    {
        Vector3 scale = transform.localScale;
        scale.x = x;
        transform.localScale = scale;
    }
}
