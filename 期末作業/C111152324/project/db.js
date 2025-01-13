const mysql = require('mysql2/promise'); // 使用 promise 支援更佳的異步處理

const pool = mysql.createPool({
    host: 'database-1.c54qu48sokg2.ap-northeast-3.rds.amazonaws.com',
    user: 'garden',
    password: 'a12345678',
    database: 'a1',
    waitForConnections: true,
    connectionLimit: 10,
    queueLimit: 0,
});

module.exports = pool;
