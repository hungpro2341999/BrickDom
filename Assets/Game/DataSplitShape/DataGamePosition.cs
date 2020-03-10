using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DataGamePositon
{
    public string key = "";

    public List<Vector2> Matrix = new List<Vector2>();
    public DataGamePositon(string key, List<Vector2> Pos)
    {

        this.key = key;
        this.Matrix = Pos;
    }

}