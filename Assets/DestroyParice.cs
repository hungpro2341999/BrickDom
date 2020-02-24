using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParice : DestroySelf1
{
    public float timeLife = 1.5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeLife -= Time.deltaTime;
        if (timeLife < 0)
        {
            Destroy(gameObject);
        }

    }
}
