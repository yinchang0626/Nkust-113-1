# -*- coding: utf-8 -*-

import requests
import json
import mysql.connector
from mysql.connector import Error
from datetime import datetime

def insert_data_to_mysql(records):
    """
    將解析後的資料列表 records 寫入 MySQL 資料庫。
    records 為從 JSON 中整理好的 Python list，每個元素是一筆紀錄(字典)。
    """
    try:
        # 1. 連線到 MySQL
        connection = mysql.connector.connect(
            host="database-1.c54qu48sokg2.ap-northeast-3.rds.amazonaws.com",
            user="admin",
            password="a12345678",
            database="a1",
            port=3306
        )
        
        if connection.is_connected():
            cursor = connection.cursor()

            # 2. 準備 INSERT 語法
            # 假設想以 case_code 作為唯一識別，避免重複插入
            # → ON DUPLICATE KEY UPDATE
            #   若你確定在 my_table 內 case_code 欄位已設為 UNIQUE，才會生效
            insert_sql = """
                INSERT INTO my_table (
                    case_code,
                    market_name,
                    addr,
                    business_week,
                    context,
                    ValidDate,
                    Latitude,
                    Lontitude,
                    type,
                    badge_code,
                    business_hours,
                    business_hurs_end,
                    last_edited_date
                )
                VALUES (
                    %(case_code)s,
                    %(market_name)s,
                    %(addr)s,
                    %(business_week)s,
                    %(context)s,
                    %(ValidDate)s,
                    %(Latitude)s,
                    %(Lontitude)s,
                    %(type)s,
                    %(badge_code)s,
                    %(business_hours)s,
                    %(business_hurs_end)s,
                    %(last_edited_date)s
                )
                ON DUPLICATE KEY UPDATE
                    market_name = VALUES(market_name),
                    addr = VALUES(addr),
                    business_week = VALUES(business_week),
                    context = VALUES(context),
                    ValidDate = VALUES(ValidDate),
                    Latitude = VALUES(Latitude),
                    Lontitude = VALUES(Lontitude),
                    type = VALUES(type),
                    badge_code = VALUES(badge_code),
                    business_hours = VALUES(business_hours),
                    business_hurs_end = VALUES(business_hurs_end),
                    last_edited_date = VALUES(last_edited_date)
            """

            # 3. 一次執行多筆插入
            cursor.executemany(insert_sql, records)

            # 4. 提交交易
            connection.commit()
            print(f"成功插入/更新 {cursor.rowcount} 筆資料")

    except Error as e:
        print("連線或寫入資料庫時發生錯誤:", e)
    finally:
        if connection.is_connected():
            cursor.close()
            connection.close()

def fetch_and_process_data():
    """
    從農委會提供的 API 網址抓取 JSON，並解析後回傳方便寫入 MySQL 的結構。
    URL: https://data.moa.gov.tw/Service/OpenData/TransService.aspx?UnitId=tR9TIFWlvquB&IsTransData=1
    JSON 結構內容: 
       [
         {
           "case_code": "...",
           "market_name": "...",
           "addr": "...",
           ...
         }, ...
       ]
    """
    url = "https://data.moa.gov.tw/Service/OpenData/TransService.aspx?UnitId=tR9TIFWlvquB&IsTransData=1"
    response = requests.get(url)
    
    if response.status_code == 200:
        try:
            data_list = response.json()  # 預期是一個 list
        except json.JSONDecodeError:
            print("JSON 解析失敗！")
            return []
        
        # 解析並組成可以插入資料庫的 list[dict]
        output_records = []
        for item in data_list:
            # 取值
            case_code        = item.get("case_code")
            market_name      = item.get("market_name")
            addr             = item.get("addr")
            business_week    = item.get("business_week")
            context          = item.get("context")
            
            valid_date_str   = item.get("ValidDate")
            latitude_str     = item.get("Latitude")
            lontitude_str    = item.get("Lontitude")
            
            store_type       = item.get("type")
            badge_code       = item.get("badge_code")
            business_hours   = item.get("business_hours")
            business_hurs_end= item.get("business_hurs_end")
            last_edited_str  = item.get("last_edited_date")

            # 處理日期字串
            def parse_datetime(dt_str):
                if not dt_str:
                    return None
                try:
                    return datetime.strptime(dt_str, "%Y-%m-%dT%H:%M:%S")
                except:
                    # 若解析不過，可再嘗試 "%Y-%m-%d %H:%M:%S" 或其他格式
                    return None

            valid_date_parsed = parse_datetime(valid_date_str)
            last_edited_parsed= parse_datetime(last_edited_str)

            # 經緯度轉 float
            def to_float(val):
                try:
                    return float(val)
                except:
                    return None

            lat = to_float(latitude_str)
            lon = to_float(lontitude_str)

            record_dict = {
                "case_code": case_code,
                "market_name": market_name,
                "addr": addr,
                "business_week": business_week,
                "context": context,
                "ValidDate": valid_date_parsed,
                "Latitude": lat,
                "Lontitude": lon,
                "type": store_type,
                "badge_code": badge_code,
                "business_hours": business_hours,
                "business_hurs_end": business_hurs_end,
                "last_edited_date": last_edited_parsed
            }
            output_records.append(record_dict)
        return output_records
    else:
        print("抓取資料失敗，HTTP 狀態碼:", response.status_code)
        return []

def main():
    # 1. 抓取並解析 API JSON
    records = fetch_and_process_data()
    
    # 2. 寫入 MySQL
    if records:
        insert_data_to_mysql(records)
    else:
        print("沒有資料可寫入。")

if __name__ == "__main__":
    main()
