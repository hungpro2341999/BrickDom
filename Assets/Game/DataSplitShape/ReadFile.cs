using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using System;
public class ReadFile : MonoBehaviour
{
    static readonly string rootFolder = @"Assets/Game/DataSplitShape/SplitShape.txt";

    // Start is called before the first frame update
    void Start()
    {
        // WriteString();
        // Read_File();
        // string s = "1 0 0,1 1 1,0 0 0";
        //int[,] Matrix = StringToMatrix(s);

        // string ss = "";
        // for(int i = 0; i < Matrix.GetLength(0); i++)
        // {
        //     ss += "\n";
        //     for(int j = 0; j < Matrix.GetLength(1); j++)
        //     {
        //         ss += Matrix[i, j].ToString();
        //     }
        // }
        // Debug.Log(ss);


        int[,] matrix = new int[4, 2]

        {
           {0,1 },
           {0,1 },
           {1,1 },
           {1,1 }

        };

        List<int[,]> TotalMatrix = SplitMatrix(matrix, new int[2] { 0, 2 }, new int[2] { 0,1});

//        Debug.Log("RENDER :" + TotalMatrix.Count);

        for (int i = 0; i < TotalMatrix.Count; i++)
        {
          //  Debug.Log("RENDER : " + i + "  " + Shape.Render(TotalMatrix[i]));

        }

       
    }

    // Update is called once per frame
    void Update()
    {

    }



    public void WriteFile()
    {

    }





    public static string  Read_File()
    {
        string text = "";
        if (File.Exists(rootFolder))
        {
            // Read entire text file content in one string    
            text = File.ReadAllText(rootFolder);
            Debug.Log(text);

        }
        else
        {
            Debug.Log("Konh co");   
        }
        return text;

    }
    public static void ResetFile()
    {

        StreamWriter writer = new StreamWriter(rootFolder, false);
        writer.WriteLine("");
        writer.Close();



    }

    public static void WriteString(string s)
    {
     
        StreamWriter writer = new StreamWriter(rootFolder, true);
        writer.WriteLine(s);
        writer.Close();


     
    }
    public static int[,] StringToMatrix(string s)
    {
        int[,] matrix = null;
        string[] RowSplit = s.Split(',');
        int row = s.Split(',').Length;
        int column = s.Split(',')[0].Split(' ').Length;
        matrix = new int[row, column];
        for(int i = 0 ; i < row ; i++)
        {
            string[] SplitColumn = RowSplit[i].Split(' ');
            for(int j = 0 ; j < SplitColumn.Length; j++)
            {
                matrix[i, j] = int.Parse(SplitColumn[j]);

            }
        }



        return matrix;
    }

    public List<int[,]> SplitMatrix(int[,] matrix, int[] row1, int[] col1)
    {
        int[,] type = CtrlGamePlay.CloneBoard(matrix);
        List<int[,]> ListRowtMatrix = new List<int[,]>();
        List<int[,]> ListColumnMatrix = new List<int[,]>();
        List<int[,]> TotalMatrix = new List<int[,]>();
        List<int> ListPointRow = new List<int>();
        List<int> ListPointColumn = new List<int>();


        ListPointRow.Add(0);
        ListPointColumn.Add(0);
        List<List<int[,]>> ListColumMatrix_1 = new List<List<int[,]>>();

        if (row1 != null)
        {

            

            ListRowtMatrix = SplitRowMatrix(type, row1, ref ListPointRow);

            for (int i = 0; i < ListRowtMatrix.Count; i++)
            {
                Debug.Log("Split Row");
                Debug.Log(CtrlGamePlay.Render(ListRowtMatrix[i]));
            }

        }

        if (ListRowtMatrix.Count != 0 && col1 != null)
        {
           
            for (int i = 0; i < ListRowtMatrix.Count; i++)
            {
                Debug.Log("Split Col");
                ListColumMatrix_1.Add(SplitColumnMatrix(ListRowtMatrix[i], col1, ref ListPointColumn));


            }
            for (int j = 0; j < ListColumMatrix_1.Count; j++)
            {
                for (int k = 0; k < ListColumMatrix_1[j].Count; k++)
                {
                    Debug.Log(CtrlGamePlay.Render(ListColumMatrix_1[j][k]));
                }
            }


            if (ListColumMatrix_1.Count != 0)
            {
                for (int j = 0; j < ListColumMatrix_1.Count; j++)
                {
                    for (int k = 0; k < ListColumMatrix_1[j].Count; k++)
                    {
                        ListColumnMatrix.Add(ListColumMatrix_1[j][k]);
                    }
                }
            }
            for (int i = 0; i < ListColumnMatrix.Count; i++)
            {
                TotalMatrix.Add(ListColumnMatrix[i]);
            }



        }
        else if (col1 != null)
        {
            ListColumnMatrix = SplitColumnMatrix(type, col1, ref ListPointColumn);
            for (int i = 0; i < ListColumnMatrix.Count; i++)
            {
                TotalMatrix.Add(ListColumnMatrix[i]);
            }
            List<Vector2> Points = new List<Vector2>();





        }
        else if (row1 != null)
        {
            for (int i = 0; i < ListRowtMatrix.Count; i++)
            {
                TotalMatrix.Add(ListRowtMatrix[i]);
            }
        }
        else
        {

            TotalMatrix.Add(type);
        }

        List<Vector2> ListPoint =  GrounpPoint(ListPointRow, ListPointColumn);

        Debug.Log("SPLIT POINT ");

        for(int i = 0; i < ListPoint.Count; i++)
        {
            Debug.Log(ListPoint[i].x + "  " + ListPoint[i].y);
        }

        for (int i = 0; i < TotalMatrix.Count; i++)
        {

            Debug.Log("SHAPE : " + CtrlGamePlay.Render(TotalMatrix[i]));
        } 

            return TotalMatrix;
       
    }

