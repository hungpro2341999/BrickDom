using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelf1 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void DestroyObj()
    {
        Destroy(gameObject);
    }
    private IEnumerator StartDestroy(float time)
    {
        yield return new WaitForSeconds(time);
        DestroyObj();
    }
    public void StartDelayDestroy(float time)
    {
        StartDestroy(time);
    }

}
