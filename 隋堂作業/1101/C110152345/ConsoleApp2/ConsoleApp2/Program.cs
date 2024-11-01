using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;

/*
2. * *數據分析與處理：**
   -使用 C# 撰寫程式讀取並分析該資料集。
   - 根據數據的結構，提取出有意義的資訊，如：
     - 數據的總量
     - 不同類別的數據分布
     - 最大值、最小值、平均值等統計數據

3. **應用統計與展示：**
   - 使用 C# 進行一些簡單的應用統計，例如：
     - 類別統計（如不同地區、不同種類的數量對比）
     - 數據趨勢分析（如隨著時間的變化趨勢）
   - 將統計結果顯示在控制台，或使用簡單的圖形庫將結果視覺化展示（可選）。
*/

namespace ConsoleApplication5
{
    struct Data
    {
        public int number, date, sumload;

        public string week, update;

        public Data(int x, int y, int z, string i, string j) {

            number = x;
            date = y;
            sumload = z;
            
            week = i; 
            update = j;

        }

    }
    class Test
    {
        static void Swap(int a, int b, ref List<Int64> s)
        {
            Int64 temp = s[a];
            s[a] = s[b];
            s[b] = temp;
        }
        static void Partition(int low, int high, ref int pivotpoint,ref List<Int64> s)
        {
            Int64 pivotpointitem = s[low];

            int j = low;
            for (int i = low + 1; i <= high; i++)
            {
                if (s[i] < pivotpointitem)
                {
                    j++;
                    /*
                    int temp = s[i];
                    s[i] = s[j];
                    s[j] = temp;
                    */
                    Swap(i, j, ref s);
                }
            }
            pivotpoint = j;
            /*
            int ttemp = s[low];
            s[low] = s[pivotpoint];
            s[pivotpoint] = ttemp;
            */
            Swap(low,  pivotpoint, ref s);
        }
        static void Quicksort(int low, int high,ref List<Int64> s)
        {
            int pivotpoint = 0;
            if (low < high)
            {
                Partition(low, high,ref pivotpoint,ref s);
                Quicksort(low, pivotpoint - 1,ref s);
                Quicksort(pivotpoint + 1, high,ref s);
            }
        }

        public static void Main()
        {
            try
            {
                //path @"C:\Users\user\Desktop\202408.csv" or "C:\\Users\\user\\Desktop\\202408.csv"
                using StreamReader reader = new StreamReader(@"臺中捷運全系統旅運量統計202408.csv", Encoding.UTF8);
                
                string readstr = string.Empty;
                List<Data> data = new List<Data>();
                int start = 0;
                while ((readstr = reader.ReadLine()) != null)
                {
                    //Console.WriteLine(readstr);
                    string item = "";
                    Data d = new Data();
                    int order = 1;
                    if (start > 0)
                    {
                        foreach (char word in readstr)
                        {
                            if (word != ',') item = item + word.ToString();
                            else if (word == ',')
                            {
                                switch (order)
                                {
                                    case 1:
                                        d.number = Convert.ToInt32(item);
                                        break;
                                    case 2:
                                        d.date = Convert.ToInt32(item);
                                        break;
                                    case 3:
                                        d.week = item;
                                        break;
                                    case 4:
                                        d.sumload = Convert.ToInt32(item);
                                        break;
                                    case 5:
                                        d.update = item;
                                        break;
                                    default:
                                        break;
                                }
                                order++;
                                item = "";
                            }
                        }
                        //Console.WriteLine(d.sumload);
                        data.Add(d);
                    }
                    start++;
                } 
                reader.Close();

                List<Int64> sumloadlist = new List<Int64> ();
                int allsumload = 0;
                foreach(Data i in data)
                {
                    allsumload = allsumload + i.sumload;
                    sumloadlist.Add(i.sumload);
                    //Console.WriteLine(i.sumload);
                }
                int num = 1;
                Quicksort(0, sumloadlist.Count-1,ref sumloadlist);
                foreach (int i in sumloadlist)
                {
                    Console.WriteLine(num.ToString() + ": " + i.ToString());
                    num++;
                }
                Console.WriteLine("幾筆總運量: " + sumloadlist.Count);
                Console.WriteLine("全部的總運量: " + allsumload.ToString());
                Console.WriteLine("最大的總運量: " + sumloadlist[sumloadlist.Count - 1].ToString());
                Console.WriteLine("最小的總運量: " + sumloadlist[0].ToString());
                Console.WriteLine("中位數的總運量: " + sumloadlist[((sumloadlist.Count + 1) / 2) - 1].ToString());
                Console.WriteLine("平均的總運量: " + (allsumload/sumloadlist.Count).ToString());
                

            }
            catch (Exception e)
            {
                // Let the user know what went wrong.
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
            Console.ReadLine();
        }
    }

}

