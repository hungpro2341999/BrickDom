using System.Collections;
using System.Collections.Generic;

using System;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
public class ToHop 
{
    
   
        #region Khai báo
       public int n, k;
       public int[] a, dau;
    public List<List<int>> ListTohop = new List<List<int>>();
    #endregion

  
       
    public  void Xuat(int i)
        {
        List<int> Row = new List<int>();
        string s = "";
            for (int j = 1; j <= k; j++)
        {
            s += a[j] + " ";
            if (!Row.Contains(a[j]))
            {
                Row.Add(a[j]);
            }
            else
            {
                return;
            }
           
           
        }
        ListTohop.Add(Back(Row));

      //  Debug.Log(s);
            
        }
    

      public  void Duyet(int i)
        {
            if (i > k) Xuat(i);
            else
            {
                int j;
                 
                for (j = a[i - 1]; j <= n; j++)
                {
                    if (dau[j] == 0)
                    {
                        a[i] = j;
                        dau[j] = 1;
                        Duyet(i + 1);
                        dau[j] = 0;
                    }
                }
            }
        }

    public List<int> Back(List<int> List)
    {
        for (int i = 0; i < List.Count; i++)
        {
            List[i]--;
        }
        return List;
    }
    public List<List<int>> Start_Find(int n,int k) {

        this.n = n;
        this.k = k;
        ListTohop = new List<List<int>>();
           a = new int[n + 1];
            dau = new int[n + 1];
            for (int i = 0; i < n + 1; i++)
                dau[i] = 0;
          

          
          
            a[0] = 1;
            Duyet(1);
        for(int i = 0; i < ListTohop.Count; i++) 
        {

            string s = "";
            for(int j = 0; j < ListTohop[i].Count; j++)
            {
                s += ListTohop[i][j] + " ";
            }
           
        }
        return ListTohop;
            
        }

    
}

