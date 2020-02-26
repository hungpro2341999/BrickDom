using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelf : MonoBehaviour
{
   public Vector2 Point;
   public Shape shape;
    public bool isDestroy = false;
    private void Start()
    {
        shape = GetComponentInParent<Shape>();
    }
    public void Destroy()
    {

        //Effect Destroy

        Instantiate(CtrlData.Ins.GetEffect(shape.IDColor), transform.position, Quaternion.identity, null);
        isDestroy = true;
        GetComponentInParent<Shape>().RemoveCube(gameObject);
        Destroy(gameObject);
    }
    public void StartDestroy(float time)
    {
        StartCoroutine(DestroyCube(time));
    }

    public IEnumerator DestroyCube(float time)
    {
        yield return new WaitForSeconds(time);
        Instantiate(CtrlData.Ins.GetEffect(5), transform.position, Quaternion.identity, null);
        isDestroy = true;
        GetComponentInParent<Shape>().RemoveCube(gameObject);
        Destroy(gameObject);
    }
    public Vector2 GetPoint()
    {
        return Point;
    }
   
}
