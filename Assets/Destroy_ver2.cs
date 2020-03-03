using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy_ver2 : PoolItem
{
    // Start is called before the first frame update
    public bool destroy = false;
    public float time;
    // Start is called before the first frame update
    void Start()
    {
        if (destroy)
        {
            StartCoroutine(StartDestroy(time));
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void DestroyObj()
    {
        gameObject.SetActive(false);
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
    public override void OnSpawn()
    {
        transform.parent = CtrlData.Ins.UI;
        base.OnSpawn();
        StartCoroutine(StartDestroy(time));
    }
}
