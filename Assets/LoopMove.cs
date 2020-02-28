using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopMove : MonoBehaviour
{
    public Vector2 PStart;
    public Vector2 End;
    public float Speed;
    bool chance = false;
    
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (!chance)
        {
            transform.position = Vector2.MoveTowards(transform.position, PStart,Speed*Time.deltaTime);
            if((Vector2)PStart == (Vector2)transform.position)
            {
                chance = !chance;
            }

        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, End, Speed * Time.deltaTime);
            if ((Vector2)End == (Vector2)transform.position)
            {
                chance = !chance;
            }

        }
    }
    public void SetUp(Vector2 Start, Vector2 End,float speed)
    {
        this.PStart = Start;
        this.End = End;
        Speed = speed;
    }
    
}
