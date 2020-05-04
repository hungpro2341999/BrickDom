using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowGamePlay : Windown
{
    public Animator AnimGamePlay;
    public bool open = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        //open = !open;
        //AnimGamePlay.SetBool("Open",open);
    }
    private void OnDisable()
    {
        //open = !open;
        //AnimGamePlay.SetBool("Open", open);
    }

}
