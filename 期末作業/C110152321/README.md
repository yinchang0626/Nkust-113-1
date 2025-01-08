# Open Data Project

## 介紹
這是一個用於管理課程、用戶和留言的系統。用戶可以使用預設的帳號進行登入，登入成功後可以進行以下操作：

- 新增資料
- 更新資料
- 刪除資料
- 查看留言
- 新增留言

![search1](https://github.com/Shirleen03/Nkust-113-1/blob/main/%E6%9C%9F%E6%9C%AB%E4%BD%9C%E6%A5%AD/C110152321/image/search1.png)
![search2](https://github.com/Shirleen03/Nkust-113-1/blob/main/%E6%9C%9F%E6%9C%AB%E4%BD%9C%E6%A5%AD/C110152321/image/search2.png)

## 預設帳號資訊
- **用戶名**: 王
- **密碼**: 1

![login](https://github.com/Shirleen03/Nkust-113-1/blob/main/%E6%9C%9F%E6%9C%AB%E4%BD%9C%E6%A5%AD/C110152321/image/login.png)

## 如何架設網站

### 步驟 1: 下載 SQL 檔案
1. 訪問以下連結下載所需的 SQL 檔案：
   [下載 SQL 檔案](https://ithelp.ithome.com.tw/articles/10259766)

### 步驟 2: 設定 MySQL 資料庫
1. 開啟 MySQL，並輸入以下命令來建立資料庫和資料表：

   開啟命令提示字元，並進入 MySQL 目錄：
   ```bash
   C:\Program Files\MySQL\MySQL Server 9.1\bin> .\mysql -u root -p
建立資料表：
sql
複製程式碼
CREATE DATABASE open_data_project;

CREATE TABLE Courses(
    course_name VARCHAR(255) NOT NULL,
    sessions INT NOT NULL,
    participants INT NOT NULL
);

CREATE TABLE Users (
    id INT AUTO_INCREMENT PRIMARY KEY,
    username VARCHAR(255) UNIQUE NOT NULL,
    password VARCHAR(255) NOT NULL
);

CREATE TABLE Messages (
    id INT AUTO_INCREMENT PRIMARY KEY,
    username VARCHAR(255) NOT NULL,
    message TEXT NOT NULL,
    timestamp TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (username) REFERENCES Users(username)
);
步驟 3: 匯入 CSV 資料
sql
複製程式碼
USE open_data_project;

LOAD DATA INFILE 'C:/ProgramData/MySQL/MySQL Server 9.1/Uploads/Taipei_Courses_112.csv'
INTO TABLE Courses
FIELDS TERMINATED BY ','
ENCLOSED BY '"'
LINES TERMINATED BY '\r\n'
IGNORE 1 ROWS;
步驟 4: 訪問網站
配置好資料庫後，啟動您的本地伺服器。
在瀏覽器中開啟 localhost，即可使用該系統。
![cmd](https://github.com/Shirleen03/Nkust-113-1/blob/main/%E6%9C%9F%E6%9C%AB%E4%BD%9C%E6%A5%AD/C110152321/image/cmd.png)

**結論**
這個 `README.md` 文件包含了您提到的架設過程，並描述了如何設置 MySQL 資料庫、匯入 CSV 資料，以及如何在瀏覽器中訪問該網站。
