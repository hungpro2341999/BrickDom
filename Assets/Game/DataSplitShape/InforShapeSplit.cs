using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class InforShapeSplit : MonoBehaviour
{
    public static List<string> ListKey = new List<string>();
    public static Dictionary<string,List<int[,]>> InforShape = new Dictionary<string, List<int[,]>>();
    public static Dictionary<string, List<Vector2>> List_Point = new Dictionary<string, List<Vector2>>();
    public static Dictionary<TypeShape,InforForSplitShape> Directory = new Dictionary<TypeShape,InforForSplitShape>();
    public const string key_Data = "Key_Data_game_1";
    public const string key_Dictinory = "Key";


    public static List<string> Key = new List<string>();

    public static DataGameShape[] ArrayDataGameShape = null;

    public static DataGamePositon[] ArrayPositonShape = null;

    public static Dictionary<string, int> DirectKey = new Dictionary<string, int>();
    public List<Dictionary<string, List<int[,]>>> Type = new List<Dictionary<string, List<int[,]>>>();
    public string key = "ROLL : 3 T - [012][01]";
    // Start is called before the first frame update

    public static void  Filter()
    {
        ListKey keyShape = JsonUtility.FromJson<ListKey>(ReadFile.Ins.JsonDataKey.text); 
        //ListKey keyShape  = SavingDataWithJson.LoadData<ListKey>("Key.fun");
        List<string> keyGame = new List<string>();

        keyGame = keyShape.Key;

        for(int i = 0; i < keyGame.Count; i++)
        {
            DirectKey.Add(keyGame[i],i);
        }

        keyGame = keyShape.Key;



        Debug.Log("Has KEY : " + keyShape.Key.Count);
        //for (int i = 0; i < keyShape.Key.Count; i++)
        //{
        //    //  Debug.Log(" KEY : " + keyShape.Key[i]+ CtrlData.AllShape.Count);
        //}
        //for (int i = 0; i < CtrlData.AllShape.Count; i++)
        //{
        //    InforForSplitShape SplitShape = new InforForSplitShape();
        //    string key = CtrlData.AllShape[i].ToString();
        //    for (int j = 0; j < keyShape.Key.Count; j++)
        //    {

        //        //  Debug.Log("Key : "+ keyShape.Key[j] +"  "+ key);
        //        //if (keyShape.Key[j].Contains(key))
        //        //{
        //        //    Debug.Log("Co Key : " + keyShape.Key[j] + "  " + key);

        //        //}
        //        //else
        //        //{
        //        //    Debug.Log("NO Key:" + keyShape.Key[j] + "  " + key);
        //        //}
        //        if (keyShape.Key[j].Contains(key))
        //        {

        //            //  Debug.Log("Push Key : " + keyShape.Key[j]);
        //            SplitShape.InforShape.Add(keyShape.Key[j], InforShapeSplit.InforShape[keyShape.Key[j]]);
        //            SplitShape.List_Point.Add(keyShape.Key[j], InforShapeSplit.List_Point[keyShape.Key[j]]);

        //        }
        //        else
        //        {
        //            // Debug.Log("Khong Push Key : " + keyShape.Key[j] + "  " + key);
        //        }

        //    }
        //    Directory.Add(CtrlData.AllShape[i], SplitShape);

        //}




    }

    void Start()
    {





        //ReadFile.ResetFile();


        //for (int i = 0; i < CtrlData.AllShape.Count; i++)
        //{
        //    Debug.Log("ss");
        //    ProcessAllType(CtrlData.AllShape[i]);
        //}

        //Process(new int[4, 2]
        //{
        //    {1,0 },
        //    {1,1 },
        //      {1,1 },
        //        {1,1 }

        //});

        //

    }

   

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
           
            //ToHop tohop = new ToHop();
            //List<List<int>> th = tohop.Start_Find(2, 1);
            //for (int i = 0; i < th.Count; i++)
            //{
            //    Debug.Log(CtrlGamePlay.RenderList(th[i]));
            //}
            //Debug.Log("ss");



            //int[,] Matrix = new int[1,2]
            //{
            //    {1,1}

            //};
            //Process_1(Matrix, "KEY");
        }
        
    }

   

    public List<int[,]> GetInforShape(string rowcolum)
    {
        return InforShape[rowcolum];
    }

    public void ProcessAllType(TypeShape type)
    {
        string space = "\n";
        int[,] matrix = CtrlData.GetMatrixShapeType(type);
        int roll = CtrlData.CountRoll(type);

        string Type_Shape = "TypeShape : " + type.ToString();
        
        ReadFile.WriteString(Type_Shape);
        ReadFile.WriteString_Positon(Type_Shape);
        Type_Shape += space;
        if(TypeShape.square == type)
        {
            roll = 1;
        }
        for (int i = 0; i < roll; i++)
        {
            string Roll = "";
            
            ReadFile.WriteString(Roll);

            string key = "ROLL : " + i + " " + type.ToString();
            Debug.Log(key);
            if (TypeShape.square != type)
            {
                Process_1(CtrlData.ConverStringToMatrix(CtrlData.Ins.GetSimulateRoll(type, i)), key);
            }
            else
            {
                int[,] square = new int[2, 2] 
                {
                    {1,1 },
                    {1,1 }
                };
                Process_1(square, key);
            }
            

        }
       
       

    }
   
    public void Process_1(int[,] matrix, string keyRoll)
    {

        




        //for (int i = 0; i < matrix.GetLength(0); i++)
        //{
        //    //1 ///
        //    List<List<int>> ListTopHopRow = new List<List<int>>();
        //      ToHop toHopRow = new ToHop();
           
        //        Debug.Log("TH ROW: " + i + "  " + (matrix.GetLength(0) - 1));
        //        ListTopHopRow = toHopRow.Start_Find(matrix.GetLength(0),i+1);
           
        //    //////////////////////////


        //    for (int j = 0; j < matrix.GetLength(1); j++)
        //    {

        //        //2 ///




        //        ToHop toHopColumn = new ToHop();
        //        List<List<int>> ListTopHopColumn = new List<List<int>>();

        //            Debug.Log("TH COLUMN: "+j+"  "+ (matrix.GetLength(1) - 1));
        //            ListTopHopColumn = toHopColumn.Start_Find(matrix.GetLength(1),j+1);

        //            for(int t = 0; t < ListTopHopColumn.Count; t++)
        //            {
        //                Debug.Log("COLUMN : " + CtrlGamePlay.RenderList(ListTopHopColumn[t]));
        //            }
                
               
        //        for(int r = 0; r < ListTopHopRow.Count; r++)
        //        {
        //            int[] row = ListTopHopRow[r].ToArray();
        //            for (int c = 0; c < ListTopHopColumn.Count; c++)
        //            {
        //                string TypeShape = "CUT" + " - ";
        //                string key = keyRoll + " : " + "[";
                      
                     
        //                int[] column = ListTopHopColumn[c].ToArray();

        //                for (int r1 = 0; r1 < row.Length; r1++)
        //                {
        //                    key += row[r1];
        //                }
        //                key += "]";
        //                key += "[";
        //                for (int c1 = 0; c1 < column.Length; c1++)
        //                {
        //                    key += column[c1];
        //                }
        //                key += "]";
        //                TypeShape += key + " - ";

        //                if (List_Point.ContainsKey(key))
        //                {
        //                    continue;
        //                }
        //                string TypeShapePoint = TypeShape;
        //                List<Vector2> ListPoint = new List<Vector2>();
        //                List<int[,]> TotalMatrixSplit = SplitMatrix(matrix, row, column, out ListPoint);
        //                Debug.Log("RENDER111 : " + TotalMatrixSplit.Count);
        //                for (int z = 0; z < TotalMatrixSplit.Count; z++)
        //                {
        //                    string ss = InforShapeSplit.MatrixToString(TotalMatrixSplit[z]) + "||";
        //                    TypeShape += ss;
        //                    Debug.Log(ss);
        //                }

        //                string sss = ListToString(ListPoint);
        //                TypeShape += " - ";
        //                TypeShape += sss;



        //                Debug.Log(key);
        //                InforShape.Add(key, TotalMatrixSplit);
        //                List_Point.Add(key, ListPoint);
        //                ReadFile.WriteString(TypeShape);
                      
        //            }


        //        }




              
        //    }
        //}


        //for (int i = 0; i < matrix.GetLength(0); i++)
        //{
        //    string col = "[]";
        //    ToHop ToHopRow_1 = new ToHop();
          
           
        //    List<List<int>> ListToHoprow = ToHopRow_1.Start_Find(matrix.GetLength(0), i + 1);


        //    for (int r = 0; r < ListToHoprow.Count; r++)
        //    {
        //        string TypeShape = "CUT" + " - ";
        //        string key = keyRoll + " : " + "[";
        //        int[] Row = ListToHoprow[r].ToArray();
        //        string TypeShapePoint = "";


        //        for (int r1 = 0; r1 < Row.Length; r1++)
        //        {
        //            key += Row[r1];
        //        }
        //        key += "]";

               

              
        //        key += col;
        //        TypeShape += key + " - ";
        //        if (List_Point.ContainsKey(key))
        //        {
        //            continue;
        //        }
        //        List<Vector2> ListPoint = new List<Vector2>();
        //        List<int[,]> TotalMatrixSplit = SplitMatrix(matrix,Row, null, out ListPoint);
        //        Debug.Log("RENDER111 : " + TotalMatrixSplit.Count);

             
        //        for (int z = 0; z < TotalMatrixSplit.Count; z++)
        //        {
        //            string ss = InforShapeSplit.MatrixToString(TotalMatrixSplit[z]) + "||";
        //            TypeShape += ss;
        //            Debug.Log(ss);
        //        }
        //        string sss = ListToString(ListPoint);
        //        TypeShape += " - ";
        //        TypeShape += sss;
                  
                   
                
              
              
        //        Debug.Log(key);
        //        InforShape.Add(key, TotalMatrixSplit);
        //        List_Point.Add(key, ListPoint);
        //        ReadFile.WriteString(TypeShape);
               
        //    }
           


          

        //}
        ////////////
        for (int j = 0; j < matrix.GetLength(1); j++)
        {
           
            ToHop ToHopColumn = new ToHop();
            List<List<int>> Column = ToHopColumn.Start_Find(matrix.GetLength(1), j + 1);

            for(int c = 0; c < Column.Count; c++)
            {
              
                string TypeShape = "CUT" + " - ";
                string key = keyRoll + " : ";
                string Row = "[]";
                key += Row;
                key += "[";

                int[] column = Column[c].ToArray();

                for(int c1 = 0; c1 < column.Length; c1++)
                {
                    key += column[c1];
                }
                key += "]";
                TypeShape += key + " - ";
                if (List_Point.ContainsKey(key))
                {
                    continue;
                }
                List<Vector2> ListPoint = new List<Vector2>();
                List<int[,]> TotalMatrixSplit = SplitMatrix(matrix,null,column, out ListPoint);
                Debug.Log("RENDER111 : " + TotalMatrixSplit.Count);
               
                for (int z = 0; z < TotalMatrixSplit.Count; z++)
                {
                    string ss = InforShapeSplit.MatrixToString(TotalMatrixSplit[z]) + "||";
                    TypeShape += ss;
                    Debug.Log(ss);
                }

                string sss = ListToString(ListPoint);
                TypeShape += " - ";
                TypeShape += sss;

                Debug.Log(key);
                InforShape.Add(key, TotalMatrixSplit);
                List_Point.Add(key, ListPoint);
                ReadFile.WriteString(TypeShape);
               
            }
           

           
           
        }



    }









    public string ListToString(List<Vector2> list)
    {
        string s = "";
        for(int i = 0; i < list.Count; i++)
        {
            s += list[i].x +" "+list[i].y+"||";
        }
        return s;
    }


    public void Process(int[,] matrix,string keyRoll)
    {
        //First Commit -m:


        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            
                string TypeShape = "CUT" + " - ";
                string key = keyRoll + " - " + "[";
                int[] row = getListIntBefor(i);
                int[] column = new int[0];

                for (int r = 0; r < row.Length; r++)
                {
                      key += row[r];
                }
                key += "]";
                key += "[";
                for (int c = 0; c < column.Length; c++)
                {
                   // key += column[c].ToString();
                }
                key += "]";

                TypeShape += key + " - ";
            if (List_Point.ContainsKey(key))
            {
                continue;
            }
            List<Vector2> ListPoint = new List<Vector2>();
                List<int[,]> TotalMatrixSplit = SplitMatrix(matrix, row,null, out ListPoint);
                Debug.Log("RENDER111 : " + TotalMatrixSplit.Count);
                for (int z = 0; z < TotalMatrixSplit.Count; z++)
                {
                    string ss = InforShapeSplit.MatrixToString(TotalMatrixSplit[z]) + "||";
                    TypeShape += ss;
                    Debug.Log(ss);
                }

                Debug.Log(key);
                InforShape.Add(key, TotalMatrixSplit);
                List_Point.Add(key, ListPoint);
                ReadFile.WriteString(TypeShape);
           
        }


        for (int i = 0; i < matrix.GetLength(1); i++)
        {

            string TypeShape = "CUT" + " - ";
            string key = keyRoll + " - " + "[";
            int[] row =  new int[0];
            int[] column = getListIntBefor(i);

            for (int r = 0; r < row.Length; r++)
            {
            //    key += r.ToString();
            }
            key += "]";
            key += "[";
            for (int c = 0; c < column.Length; c++)
            {
                 key += column[c].ToString();
            }
            key += "]";
            TypeShape += key + " - ";
            if (List_Point.ContainsKey(key))
            {
                continue;
            }
            List<Vector2> ListPoint = new List<Vector2>();
            List<int[,]> TotalMatrixSplit = SplitMatrix(matrix,null, column, out ListPoint);
            Debug.Log("RENDER111 : " + TotalMatrixSplit.Count);
            for (int z = 0; z < TotalMatrixSplit.Count; z++)
            {
                string ss = InforShapeSplit.MatrixToString(TotalMatrixSplit[z]) + "||";
                TypeShape += ss;
                Debug.Log(ss);
            }

            Debug.Log(key);
            InforShape.Add(key, TotalMatrixSplit);
            List_Point.Add(key, ListPoint);
            ReadFile.WriteString(TypeShape);

        }


        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                string TypeShape = "CUT" + " - ";
                string key = keyRoll + " - " + "[";
                int[] row = getListIntBefor(i);
                int[] column = new int[1] { j };

                for (int r = 0; r < row.Length; r++)
                {
                    key += r.ToString();
                }
                key += "]";
                key += "[";
                for (int c = 0; c < column.Length; c++)
                {
                    key += column[c].ToString();
                }
                key += "]";
                TypeShape += key + " - ";
                if (List_Point.ContainsKey(key))
                {
                    continue;
                }
                List<Vector2> ListPoint = new List<Vector2>();
                List<int[,]> TotalMatrixSplit = SplitMatrix(matrix, row, column, out ListPoint);
                Debug.Log("RENDER111 : " + TotalMatrixSplit.Count);
                for (int z = 0; z < TotalMatrixSplit.Count; z++)
                {
                    string ss = InforShapeSplit.MatrixToString(TotalMatrixSplit[z]) + "||";
                    TypeShape += ss;
                    Debug.Log(ss);
                }

                Debug.Log(key);
                InforShape.Add(key, TotalMatrixSplit);
                List_Point.Add(key, ListPoint);
                ReadFile.WriteString(TypeShape);
            }
        }


        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                string TypeShape = "CUT" + " - ";
                string key = keyRoll + " - " + "[";
                int[] row = new int[1] { i };
                int[] column = getListIntBefor(j);

                for (int r = 0; r < row.Length; r++)
                {
                    key += row[r];
                }
                key += "]";
                key += "[";
                for (int c = 0; c < column.Length; c++)
                {
                    key += c.ToString();
                }
                key += "]";
                TypeShape += key + " - ";

                if (List_Point.ContainsKey(key))
                {
                    continue;
                }
                List<Vector2> ListPoint = new List<Vector2>();
                List<int[,]> TotalMatrixSplit = SplitMatrix(matrix, row, column, out ListPoint);
                Debug.Log("RENDER111 : " + TotalMatrixSplit.Count);
                for (int z = 0; z < TotalMatrixSplit.Count; z++)
                {
                    string ss = InforShapeSplit.MatrixToString(TotalMatrixSplit[z]) + "||";
                    TypeShape += ss;
                    Debug.Log(ss);
                }
                
                Debug.Log(key);
                InforShape.Add(key, TotalMatrixSplit);
                List_Point.Add(key, ListPoint);
                ReadFile.WriteString(TypeShape);
            }
        }


        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for(int j = 0; j < matrix.GetLength(1); j++)
            {
                string TypeShape = "CUT"+" - ";
                string key = keyRoll+" - "+"[";
                int[] row = getListIntBefor(i);
                int[] column = getListIntBefor(j);
                
                for(int r = 0; r < row.Length; r++)
                {
                    key += r.ToString();
                }
                key += "]";
                key += "[";
                for (int c = 0; c < column.Length; c++)
                {
                    key += c.ToString();
                }
                key += "]";
                TypeShape +=key+" - ";
                if (List_Point.ContainsKey(key))
                {
                    continue;
                }
                List<Vector2> ListPoint = new List<Vector2>(); 
               List<int[,]> TotalMatrixSplit =  SplitMatrix(matrix, row, column,out ListPoint);
                Debug.Log("RENDER111 : " + TotalMatrixSplit.Count);
                for (int z = 0; z < TotalMatrixSplit.Count; z++)
                {
                   string ss =  InforShapeSplit.MatrixToString(TotalMatrixSplit[z])+"||";
                    TypeShape += ss;
                    Debug.Log(ss);
                }

                Debug.Log(key);
                InforShape.Add(key, TotalMatrixSplit);
                List_Point.Add(key, ListPoint);
                ReadFile.WriteString(TypeShape);
            }
        }
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                string TypeShape = "CUT" + " - ";
                string key = keyRoll + " - " + "[";
                int[] row = getListIntForward(i, matrix.GetLength(0));
                int[] column = getListIntForward(j, matrix.GetLength(1));

                for (int r = 0; r < row.Length; r++)
                {
                    key += r.ToString();
                }
                key += "]";
                key += "[";
                for (int c = 0; c < column.Length; c++)
                {
                    key += c.ToString();
                }
                key += "]";
                TypeShape += key + " - ";
                if (List_Point.ContainsKey(key))
                {
                    continue;
                }
                List<Vector2> ListPoint = new List<Vector2>();
                List<int[,]> TotalMatrixSplit = SplitMatrix(matrix, row, column, out ListPoint);
                Debug.Log("RENDER111 : " + TotalMatrixSplit.Count);
                for (int z = 0; z < TotalMatrixSplit.Count; z++)
                {
                    string ss = InforShapeSplit.MatrixToString(TotalMatrixSplit[z]) + "||";
                    TypeShape += ss;
                    Debug.Log(ss);
                }

                Debug.Log(key);
                InforShape.Add(key, TotalMatrixSplit);
                List_Point.Add(key, ListPoint);
                ReadFile.WriteString(TypeShape);
            }
        }

        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            string col = "[]";
            string TypeShape = "CUT"+" - ";
                string key = keyRoll+" - "+"[";
                int[] row = getListIntBefor(i);
                
                
                for(int r = 0; r < row.Length; r++)
                {
                    key += r.ToString();
                }
                key += "]";
                key += col;
             
               
                TypeShape +=key+" - ";
            if (List_Point.ContainsKey(key))
            {
                continue;
            }
            List<Vector2> ListPoint = new List<Vector2>(); 
               List<int[,]> TotalMatrixSplit =  SplitMatrix(matrix, row,null,out ListPoint);
                Debug.Log("RENDER111 : " + TotalMatrixSplit.Count);
                for (int z = 0; z < TotalMatrixSplit.Count; z++)
                {
                   string ss =  InforShapeSplit.MatrixToString(TotalMatrixSplit[z])+"||";
                    TypeShape += ss;
                    Debug.Log(ss);
                }

                Debug.Log(key);
                InforShape.Add(key, TotalMatrixSplit);
                List_Point.Add(key, ListPoint);
                ReadFile.WriteString(TypeShape);

        }
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            string col = "[]";
            string TypeShape = "CUT" + " - ";
            string key = keyRoll + " - " + "[";
            int[] row = getListIntForward(i, matrix.GetLength(0));


            for (int r = 0; r < row.Length; r++)
            {
                key += row[r];
            }
            key += "]";
            key += col;


            TypeShape += key + " - ";
            if (List_Point.ContainsKey(key))
            {
                continue;
            }
            List<Vector2> ListPoint = new List<Vector2>();
            List<int[,]> TotalMatrixSplit = SplitMatrix(matrix, row, null, out ListPoint);
            Debug.Log("RENDER111 : " + TotalMatrixSplit.Count);
            for (int z = 0; z < TotalMatrixSplit.Count; z++)
            {
                string ss = InforShapeSplit.MatrixToString(TotalMatrixSplit[z]) + "||";
                TypeShape += ss;
                Debug.Log(ss);
            }

            Debug.Log(key);
            InforShape.Add(key, TotalMatrixSplit);
            List_Point.Add(key, ListPoint);
            ReadFile.WriteString(TypeShape);

        }
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                string TypeShape = "CUT" + " - ";
                string key = keyRoll + " - " + "[";
                int[] row = getListIntForward(i, matrix.GetLength(0));


                for (int r = 0; r < row.Length; r++)
                {
                    key += row[r];
                }
                key += "]";
                key += "[";
                int[] Column = new int[1] { j };
                for(int c = 0; c < Column.Length; c++)
                {
                    key += Column[c];
                }
                key += "]";

                TypeShape += key + " - ";
                if (List_Point.ContainsKey(key))
                {
                    continue;
                }
                List<Vector2> ListPoint = new List<Vector2>();
                List<int[,]> TotalMatrixSplit = SplitMatrix(matrix, row, Column, out ListPoint);
                Debug.Log("RENDER111 : " + TotalMatrixSplit.Count);
                for (int z = 0; z < TotalMatrixSplit.Count; z++)
                {
                    string ss = InforShapeSplit.MatrixToString(TotalMatrixSplit[z]) + "||";
                    TypeShape += ss;
                    Debug.Log(ss);
                }

                Debug.Log(key);
                InforShape.Add(key, TotalMatrixSplit);
                List_Point.Add(key, ListPoint);
                ReadFile.WriteString(TypeShape);

            }
        }


        for (int j = 0; j < matrix.GetLength(1); j++)
            {
                string TypeShape = "CUT" + " - ";
                string key = keyRoll + " - ";
                string Row = "[]";
                key += Row;
                int[] Column = getListIntBefor(j);
                key += "[";
                for (int c = 0; c < Column.Length; c++)
                {
                    key += c.ToString();
                }
                key += "]";
                
                TypeShape += key + " - ";
            if (List_Point.ContainsKey(key))
            {
                continue;
            }
            List<Vector2> ListPoint = new List<Vector2>();
                List<int[,]> TotalMatrixSplit = SplitMatrix(matrix,null,Column, out ListPoint);
                Debug.Log("RENDER111 : " + TotalMatrixSplit.Count);
                for (int z = 0; z < TotalMatrixSplit.Count; z++)
                {
                    string ss = InforShapeSplit.MatrixToString(TotalMatrixSplit[z]) + "||";
                    TypeShape += ss;
                    Debug.Log(ss);
                }

                Debug.Log(key);
                InforShape.Add(key, TotalMatrixSplit);
                List_Point.Add(key, ListPoint);
                ReadFile.WriteString(TypeShape);
            }

       

        for (int j = 0; j < matrix.GetLength(1); j++)
        {
            string TypeShape = "CUT" + " - ";
            string key = keyRoll + " - ";
            string Row = "[]";
            key += Row;
            int[] Column = getListIntForward(j, matrix.GetLength(1));
            key += "[";
            for (int c = 0; c < Column.Length; c++)
            {
                key += Column[c];
            }
            key += "]";

            TypeShape += key + " - ";
            if (List_Point.ContainsKey(key))
            {
                continue;
            }
            List<Vector2> ListPoint = new List<Vector2>();
            List<int[,]> TotalMatrixSplit = SplitMatrix(matrix, null, Column, out ListPoint);
            Debug.Log("RENDER111 : " + TotalMatrixSplit.Count);
            for (int z = 0; z < TotalMatrixSplit.Count; z++)
            {
                string ss = InforShapeSplit.MatrixToString(TotalMatrixSplit[z]) + "||";
                TypeShape += ss;
                Debug.Log(ss);
            }

            Debug.Log(key);
            InforShape.Add(key, TotalMatrixSplit);
            List_Point.Add(key, ListPoint);
            ReadFile.WriteString(TypeShape);
        }
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                string TypeShape = "CUT" + " - ";
                string key = keyRoll + " - "+"[";


                int[] Row = new int[1] { i };

                for(int r = 0; r < Row.Length; r++)
                {
                    key += Row[r];
                }

                key += "]";
               
                int[] Column = getListIntForward(j, matrix.GetLength(1));
                key += "[";
                for (int c = 0; c < Column.Length; c++)
                {
                    key += Column[c];
                }
                key += "]";

                TypeShape += key + " - ";
                if (List_Point.ContainsKey(key))
                {
                    continue;
                }
                List<Vector2> ListPoint = new List<Vector2>();
                List<int[,]> TotalMatrixSplit = SplitMatrix(matrix,Row, Column, out ListPoint);
                Debug.Log("RENDER111 : " + TotalMatrixSplit.Count);
                for (int z = 0; z < TotalMatrixSplit.Count; z++)
                {
                    string ss = InforShapeSplit.MatrixToString(TotalMatrixSplit[z]) + "||";
                    TypeShape += ss;
                    Debug.Log(ss);
                }

                Debug.Log(key);
                InforShape.Add(key, TotalMatrixSplit);
                List_Point.Add(key, ListPoint);
                ReadFile.WriteString(TypeShape);
            }
        }
        //Final 

        for (int i = -1; i < matrix.GetLength(0); i++)
        {
            for (int j = -1; j < matrix.GetLength(1); j++)
            {
                Debug.Log("RORRRR : " + i + j);
                string TypeShape = "CUT" + " - ";
                string key = keyRoll + " - " + "[";

                int[] row;
                int[] column;
                if (i != -1)
                {
                    row = new int[1] { i };
                }
                else
                {
                    row = null;
                }

                if (j != -1)
                {
                    column = new int[1] { j };
                }
                else
                {
                    column = null;
                }

                if (i != -1)
                {
                    for (int r = 0; r < row.Length; r++)
                    {
                        key += row[r];
                    }
                }
              
                
                key += "]";
                key += "[";
                if (j != -1)
                {
                    for (int c = 0; c < column.Length; c++)
                    {
                        key += column[c];
                    }
                }
               
               
                key += "]";
                TypeShape += key + " - ";

                Debug.Log("KEY ::::::::: "+key);
                if (InforShape.ContainsKey(key))
                {
                    continue;
                }
                List<Vector2> ListPoint = new List<Vector2>();
                List<int[,]> TotalMatrixSplit = new List<int[,]>();
                if (i == -1)
                {
                    TotalMatrixSplit = SplitMatrix(matrix,null, column, out ListPoint);
                }else if(j == -1)
                {
                    TotalMatrixSplit = SplitMatrix(matrix, row, null, out ListPoint);
                }
                else
                {
                    TotalMatrixSplit = SplitMatrix(matrix, row, column, out ListPoint);
                }

                //Debug.Log("RENDER111 : " + TotalMatrixSplit.Count);
                for (int z = 0; z < TotalMatrixSplit.Count; z++)
                {
                    string ss = InforShapeSplit.MatrixToString(TotalMatrixSplit[z]) + "||";
                    TypeShape += ss;
                    Debug.Log(ss);
                }

                Debug.Log(key);
                
                InforShape.Add(key, TotalMatrixSplit);
                List_Point.Add(key, ListPoint);
                ReadFile.WriteString(TypeShape);
              //  Debug.log("Complete : " + key);
            }
        }




    }

    public static string MatrixToString(int[,] matrix)
    {

        string s = "";
        for(int i = 0; i < matrix.GetLength(0); i++)
        {
          
            for(int j = 0; j < matrix.GetLength(1); j++)
            {
                
                s += matrix[i, j];
                if (j != matrix.GetLength(1) - 1)
                {
                    s += " ";
                }
            }
            if(i!=matrix.GetLength(0)-1)
              s += ",";
        }
        return s;
    }

    public int[] getListIntBefor(int index)
    {
        List<int> IndexBefor = new List<int>();
        for(int i = 0; i <=index; i++)
        {
            IndexBefor.Add(i);
        }
        return IndexBefor.ToArray();    
    }
    public int[] getListIntForward(int index,int max)
    {
        List<int> IndexBefor = new List<int>();
        for (int i = index; i <max; i++)
        {
            IndexBefor.Add(i);
        }
        return IndexBefor.ToArray();
    }

    public static int[,] StringToMatrix(string s)
    {
        int[,] matrix = null;
        string[] RowSplit = s.Split(',');
        int row = s.Split(',').Length;
        int column = s.Split(',')[0].Split(' ').Length;
        matrix = new int[row, column];
        for (int i = 0; i < row; i++)
        {
            string[] SplitColumn = RowSplit[i].Split(' ');
            for (int j = 0; j < SplitColumn.Length; j++)
            {
                matrix[i, j] = int.Parse(SplitColumn[j]);

            }
        }



        return matrix;
    }

    public List<int[,]> SplitMatrix(int[,] matrix, int[] row1, int[] col1,out List<Vector2> ListPoint)
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

        ListPoint = GrounpPoint(ListPointRow, ListPointColumn);

        Debug.Log("SPLIT POINT ");

        for (int i = 0; i < ListPoint.Count; i++)
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
        for (int i = 0; i < ListMatrix.Count; i++)
        {


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
        for (int i = 0; i < ListColumn.Count; i++)
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
            Debug.Log(CtrlGamePlay.RenderList(ListRow[i]));
        }
        List<int[,]> ListMatrix = new List<int[,]>();

        List<int> Flag = SetUpFlag(matrixs.GetLength(0), row);
        for (int i = 0; i < ListRow.Count; i++)
        {
            Debug.Log(CtrlGamePlay.RenderList(Flag));
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
[System.Serializable]
public class SaveFileGame
{
    public string s;
    public Dictionary<string, List<int[,]>> InforShape = new Dictionary<string, List<int[,]>>();
    public Dictionary<string, List<Vector2>> List_Point = new Dictionary<string, List<Vector2>>();


}
public class InforForSplitShape
{
    public Dictionary<string, List<int[,]>> InforShape = new Dictionary<string, List<int[,]>>();
    public Dictionary<string, List<Vector2>> List_Point = new Dictionary<string, List<Vector2>>();
    public List<int[,]> GetMatrix(string key)
    {
        return InforShape[key];
    }
    public List<Vector2> GetPoint(string key)
    {
        return List_Point[key];
    }
}

