const express = require('express');
const bodyParser = require('body-parser');
const cors = require('cors');
const pool = require('./db'); // 引用資料庫連線模組

const app = express();

app.use(cors());
app.use(bodyParser.json());
app.use(express.static('public'));

// 測試 API：取得所有資料表中的資料
app.get('/data', async (req, res) => {
    try {
        const [results] = await pool.query('SELECT * FROM my_table');
        res.json(results);
    } catch (err) {
        console.error('資料查詢失敗:', err.message);
        res.status(500).send('資料查詢失敗');
    }
});

// 搜尋資料
app.get('/search', async (req, res) => {
    const { keyword } = req.query;
    const searchKeyword = `%${keyword}%`;
    try {
        const [results] = await pool.query(
            'SELECT * FROM my_table WHERE market_name LIKE ? OR addr LIKE ?',
            [searchKeyword, searchKeyword]
        );
        res.json(results);
    } catch (err) {
        console.error('搜尋資料失敗:', err.message);
        res.status(500).send('搜尋失敗');
    }
});

// 新增資料
app.post('/add', async (req, res) => {
    const { case_code, market_name, addr, business_hours, business_hurs_end } = req.body;
    try {
        const query =
            'INSERT INTO my_table (case_code, market_name, addr, business_hours, business_hurs_end) VALUES (?, ?, ?, ?, ?)';
        await pool.query(query, [case_code, market_name, addr, business_hours, business_hurs_end]);
        res.send('新增成功');
    } catch (err) {
        console.error('新增資料失敗:', err.message);
        res.status(500).send('新增失敗');
    }
});

// 修改資料
app.post('/edit', async (req, res) => {
    const { case_code, market_name, addr, business_hours, business_hurs_end } = req.body;
    try {
        const query = `
            UPDATE my_table 
            SET market_name = ?, addr = ?, business_hours = ?, business_hurs_end = ? 
            WHERE case_code = ?
        `;
        await pool.query(query, [market_name, addr, business_hours, business_hurs_end, case_code]);
        res.send('修改成功');
    } catch (err) {
        console.error('修改資料失敗:', err.message);
        res.status(500).send('修改失敗');
    }
});

// 刪除資料
app.post('/delete', async (req, res) => {
    const { case_code } = req.body;
    try {
        const query = 'DELETE FROM my_table WHERE case_code = ?';
        await pool.query(query, [case_code]);
        res.send('刪除成功');
    } catch (err) {
        console.error('刪除資料失敗:', err.message);
        res.status(500).send('刪除失敗');
    }
});

// 啟動伺服器
app.listen(3000, () => {
    console.log('伺服器運行在 http://localhost:3000');
});
