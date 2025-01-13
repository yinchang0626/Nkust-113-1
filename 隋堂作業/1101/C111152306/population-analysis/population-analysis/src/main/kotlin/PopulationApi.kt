package com.example.population

import com.google.gson.Gson
import spark.Spark.*
import java.sql.Connection

class PopulationApi(private val connection: Connection) {
    private val gson = Gson()

    fun setupRoutes() {
        // 查詢指定地區的人口年齡分布
        get("/api/population/:region") { req, res ->
            val region = req.params("region")
            res.type("application/json")

            val query = """
                SELECT age, SUM(population) as population
                FROM population_data
                WHERE region = ?
                GROUP BY age
            """
            val statement = connection.prepareStatement(query)
            statement.setString(1, region)

            val resultSet = statement.executeQuery()
            val populationByAgeGroup = mutableMapOf<String, Int>()

            // 將查詢結果依照每 10 歲分組
            while (resultSet.next()) {
                val age = resultSet.getInt("age")
                val population = resultSet.getInt("population")

                // 將年齡分組成每 10 歲一個區間
                val ageGroup = when (age) {
                    in 0..9 -> "0-9"
                    in 10..19 -> "10-19"
                    in 20..29 -> "20-29"
                    in 30..39 -> "30-39"
                    in 40..49 -> "40-49"
                    in 50..59 -> "50-59"
                    in 60..69 -> "60-69"
                    in 70..79 -> "70-79"
                    in 80..89 -> "80-89"
                    in 90..99 -> "90-99"
                    else -> "100-109"
                }

                // 累加每個區間的人口數
                populationByAgeGroup[ageGroup] = populationByAgeGroup.getOrDefault(ageGroup, 0) + population
            }

            // 將結果轉換成 JSON 格式並回傳
            gson.toJson(populationByAgeGroup)
        }

        // 查詢指定地區的歷月人口變化
        get("/api/population/trend/:region") { req, res ->
            val region = req.params("region")
            res.type("application/json")

            val query = """
        SELECT REPLACE(year, '"', '') AS month, SUM(population) as population
        FROM population_data
        WHERE region = ?
        GROUP BY month
        ORDER BY month
    """
            val statement = connection.prepareStatement(query)
            statement.setString(1, region)

            val resultSet = statement.executeQuery()
            val populationTrend = mutableMapOf<String, Int>()

            while (resultSet.next()) {
                val month = resultSet.getString("month")
                val population = resultSet.getInt("population")
                populationTrend[month] = population
            }

            gson.toJson(populationTrend)
        }

        get("/api/population/:region/:month") { req, res ->
            val region = req.params("region")
            val month = req.params("month")
            res.type("application/json")

            val query = """
        SELECT age, SUM(population) as population
        FROM population_data
        WHERE region = ? AND year LIKE ?
        GROUP BY age
    """
            val statement = connection.prepareStatement(query)
            statement.setString(1, region)
            statement.setString(2, "%${month}月")

            val resultSet = statement.executeQuery()
            val populationByAgeGroup = mutableMapOf<String, Int>()

            while (resultSet.next()) {
                val age = resultSet.getInt("age")
                val population = resultSet.getInt("population")
                val ageGroup = when (age) {
                    in 0..9 -> "0-9"
                    in 10..19 -> "10-19"
                    in 20..29 -> "20-29"
                    in 30..39 -> "30-39"
                    in 40..49 -> "40-49"
                    in 50..59 -> "50-59"
                    in 60..69 -> "60-69"
                    in 70..79 -> "70-79"
                    in 80..89 -> "80-89"
                    in 90..99 -> "90-99"
                    else -> "100-109"
                }
                populationByAgeGroup[ageGroup] = populationByAgeGroup.getOrDefault(ageGroup, 0) + population
            }

            gson.toJson(populationByAgeGroup)
        }
    }
}
