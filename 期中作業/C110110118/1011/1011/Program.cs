using System;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace VisionDataAnalysis
{
    public class VisionData
    {
        public string 地區 { get; set; }
        public string 項目 { get; set; }
        public string 欄位名稱 { get; set; }
        public int 數值 { get; set; }
        public string 資料時間日期 { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var filePath = @"C:/1101/C110110118/1011/1011/2023.csv"; // 新的檔案路徑
            var records = new List<VisionData>();

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Encoding = System.Text.Encoding.UTF8,
                HeaderValidated = null,
                MissingFieldFound = null
            };

            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, config))
            {
                records = csv.GetRecords<VisionData>().ToList();
            }

            // 數據總量
            Console.WriteLine($"2023臺中市國民中學學生裸眼視力檢查");

            // 正則表達式來提取年級、公私立、性別及視力狀況（視力不良或視力檢查）等資訊
            var gradePattern = new Regex(@"國中_(.+?)年級");
            var schoolTypePattern = new Regex(@"^(國立|市立|私立)");
            var genderPattern = new Regex(@"_(男|女)");
            var visionStatusPattern = new Regex(@"(視力不良|視力檢查)");

            // 公私立學校的數量分布
            var schoolTypeCounts = records
                .GroupBy(r => schoolTypePattern.Match(r.欄位名稱).Value)
                .Select(g => new { SchoolType = g.Key, Count = g.Sum(r => r.數值) })
                .ToList();

            Console.WriteLine("\n國立/市立/私立學校之學生數量:");
            foreach (var schoolType in schoolTypeCounts)
            {
                Console.WriteLine($"{schoolType.SchoolType}: {schoolType.Count}");
            }

            // 各年級及男女生的總人數、近視人數及近視比例
            var gradeGenderStats = records
                .Where(r => visionStatusPattern.IsMatch(r.欄位名稱))
                .GroupBy(r => new { Grade = gradePattern.Match(r.欄位名稱).Value, Gender = genderPattern.Match(r.欄位名稱).Value })
                .Select(g =>
                {
                    var gradeGender = g.Key;
                    var totalStudents = g.Where(r => r.欄位名稱.Contains("視力檢查")).Sum(r => r.數值);
                    var nearsightedStudents = g.Where(r => r.欄位名稱.Contains("視力不良")).Sum(r => r.數值);
                    var nearsightedRatio = totalStudents > 0 ? (double)nearsightedStudents / totalStudents * 100 : 0;

                    return new
                    {
                        Grade = gradeGender.Grade,
                        Gender = gradeGender.Gender,
                        TotalStudents = totalStudents,
                        NearsightedStudents = nearsightedStudents,
                        NearsightedRatio = nearsightedRatio
                    };
                })
                .ToList();

            Console.WriteLine("\n各年級男女生總人數、近視人數及近視比例:");
            foreach (var stat in gradeGenderStats)
            {
                Console.WriteLine($"{stat.Grade} {stat.Gender}: 總人數 = {stat.TotalStudents}, 近視人數 = {stat.NearsightedStudents}, 近視比例 = {stat.NearsightedRatio:F2}%");
            }

            // 視力狀況分類
            var visionStatusCounts = records
                .GroupBy(r => visionStatusPattern.Match(r.欄位名稱).Value)
                .Select(g => new { VisionStatus = g.Key == "視力檢查" ? "視力檢查總人數" : g.Key, Count = g.Sum(r => r.數值) })
                .ToList();

            Console.WriteLine("\n視力狀況分類:");
            foreach (var status in visionStatusCounts)
            {
                Console.WriteLine($"{status.VisionStatus}: {status.Count}");
            }

            // 計算所有學生的近視比例
            var totalStudentsOverall = records.Where(r => r.欄位名稱.Contains("視力檢查")).Sum(r => r.數值);
            var totalNearsightedOverall = records.Where(r => r.欄位名稱.Contains("視力不良")).Sum(r => r.數值);
            var overallNearsightedRatio = totalStudentsOverall > 0 ? (double)totalNearsightedOverall / totalStudentsOverall * 100 : 0;

            Console.WriteLine($"\n所有學生的近視比例: {overallNearsightedRatio:F2}%");

            // 計算所有男生和女生的近視比例
            var maleTotal = records.Where(r => r.欄位名稱.Contains("男") && r.欄位名稱.Contains("視力檢查")).Sum(r => r.數值);
            var maleNearsighted = records.Where(r => r.欄位名稱.Contains("男") && r.欄位名稱.Contains("視力不良")).Sum(r => r.數值);
            var femaleTotal = records.Where(r => r.欄位名稱.Contains("女") && r.欄位名稱.Contains("視力檢查")).Sum(r => r.數值);
            var femaleNearsighted = records.Where(r => r.欄位名稱.Contains("女") && r.欄位名稱.Contains("視力不良")).Sum(r => r.數值);

            var maleNearsightedRatio = maleTotal > 0 ? (double)maleNearsighted / maleTotal * 100 : 0;
            var femaleNearsightedRatio = femaleTotal > 0 ? (double)femaleNearsighted / femaleTotal * 100 : 0;

            Console.WriteLine($"\n所有男生的近視比例: {maleNearsightedRatio:F2}%");
            Console.WriteLine($"所有女生的近視比例: {femaleNearsightedRatio:F2}%");

            //Console.WriteLine("\n分析完成！");
        }
    }
}
