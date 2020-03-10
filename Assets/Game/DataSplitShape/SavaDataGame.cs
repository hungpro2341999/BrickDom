using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class SavaDataGame
{
    public List<DataGameShape> shape = new List<DataGameShape>();
    public List<DataGamePositon> Postion = new List<DataGamePositon>();
    public SavaDataGame(List<DataGameShape> shape, List<DataGamePositon> Postion)
    {
        this.shape = shape;
        this.Postion = Postion;
    }

}