    public List<Vector2> GrounpPoint(List<int> Row, List<int> Column)
    {
        List<Vector2> point = new List<Vector2>();
        for (int i = 0; i < Row.Count; i++)
        {
            Vector2 p = Vector2.zero;
            p.x = Row[i];
            for (int j = 0; j < Column.Count; j++)
            {
                p.y = Column[j];
                point.Add(p);
            }

        }

        for (int i = 0; i < point.Count; i++)
        {
            Debug.Log(point[i].x + " :::: " + point[i].y);

        }
        return point;


    }

    public List<int[,]> SplitColumnMatrix(int[,] matrixs, int[] column, ref List<int> point)
    {
        List<int> ListPointRow = new List<int>();
        List<int> ListPointColumn = new List<int>();
        ListPointRow.Add(0);
        ListPointColumn.Add(0);


        List<int> ListPoint = new List<int>();
        List<List<int>> ListColumn = CutColumn(matrixs);

        List<int[,]> ListMatrix = new List<int[,]>();

        List<int> Flag = SetUpFlag(matrixs.GetLength(1), column);

        Debug.Log(CtrlGamePlay.RenderList(Flag));

        List<int[,]> matrix = new List<int[,]>();

        List<List<int>> Column = new List<List<int>>();

        ListPoint.Add(0);
        for (int i = 0; i < Flag.Count; i++)
        {
            if (Flag[i] == -1)
            {
               
                

                if (Column.Count != 0)
                {

                    ListMatrix.Add(ListColumToMatrix(Column));
                    Column = new List<List<int>>();

                }
               
                {
                    Column.Add(ListColumn[i]);

                    ListMatrix.Add(ListColumToMatrix(Column));

                    Column = new List<List<int>>();
                }



                if (!ListPoint.Contains(i))
                {
                    ListPoint.Add(i);
                    if (i + 1 < Flag.Count)
                    {
                        if (!ListPoint.Contains(i + 1))
                        {
                            ListPoint.Add(i + 1);
                        }

                    }
                }
                else
                {
                    if (!ListPoint.Contains(i + 1))
                    {
                        if (i + 1 < Flag.Count)
                            ListPoint.Add(i + 1);
                    }

                }

                if (Column.Count != 0)
                {

                    ListMatrix.Add(ListColumToMatrix(Column));
                    Column = new List<List<int>>();

                }




            }
            else
            {
                Column.Add(ListColumn[i]);
            }
        }

        if (Column.Count != 0)
        {
            ListMatrix.Add(ListColumToMatrix(Column));
        }
        Debug.Log(ListMatrix.Count);
        for(int i = 0; i < ListMatrix.Count; i++){


           Debug.Log(CtrlGamePlay.Render(ListMatrix[i]));
        }
        point = ListPoint;
      
        return ListMatrix;




    }

    public List<List<int>> CutColumn(int[,] matrix)
    {
        List<List<int>> ListColumn = new List<List<int>>();
        for (int i = 0; i < matrix.GetLength(1); i++)
        {
            List<int> Column = new List<int>();
            for (int j = 0; j < matrix.GetLength(0); j++)
            {
                Column.Add(matrix[j, i]);
            }
            ListColumn.Add(Column);
        }
        Debug.Log("COLUM");
        for(int i = 0; i < ListColumn.Count; i++)
        {
           Debug.Log(CtrlGamePlay.RenderList(ListColumn[i]));
        }
        return ListColumn;

    }

