const express = require('express');
const router = express.Router();
const app = express();
const sql = require('../backend/db'); // 確保正確引入 db.js 中的 sql 模組

// 獲取財務數據 (可篩選年份)
router.get('/data', async (req, res) => {
    const { startYear, endYear } = req.query;
    let query = 'SELECT * FROM dbo.money';
    let queryParams = [];

    if (startYear && endYear) {
        query += ' WHERE year BETWEEN @startYear AND @endYear'; // 使用參數化查詢
        queryParams.push({ name: 'startYear', type: sql.Int, value: parseInt(startYear) });
        queryParams.push({ name: 'endYear', type: sql.Int, value: parseInt(endYear) });
    }

    try {
        const pool = await sql.connect(); // 確保連接到資料庫
        const request = pool.request();

        // 添加參數到請求中
        queryParams.forEach(param => {
            request.input(param.name, param.type, param.value);
        });

        const result = await request.query(query);
        res.json(result.recordset); // 返回查詢結果
    } catch (err) {
        console.error('Database connection error:', err); // 更詳細的錯誤信息
        if (!res.headersSent) { // 確保只在未發送響應的情況下發送錯誤響應
            return res.status(500).json({ error: 'Database query failed', details: err.message });
        }
    }
});

module.exports = router;