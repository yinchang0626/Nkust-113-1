const sql = require('mssql');
const dotenv = require('dotenv');

dotenv.config();

const config = {
    user: process.env.DB_USER || 'sa',
    password: process.env.DB_PASSWORD || 'Passw0rd',
    server: process.env.DB_SERVER || 'localhost',
    database: process.env.DB_DATABASE || 'moneyDataBase',
    port: parseInt(process.env.DB_PORT) || 1433, // 確保這裡有 port 設置
    options: {
        encrypt: true,
        trustServerCertificate: true
    }
};

// 測試資料庫連接
async function testConnection() {
    try {
        await sql.connect(config);
        console.log('Connected to database.');
    } catch (err) {
        console.error('Database connection failed:', err);
    }
}

testConnection();

module.exports = sql; // 將 sql 模組導出