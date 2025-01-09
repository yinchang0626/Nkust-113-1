using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class oofer
{
    public int year = 0;
    public int supervisor_gas = 0;
    public int supervisor_stone = 0;
    public int worker_gas = 0;
    public int worker_stone = 0;

    public oofer(int year, int supervisor_gas, int supervisor_stone, int worker_gas, int worker_stone)
    {
        this.year = year;
        this.supervisor_gas = supervisor_gas;
        this.supervisor_stone = supervisor_stone;
        this.worker_gas = worker_gas;
        this.worker_stone = worker_stone;
    }
}

class Program
{
    static void Main()
    {
        string filePath = @"D:\Course\113 first half\software engineering\0109\ConsoleApp2\ConsoleApp2\data.csv";
        List<oofer> oofers = new List<oofer>();

        using (StreamReader sr = new StreamReader(filePath))
        {
            string line;
            bool isHeader = true;

            while ((line = sr.ReadLine()) != null)
            {
                if (isHeader)
                {
                    isHeader = false;
                    continue;
                }

                string[] values = line.Split(',');

                int year = int.Parse(values[0]);
                int supervisor_gas = int.Parse(values[1]);
                int supervisor_stone = int.Parse(values[2]);
                int worker_gas = int.Parse(values[3]);
                int worker_stone = int.Parse(values[4]);

                oofer o = new oofer(year, supervisor_gas, supervisor_stone, worker_gas, worker_stone);
                oofers.Add(o);
            }
        }

        // 資料總數
        int data_count = oofers.Count;
        Console.WriteLine("資料總數: " + data_count);

        // 統計數據的計算
        var supervisorGasValues = oofers.Select(o => o.supervisor_gas);
        var supervisorStoneValues = oofers.Select(o => o.supervisor_stone);
        var workerGasValues = oofers.Select(o => o.worker_gas);
        var workerStoneValues = oofers.Select(o => o.worker_stone);

        // 監工 (Gas) 統計
        Console.WriteLine("\nSupervisor Gas:");
        Console.WriteLine("最大值: " + supervisorGasValues.Max());
        Console.WriteLine("最小值: " + supervisorGasValues.Min());
        Console.WriteLine("平均值: " + supervisorGasValues.Average());

        // 監工 (Stone) 統計
        Console.WriteLine("\nSupervisor Stone:");
        Console.WriteLine("最大值: " + supervisorStoneValues.Max());
        Console.WriteLine("最小值: " + supervisorStoneValues.Min());
        Console.WriteLine("平均值: " + supervisorStoneValues.Average());

        // 工人 (Gas) 統計
        Console.WriteLine("\nWorker Gas:");
        Console.WriteLine("最大值: " + workerGasValues.Max());
        Console.WriteLine("最小值: " + workerGasValues.Min());
        Console.WriteLine("平均值: " + workerGasValues.Average());

        // 工人 (Stone) 統計
        Console.WriteLine("\nWorker Stone:");
        Console.WriteLine("最大值: " + workerStoneValues.Max());
        Console.WriteLine("最小值: " + workerStoneValues.Min());
        Console.WriteLine("平均值: " + workerStoneValues.Average());

        // 逐行列印資料
        Console.WriteLine("\nYear\tSupervisor Gas\tSupervisor Stone\tWorker Gas\tWorker Stone");
        foreach (oofer o in oofers)
        {
            Console.WriteLine($"{o.year}\t{o.supervisor_gas}\t\t{o.supervisor_stone}\t\t\t{o.worker_gas}\t\t{o.worker_stone}");
        }

        Console.ReadKey();
    }
}