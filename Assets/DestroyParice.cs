using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParice  : PoolItem
{
    public float timeLife = 1.5f;
    public ParticleSystem[] partice;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isDoneEff())
        {
            OnDespawn();
            Debug.Log("xong");
        }
        else
        {
            Debug.Log("Chua xong eff");
        }

    }
    public override void OnSpawn()
    {
        partice = GetComponentsInChildren<ParticleSystem>();
        StartEff();

    }
    public override void OnDespawn()
    {

        gameObject.SetActive(false);
    }
    public void StartEff()
    {

        for(int i = 0; i < partice.Length; i++)
        {
            partice[i].Play();
        }
    }
    public bool isDoneEff()
    {
        for (int i = 0; i < partice.Length; i++)
        {
            if (partice[i].isPlaying)
            {
                return false;
            }
        }
        return true;
    }
}
