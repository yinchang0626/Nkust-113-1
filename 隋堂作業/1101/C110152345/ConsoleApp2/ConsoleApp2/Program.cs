using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;



namespace ConsoleApplication5
{
    struct Data
    {
        public string number, date, sumload;

        public string week, update;

        public Data(string x, string y, string z, string i, string j) {

            number = x;
            date = y;
            sumload = z;
            
            week = i; 
            update = j;

        }

    }
    class Test
    {
        public static void Main()
        {
            try
            {
                //path @"C:\Users\user\Desktop\202408.csv" or "C:\\Users\\user\\Desktop\\202408.csv"
                StreamReader reader = new StreamReader(@"C:\Users\user\Desktop\202408.csv", Encoding.UTF8);
                
                string readstr = string.Empty;
                List<Data> data = new List<Data>();
                int start = 0;
                while ((readstr = reader.ReadLine()) != null)
                {
                    Console.WriteLine(readstr);
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
                                        d.number = item;
                                        break;
                                    case 2:
                                        d.date = item;
                                        break;
                                    case 3:
                                        d.week = item;
                                        break;
                                    case 4:
                                        d.sumload = item;
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
                        Console.WriteLine(d.sumload);
                        data.Add(d);
                    }
                    start++;
                }
                    
                reader.Close();
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

