# C110152318 徐士諭 - 軟體工程期末專題

## 內容
### 影片
[![軟體工程期末專題影片](https://img.youtube.com/vi/KrWNrv9An9M/0.jpg)](https://youtu.be/KrWNrv9An9M)

### 帳號登入登出和註冊功能
![alt text](image/{DF22BDDB-E1AE-4131-8F9D-99C108AFDCC2}.png)
![alt text](image/{F119E534-036F-40EA-A007-4886EF6FD1DB}.png)
### 儲存密碼sha256、記錄帳號登入狀態
![帳號登入登出示意圖](image/{08C2EF74-42B6-4CD5-9E9B-22F594D2D703}.png)
### 新增課程刪除課程、搜尋
![alt text](image/{8379A91D-8CA6-4C66-874A-4669F76ED3B1}.png)
![alt text](image/{1BE08B45-1CD7-45CC-96A1-BCCA1BC4CD0D}.png)
![alt text](image/{E489864D-4045-479B-AF70-CA4E7438C152}.png)
### 上傳作業、檢視上傳的作業、下載作業、上傳時間及刪除等等
![alt text](image/{EBDE540A-EDF7-43E4-94C7-D96A0B0A1437}.png)
![alt text](image/{D06AB9B3-9674-4F64-BD16-34A098ABD820}.png)
![alt text](image/{A8B80521-3870-42FD-AF0A-E329CB55C0CE}.png)
### 顯示課程資料、以選擇課程、課程描述
![alt text](image/{57E044D9-3227-4E9B-BEED-C62BD5042D5C}.png)
### 資料庫關聯更新刪除資料等等
![alt text](image/{2B70171E-A483-44B9-AC7E-5B12904A967A}.png)
### 背景及布局設定
![alt text](image/{600DF314-6D49-4D45-8F5D-FD947C8CAFB5}.png)

## 資料庫
- 連結資料庫
![alt text](image/{2ADA8AD2-F0FB-481E-9BE7-8FF423401FCE}.png)
- appsettings.json改成自己的資料庫
- 刪除Migration、資料表和wwwroot/update裡面的資料
- 在Visual Studio的套件管理器主控台
    - `Add-Migration CreateMigration`
    - `Update-DataBase`

## 延伸模組
- Microsoft.EntityFrameworkCore.SqlServer
- Microsoft.EntityFrameworkCore.Tools
- Microsoft.AspNetCore.Authentication.Cookies