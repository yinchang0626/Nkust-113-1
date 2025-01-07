// routes/airQualityRoutes.js
const express = require('express');
const router = express.Router();
const airQualityController = require('../controllers/airQualityController');

router.get('/', async (req, res) => {
  try {
    const { county } = req.query; // 從 querystring 取得選單傳來的縣市
    // 如果沒選擇，county 可能是空字串

    const data = await airQualityController.getAirQualityData({ county });
    res.json({ success: true, data });
  } catch (error) {
    console.error(error);
    res.status(500).json({ success: false, message: '伺服器錯誤' });
  }
});

module.exports = router;