    public int[,] ListToMatrix(List<List<int>> shape)
    {
        int[,] matrix = null;
        if (shape.Count != 0)
        {
            int column = shape[0].Count;
            //  Debug.Log("Row : " + shape.Count + " " + column);
            matrix = new int[shape.Count, column];
            for (int i = 0; i < shape.Count; i++)
            {
                for (int j = 0; j < column; j++)
                {
                    matrix[i, j] = shape[i][j];
                }
            }
        }
        else
        {
            Debug.LogError("LIST MATRIX NULL");
        }

        return matrix;
    }
    public static int[,] ListColumToMatrix(List<List<int>> list)
    {

        int row = list[0].Count;

        int[,] matrix = new int[row, list.Count];
        for (int i = 0; i < list.Count; i++)
        {
            for (int j = 0; j < row; j++)
            {
                matrix[j, i] = list[i][j];
            }
        }
        return matrix;
    }

    public int[,] ListColumnMatrix(List<List<int>> shape)
    {
        int[,] matrix = null;
        if (shape.Count != 0)
        {
            matrix = new int[shape[0].Count, shape.Count];
            int column = shape.Count;
            for (int i = 0; i < shape.Count; i++)
            {
                for (int j = 0; j < shape[0].Count; j++)
                {
                    matrix[i, j] = shape[i][j];
                }
            }




        }
        else
        {
            Debug.LogError("LIST MATRIX NULL");
        }

        return matrix;
    }

    public List<int> SetUpFlag(int Row, int[] row)
    {
        List<int> Flag = new List<int>();
        for (int i = 0; i < Row; i++)
        {
            bool a = false;
            string s = "";
            for (int j = 0; j < row.Length; j++)
            {

                s += i + "  " + row[j];
                if (i == row[j])
                {
                    s += "Find";
                    Flag.Add(-1);
                    a = true;
                    break;
                }
            }
            //    Debug.Log(s);
            if (!a)
                Flag.Add(1);
        }
        return Flag;
    }
    public List<int[,]> SplitRowMatrix(int[,] matrixs, int[] row, ref List<int> listpoint)
    {
        List<int> point = new List<int>();
        List<List<int>> ListRow = CutRow(matrixs);
        Debug.Log("CUT ROW");
        for (int i = 0; i < ListRow.Count; i++)
        {
         //   Debug.Log(CtrlGamePlay.RenderList(ListRow[i]));
        }
        List<int[,]> ListMatrix = new List<int[,]>();

        List<int> Flag = SetUpFlag(matrixs.GetLength(0), row);
        for (int i = 0; i < ListRow.Count; i++)
        {
         //   Debug.Log(CtrlGamePlay.RenderList(Flag));
        }
        Debug.Log(ListRow.Count + "  " + Flag.Count);
     

        List<int[,]> matrix = new List<int[,]>();

        List<List<int>> Row = new List<List<int>>();
        point.Add(0);
        for (int i = 0; i < Flag.Count; i++)
        {
            if (Flag[i] == -1)
            {
                if (Row.Count != 0)
                {
                    ListMatrix.Add(ListToMatrix(Row));
                    Row = new List<List<int>>();
                }

                Row.Add(ListRow[i]);
                ListMatrix.Add(ListToMatrix(Row));
                Row = new List<List<int>>();


                if (!point.Contains(i))
                {
                    point.Add(i);
                    if (i + 1 < Flag.Count)
                    {
                        if (!point.Contains(i + 1))
                        {
                            point.Add(i + 1);
                        }
                    }

                }
                else
                {
                    if (i + 1 < Flag.Count)
                    {
                        if (!point.Contains(i + 1))
                        {
                            point.Add(i + 1);
                        }
                    }
                }



            }
            else
            {
                Row.Add(ListRow[i]);
            }
        }
        if (Row.Count != 0)
        {
            ListMatrix.Add(ListToMatrix(Row));

        }
        Debug.Log("RENDER SPLIT POINT ROW : " + CtrlGamePlay.RenderList(point));
        listpoint = point;
        return ListMatrix;




    }

    public List<List<int>> CutRow(int[,] matrix)
    {
        List<List<int>> ListRow = new List<List<int>>();
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            List<int> Row = new List<int>();
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                Row.Add(matrix[i, j]);
            }
            ListRow.Add(Row);
        }
        return ListRow;

    }

}
