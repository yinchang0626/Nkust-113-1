# 資料庫管理系統

## 簡介
# 目的是為了統計各區學生的課外學習裝況，並可以進一步分析各區學生差距
此專案是一個基於 Flask 後端和 HTML + JavaScript 前端的資料庫管理系統，主要用於管理不同行政區別的教育數據。該系統支援增、刪、改、查操作，並提供簡單的排序和篩選功能。

---
![image](https://github.com/user-attachments/assets/0aa318e5-2df6-44fe-b82d-383590984f3f)

## 功能
1. **資料查詢**
    - 支援根據行政區名稱篩選數據。
    - 提供按"總計"、"文理類合計"或"技藝類合計"排序功能。
![image](https://github.com/user-attachments/assets/07e07edc-1bb5-4d98-9718-c6641e2918a3)

2. **資料新增**
    - 允許用戶透過模態框新增行政區別及其數據。
![image](https://github.com/user-attachments/assets/38833891-1b26-47ea-9eac-8242a72bd372)
![image](https://github.com/user-attachments/assets/8cccf0a7-66e1-4c7f-abf7-e3735d1b0d81)
![image](https://github.com/user-attachments/assets/ccdefd82-a626-4d87-89f6-d5cb25d2ded8)

3. **資料編輯**
    - 支援修改現有資料並自動更新數據合計。
![image](https://github.com/user-attachments/assets/12e9aecf-c8d7-46f9-9de4-a8a300885733)
![image](https://github.com/user-attachments/assets/271519f7-8e2e-4b49-a966-b035bef332dd)
![image](https://github.com/user-attachments/assets/e0643b78-dfed-4ebd-b1ba-be1ebc3e1988)

4. **資料刪除**
    - 提供刪除特定行政區別資料的功能。
![image](https://github.com/user-attachments/assets/656545af-ea21-4013-b114-75727811a1c7)
![image](https://github.com/user-attachments/assets/aff216f7-6256-4015-9fdb-d273acf38ebe)
![image](https://github.com/user-attachments/assets/9fb1231e-2d5a-46df-a9dc-02c150cd7bfc)

---

## 系統架構

### 後端
- **框架**: Flask
- **資料庫**: MySQL

### 前端
- **技術**:
    - HTML5
    - JavaScript (搭配 Axios 處理 API 請求)
---

## 文件結構
```
|-- main.html          # 前端頁面 (HTML)
|-- DB.py              # 後端程式碼 (Flask)
```

## 未來改進
- 增加使用者驗證功能。
- 支援多語言界面。
- 提供的數據篩選與分析工具。
