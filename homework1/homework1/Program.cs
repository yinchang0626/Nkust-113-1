using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Data.Common;

class Program
{
    static async Task Main(string[] args)
    {
        string url = "https://data.moenv.gov.tw/api/v2/aqx_p_432?api_key=e8dd42e6-9b8b-43f8-991e-b3dee723a52d&limit=1000&sort=ImportDate desc&format=JSON";
        string county; 
        string worst_Cite;
        string best_Cite;

        List<int> Aqi_list = new List<int>();
        int avg_Aqi;
        int worst_Aqi;
        int best_Aqi;
        int Aqi;

        int worst_PM10;
        int best_PM10;
        int worst_PM25;
        int best_PM25;

        int first_index;
        bool valid;

        int i, j;

        //取得AQI資料
        using HttpClient client = new HttpClient();
        string jsonString = await client.GetStringAsync(url);
        JObject jsonObject = JObject.Parse(jsonString);

        //提示用戶輸入
        Console.WriteLine("請輸入城市名稱(ex:臺北市)");
        county = Console.ReadLine();

        //初始化數值
        valid = false;
        first_index = 0;

        //檢查資料中是否有該城市
        for(i = 0; i < jsonObject["records"].Count(); i++)
        {
            if (county == (string) jsonObject["records"][i]["county"]) {
                valid = true;
                first_index = i;
                break;
            }
        }


        if (!valid)
        {
            Console.WriteLine("沒有此縣市");
        }
        else
        {
            //初始化數值
            worst_Aqi = (int)jsonObject["records"][first_index]["aqi"];
            worst_Cite = (string)jsonObject["records"][first_index]["county"];
            worst_PM10 = (int)jsonObject["records"][first_index]["pm10"];
            worst_PM25 = (int)jsonObject["records"][first_index]["pm2.5"];
            best_Aqi = worst_Aqi;
            best_Cite = worst_Cite;
            best_PM10 = worst_PM10;
            best_PM25 = worst_PM25;
            j = 0;

            for (i = 0; i < 84; i++)
            {
                try//防止null
                {
                    if ((string)jsonObject["records"][first_index + j]["county"] == county)
                    {
                        //取得所有數值
                        Aqi = (int)jsonObject["records"][first_index + j]["aqi"];
                        Aqi_list.Add(Aqi);

                        if (worst_Aqi < Aqi)
                        {
                            worst_Aqi = Aqi;
                            worst_Cite = (string)jsonObject["records"][first_index + j]["sitename"];
                            worst_PM10 = (int)jsonObject["records"][first_index + j]["pm10"];
                            worst_PM25 = (int)jsonObject["records"][first_index + j]["pm2.5"];
                        }
                        if (best_Aqi > Aqi)
                        {
                            best_Aqi = Aqi;
                            best_Cite = (string)jsonObject["records"][first_index + j]["sitename"];
                            best_PM10 = (int)jsonObject["records"][first_index + j]["pm10"];
                            best_PM25 = (int)jsonObject["records"][first_index + j]["pm2.5"];
                        }
                        j++;
                    }
                }catch (Exception ex) { continue; }
            }


            Console.WriteLine($"\n{county}總共有{j}個觀測站");
            Console.WriteLine($"平均空氣品質AQI為{Math.Round(Aqi_list.Average())}\n");

            Console.WriteLine($"空氣最差的觀測站是{worst_Cite}觀測站");
            Console.WriteLine($"AQI:{worst_Aqi}");
            Console.WriteLine($"PM10:{worst_PM10}");
            Console.WriteLine($"PM2.5:{worst_PM25}\n");

            Console.WriteLine($"空氣最好的觀測站是{best_Cite}觀測站");
            Console.WriteLine($"AQI:{best_Aqi}");
            Console.WriteLine($"PM10:{best_PM10}");
            Console.WriteLine($"PM2.5:{best_PM25} \n");

            Console.WriteLine($"資料時間:{(string)jsonObject["records"][first_index + j]["publishtime"]}");
            Console.Read();
        }

        
    }
}