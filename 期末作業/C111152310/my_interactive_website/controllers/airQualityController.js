// controllers/airQualityController.js
const mysql = require('mysql2/promise');

// 建立連線池 (pool) 以重複利用連線
const pool = mysql.createPool({
  host: 'air-quality-sql.cb6066o8cpi6.ap-northeast-3.rds.amazonaws.com',        // 你的 MySQL 主機
  user: 'admin',             // MySQL 帳號
  password: 'qwe2123806',     // MySQL 密碼
  database: 'air_quality_data' // MySQL 資料庫名稱
});

/**
 * 範例：查詢 air_quality 資料表。
 * 可以依需要實作篩選/查詢條件。
 */
async function getAirQualityData(filters) {
  let sql = 'SELECT * FROM air_quality';
  const params = [];

  // 如果前端有帶 county，就加上 WHERE 條件
  if (filters.county) {
    sql += ' WHERE county = ?';
    params.push(filters.county);
  }

  const [rows] = await pool.query(sql, params);
  return rows;
}

module.exports = {
  getAirQualityData
};
