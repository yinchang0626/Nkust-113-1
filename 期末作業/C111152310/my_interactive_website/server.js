// server.js
console.log('*** __filename:', __filename);
console.log('*** __dirname:', __dirname);
console.log('*** server.js: Starting... ***');
const express = require('express');
const app = express();
const path = require('path'); // 確保有引入 path
console.log('server.js START'); // 測試訊息

// 解析 body
app.use(express.json());
app.use(express.urlencoded({ extended: true }));

// 靜態檔案
app.use(express.static(path.join(__dirname, 'public')));

// 路由
const airQualityRoutes = require('./routes/airQualityRoutes');
app.use('/api/airQuality', airQualityRoutes);

// 啟動伺服器
const PORT = 3000;
app.listen(PORT, () => {
  console.log(`Server is running on http://localhost:${PORT}`);
});
