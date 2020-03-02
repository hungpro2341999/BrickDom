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

    public void StartDestroy_Ver_2(float time,bool eff)
    {
        StartCoroutine(DelayDestroy(time, eff));
    }
    public IEnumerator DelayDestroy(float time,bool eff)
    {
       // Destroy(eff);
        yield return new WaitForSeconds(time);
        //Destroy(false);

    }
    public void Destroy()
    {

        //Effect Destroy

        if (!shape.isEff)
        {
            Instantiate(CtrlData.Ins.GetEffect(shape.IDColor), transform.position, Quaternion.identity, null);
        }
           
        
        //Instantiate(CtrlData.Ins.GetEffect(shape.IDColor), transform.position, Quaternion.identity, null);
        isDestroy = true;
        shape.RemoveCube(this.gameObject);
        DestroyImmediate(this.gameObject);
    }
    public void StartSuperEff()
    {
        Instantiate(CtrlData.Ins.GetEffect(8), transform.position, Quaternion.identity, null);
        shape.StartFade();
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
