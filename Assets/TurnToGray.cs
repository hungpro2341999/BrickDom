using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnToGray : MonoBehaviour
{

    public _2dxFX_GrayScale gray;
    public float Speed;
    public static int CountPlay = 1;
    // Start is called before the first frame update
    void Start()
    {
        gray = GetComponent<_2dxFX_GrayScale>();
        Speed = 2.25f;
     //   StartCoroutine(StartTurnToGray(0.2f));

    }
   
    public void Start_Turn_To_Gray(float time)
    {
        StartCoroutine(StartTurnToGray(time));
    }
    public  IEnumerator StartTurnToGray(float time)
    {
        yield return new WaitForSeconds(time);
        while(gray._EffectAmount <= 1)
        {
                gray._EffectAmount += Speed*Time.deltaTime;
            yield return new WaitForSeconds(0);
        }
    }
    public void Reset()
    {
        if (gray == null)
        {
            gray = GetComponent<_2dxFX_GrayScale>();
        }
        gray._EffectAmount = 0;
    }

}

