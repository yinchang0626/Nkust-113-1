using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class useful
{
    public int year = 0;
    public float income = 0;
    public float outcome = 0;
    public float finance = 0;

    public useful(int year, float income, float outcome, float finance)
    {
        this.year = year;
        this.income = income;
        this.outcome = outcome;
        this.finance = finance;
    }
}

class Program
{
    static void Main()
    {
        string filePath = @"E:\software\隋堂作業\1101\C110152305\ConsoleApp1\ConsoleApp1\data.csv";
        List<useful> usefuls= new List<useful>();

        using(StreamReader sr= new StreamReader(filePath))
        {
            string line;
            bool header = true;

            while ((line = sr.ReadLine()) != null)
            {
                if (header)
                {
                    header= false;
                    continue;
                }

                string[] value = line.Split(",");
                int year = int.Parse(value[0]);
                float income = float.Parse(value[1]);
                float outcome = float.Parse(value[2]);
                float finance = float.Parse(value[3]);

                useful willuse=new useful(year, income, outcome, finance);
                usefuls.Add(willuse);
            }
        }
        //all print out
        Console.WriteLine("年度\t收入\t\t支出\t\t財務");
        foreach (useful willuse in usefuls)
        {
            Console.WriteLine($"{willuse.year}\t{willuse.income}\t\t{willuse.outcome}\t\t{willuse.finance}");
        }

        //total data
        int count = usefuls.Count;
        Console.WriteLine("\n資料總數: " + count);

        var incomeValues=usefuls.Select(willuse=> willuse.income);
        var outcomeValues = usefuls.Select(willuse => willuse.outcome);
        var financeValues=usefuls.Select(usefuls=>usefuls.finance);

        //income
        Console.WriteLine("\n總收入統計: ");
        Console.WriteLine("最大值: " + incomeValues.Max());
        Console.WriteLine("最小值: "+incomeValues.Min());
        Console.WriteLine("平均值: " + incomeValues.Average());

        //outcome
        Console.WriteLine("\n總支出統計: ");
        Console.WriteLine("最大值: " + outcomeValues.Max());
        Console.WriteLine("最小值: " + outcomeValues.Min());
        Console.WriteLine("平均值: " + outcomeValues.Average());

        //finance
        Console.WriteLine("\n高捷財務統計: ");
        Console.WriteLine("最大值: " + financeValues.Max());
        Console.WriteLine("最小值: " + financeValues.Min());
        Console.WriteLine("平均值: " + financeValues.Average());

        Console.ReadKey();
    }
}