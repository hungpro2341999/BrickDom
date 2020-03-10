using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class DataGameShape
{
    public string key = "";
    [SerializeField]
    
    public List<string> StringMatrix = new List<string>();
    public DataGameShape(string key, List<int[,]> Matrix)
    {

        this.key = key;
        this.StringMatrix = ConvertoString(Matrix);
    }
    public List<string> ConvertoString(List<int[,]> matrix)
    {
        List<string> StringMatrix = new List<string>();

        for(int i = 0; i < matrix.Count; i++)
        {
            StringMatrix.Add(Shape.Render(matrix[i]));
        }
        return StringMatrix;
    } 

    public List<int[,]> GetListMatrix()
    {
        List<int[,]> Matrixs = new List<int[,]>();
        for(int i = 0; i < StringMatrix.Count; i++)
        {
            Matrixs.Add(CtrlData.ConverStringToMatrix(StringMatrix[i]));
        }
        return Matrixs;
    }

}