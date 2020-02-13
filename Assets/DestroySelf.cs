using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelf : MonoBehaviour
{
   public Vector2 Point;
    public Shape shape;
    private void Start()
    {
        shape = GetComponentInParent<Shape>();
    }
    public void Destroy()
    {
        GetComponentInParent<Shape>().RemoveCube(gameObject);
        Destroy(gameObject);
    }
}
