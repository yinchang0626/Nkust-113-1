# 人口統計資料分析系統

本專案為一個基於政府開放資料的 **人口統計資料分析系統**，使用 **Kotlin** 作為後端開發語言，搭配 **SQLite** 資料庫和 **Chart.js** 前端圖表庫，實現了一個互動式的網頁應用，提供使用者查詢和視覺化人口統計資料的功能。

---

## 🔧 **系統架構**
- **後端框架**：Kotlin + Spark Java
- **資料庫**：SQLite
- **前端技術**：HTML、CSS、JavaScript、Chart.js
- **API**：提供人口統計資料查詢和趨勢分析的 RESTful API

---

## 📂 **專案結構**
```plaintext
population-analysis/
├── src/
│   └── main/
│       ├── kotlin/
│       │   └── com/example/population/
│       │       └── PopulationApi.kt
│       ├── resources/
│       │   └── population_data.csv
│       └── static/
│           ├── index.html
│           ├── trend.html
│           ├── style.css
│           └── script.js
└── README.md
```

---

## 🚀 **功能介紹**
### 1️⃣ 年齡區間分布圖
- 選擇地區和月份，查看該地區不同年齡段的人口分布。
- 使用圓餅圖視覺化每 10 歲年齡段的人口占比。

### 2️⃣ 歷年趨勢分析
- 選擇多個地區，查看各地區在不同月份的人口變化趨勢。
- 使用折線圖視覺化不同地區的變化趨勢。

---

## 📄 **API 文件**
### 🔹 查詢地區人口年齡分布
**GET** `/api/population/:region/:month`
- **描述**：查詢指定地區和月份的人口年齡分布資料。
- **參數**：
  - `region`：地區名稱
  - `month`：月份數字（例如 `1` 表示 1 月）

### 🔹 查詢地區歷年人口趨勢
**GET** `/api/population/trend/:region`
- **描述**：查詢指定地區歷年的人口變化趨勢。
- **參數**：
  - `region`：地區名稱

---

## 🛠 **環境設定與執行方式**
### **1️⃣ 安裝 Kotlin 和 SQLite**
- 確認已安裝 **Kotlin** 開發環境。
- 確認已安裝 **SQLite**。

### **2️⃣ 專案執行**
1. 將專案 Clone 到本地端：
   ```bash
   git clone https://github.com/your-repo/population-analysis.git
   ```
2. 進入專案目錄：
   ```bash
   cd population-analysis
   ```
3. 執行專案：
   ```bash
   ./gradlew run
   ```
4. 打開瀏覽器並進入 `http://localhost:4567` 查看網頁。

---

## 📊 **圖表說明**
### 年齡區間分布圖
- 每個年齡區間以圓餅圖呈現。
- 可以選擇月份

### 歷年趨勢圖
- 每個地區的歷年人口數以折線圖呈現。
- 支援多地區勾選，方便比較不同地區的人口趨勢。

---

## 🧩 **未來改進方向**
1. 提供更多的篩選條件，例如性別、人均年齡等。
2. 增加用戶認證和登入功能，實現個人化數據儀表板。
3. 優化前端介面，提供更多的圖表類型和視覺化效果。

---

## 📝 **開發人員**
- **開發者**：C111152306 馮翊旻
- **聯絡方式**：C111152306@nkust.edu.tw

---

## 📎 **License**
本專案採用 [MIT License](https://opensource.org/licenses/MIT) 授權。

