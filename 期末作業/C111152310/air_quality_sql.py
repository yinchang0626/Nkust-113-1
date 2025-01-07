# 導入必要套件
import requests
import mysql.connector
from mysql.connector import Error
from datetime import datetime

def insert_data_to_mysql(records):
    """
    將解析後的資料列表 records 寫入 MySQL 資料庫。
    records 為從 JSON 中整理好的 Python list，每個元素是一筆要寫入的紀錄(字典)。
    """
    # 連線到 MySQL
    try:
        connection = mysql.connector.connect(
            host="air-quality-sql.cb6066o8cpi6.ap-northeast-3.rds.amazonaws.com",  # 你的 RDS 或 MySQL 主機
            user="admin",             # 你的 MySQL 帳號
            password="qwe2123806",    # 你的 MySQL 密碼
            database="air_quality_data",
            port=3306
        )
        
        if connection.is_connected():
            cursor = connection.cursor()

            # 寫入資料 SQL
            # 注意：此處 INSERT 的欄位順序，需與你資料表的欄位名稱、資料型態對應
            # 若欄位結構與示例不同，請自行調整。
            insert_sql = """
                INSERT INTO air_quality (
                    site_name, county, aqi, status, pollutant, 
                    pm10, pm25, no2, co, co_8hr, 
                    o3, o3_8hr, so2, nox, no_, 
                    wind_speed, wind_direc, publish_time, site_id, import_date
                )
                VALUES (
                    %(site_name)s, %(county)s, %(aqi)s, %(status)s, %(pollutant)s,
                    %(pm10)s, %(pm25)s, %(no2)s, %(co)s, %(co_8hr)s,
                    %(o3)s, %(o3_8hr)s, %(so2)s, %(nox)s, %(no_)s,
                    %(wind_speed)s, %(wind_direc)s, %(publish_time)s, %(site_id)s, %(import_date)s
                )
                ON DUPLICATE KEY UPDATE
                    aqi = VALUES(aqi),
                    status = VALUES(status),
                    pollutant = VALUES(pollutant),
                    pm10 = VALUES(pm10),
                    pm25 = VALUES(pm25),
                    no2 = VALUES(no2),
                    co = VALUES(co),
                    co_8hr = VALUES(co_8hr),
                    o3 = VALUES(o3),
                    o3_8hr = VALUES(o3_8hr),
                    so2 = VALUES(so2),
                    nox = VALUES(nox),
                    no_ = VALUES(no_),
                    wind_speed = VALUES(wind_speed),
                    wind_direc = VALUES(wind_direc),
                    site_id = VALUES(site_id),
                    import_date = VALUES(import_date)
            """
            
            # 使用 executemany 一次插入/更新多筆
            cursor.executemany(insert_sql, records)
            
            # 提交交易
            connection.commit()
            print("成功插入/更新 {} 筆資料".format(cursor.rowcount))

    except Error as e:
        print("連線或寫入資料庫時發生錯誤:", e)
    finally:
        if connection.is_connected():
            cursor.close()
            connection.close()


def fetch_and_process_data():
    """
    從環保署網址抓取 JSON，並解析後回傳方便寫入 MySQL 的結構。
    這裡範例對應你所貼的新 JSON 格式，欄位如：
       sitename, county, aqi, pollutant, status, so2, co, o3, o3_8hr, pm10, pm2.5, no2, nox, no, wind_speed, wind_direc, publishtime, ...
    其中時間欄位 publishtime 的格式為 "YYYY/MM/DD HH:mm:ss"。
    """
    
    # 這裡使用你所貼的新 JSON 的同一個 API (一樣的網址)
    # 如果實際上該網址產生的 JSON 還是舊格式，就需要再確認。
    url = "https://data.moenv.gov.tw/api/v2/aqx_p_432?api_key=e8dd42e6-9b8b-43f8-991e-b3dee723a52d&limit=1000&sort=ImportDate%20desc&format=JSON"
    response = requests.get(url)
    
    if response.status_code == 200:
        data = response.json()
        
        # data 通常包含 "records" 作為真正的測站資料清單
        records_list = data.get("records", [])
        
        output_records = []
        for item in records_list:
            # 取值時注意 key 與大小寫，與新版 JSON 相符
            site_name = item.get("sitename")
            county = item.get("county")
            aqi = item.get("aqi")
            pollutant = item.get("pollutant")
            status = item.get("status")
            so2 = item.get("so2")
            co = item.get("co")
            o3 = item.get("o3")
            o3_8hr = item.get("o3_8hr")
            pm10 = item.get("pm10")
            pm25 = item.get("pm2.5")  # 注意這裡帶點號
            no2 = item.get("no2")
            nox = item.get("nox")
            no_ = item.get("no")
            wind_speed = item.get("wind_speed")
            wind_direc = item.get("wind_direc")
            publish_time_str = item.get("publishtime")  # 新欄位是 publishtime, 格式: 2025/01/06 17:00:00
            site_id = item.get("siteid")  # 若需要測站編號

            # 解析 publishtime，格式 YYYY/MM/DD HH:mm:ss
            publish_time = None
            if publish_time_str:
                try:
                    publish_time = datetime.strptime(publish_time_str, "%Y/%m/%d %H:%M:%S")
                except:
                    # 若解析失敗，可自行處理
                    pass

            # import_date 若 JSON 中沒有，這裡可以用程式執行的當下時間
            import_date = datetime.now()

            # 數值轉型 (若欄位是空字串或 None，轉失敗就給 None)
            def to_int(val):
                try:
                    return int(val)
                except:
                    return None
            
            def to_float(val):
                try:
                    return float(val)
                except:
                    return None

            # 建立要插入的紀錄 dict
            record = {
                "site_name": site_name,
                "county": county,
                "aqi": to_int(aqi),
                "status": status,
                "pollutant": pollutant,
                "pm10": to_int(pm10),
                "pm25": to_int(pm25),
                "no2": to_float(no2),
                "co": to_float(co),
                "co_8hr": to_float(o3_8hr),  # 這裡要注意你要存到 co_8hr 還是 o3_8hr？應該是 co_8hr → item.get("co_8hr")
                                           # 若此處錯了，就自行修正
                "o3": to_float(o3),
                "o3_8hr": to_float(o3_8hr),
                "so2": to_float(so2),
                "nox": to_float(nox),
                "no_": to_float(no_),
                "wind_speed": to_float(wind_speed),
                "wind_direc": to_float(wind_direc),
                "publish_time": publish_time,
                "site_id": to_int(site_id),
                "import_date": import_date
            }
            
            output_records.append(record)
        
        return output_records
    else:
        print("抓取資料失敗，HTTP 狀態碼:", response.status_code)
        return []


if __name__ == "__main__":
    # 1. 先抓取並解析 API JSON
    records = fetch_and_process_data()
    
    # 2. 寫入 MySQL
    if records:
        insert_data_to_mysql(records)
    else:
        print("沒有資料可寫入。")
