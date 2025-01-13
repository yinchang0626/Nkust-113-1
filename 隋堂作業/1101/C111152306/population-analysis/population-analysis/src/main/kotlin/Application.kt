package com.example.population

import java.io.File
import java.sql.Connection
import java.sql.DriverManager
import spark.Spark.*
import com.google.gson.Gson

fun main() {
    // 連接到 SQLite 資料庫
    val connection = getDatabaseConnection()

    // 啟用靜態檔案支援（前端檔案）
    staticFiles.location("/static")

    // 初始化 API 路由
    val populationApi = PopulationApi(connection)
    populationApi.setupRoutes()

    println("伺服器啟動中...")
}

// 建立 SQLite 資料庫連線
fun getDatabaseConnection(): Connection {
    val url = "jdbc:sqlite:src/main/database/population.db"
    return DriverManager.getConnection(url)
}

// 從 CSV 檔案讀取資料並插入到資料庫
fun readDataFromCsv(connection: Connection, filePath: String) {
    val file = File(filePath)
    val lines = file.readLines()

    if (lines.size < 2) {
        println("CSV 檔案內容不足！")
        return
    }

    val ageGroups = lines[0].split(",").drop(2)

    lines.drop(1).forEachIndexed { lineNumber, line ->
        val parts = line.split(",")

        if (parts.size < 2 + ageGroups.size) {
            println("跳過不完整的資料行（第 $lineNumber 行）：$line")
            return@forEachIndexed
        }

        val regionAndGender = parts[0].split("/")

        if (regionAndGender.size < 3) {
            println("跳過不完整的地區和性別資料（第 $lineNumber 行）：$line")
            return@forEachIndexed
        }

        val year = regionAndGender[0].trim()
        val region = regionAndGender[1].trim()
        val gender = regionAndGender[2].trim()

        ageGroups.forEachIndexed { index, ageGroup ->
            val populationStr = parts[index + 2].trim()
            val population = populationStr.toIntOrNull()

            if (population == null || population <= 0) {
                return@forEachIndexed
            }

            // 處理年齡段
            val age = when {
                ageGroup.contains("歲以上") -> 100
                ageGroup.contains("不詳") -> -1
                else -> ageGroup.filter { it.isDigit() }.toIntOrNull() ?: 0
            }

            insertData(connection, year, region, gender, age, population)
            println("已插入資料：年份=$year，地區=$region，性別=$gender，年齡=$age，人口數=$population")
        }
    }
    println("CSV 資料已匯入！")
}

// 插入資料到資料表
fun insertData(connection: Connection, year: String, region: String, gender: String, age: Int, population: Int) {
    val statement = connection.prepareStatement(
        "INSERT INTO population_data (year, region, gender, age, population) VALUES (?, ?, ?, ?, ?)"
    )
    statement.setString(1, year)
    statement.setString(2, region)
    statement.setString(3, gender)
    statement.setInt(4, age)
    statement.setInt(5, population)
    statement.executeUpdate()
}
