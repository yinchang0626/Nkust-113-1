import mysql.connector
import json

# MySQL 連線設定
connection = mysql.connector.connect(
    host="database-1.c54qu48sokg2.ap-northeast-3.rds.amazonaws.com",
    user="garden",
    password="a12345678",
    database="a1"  # 替換為你的資料庫名稱
)
cursor = connection.cursor()

# 加載 JSON 文件
def load_json_to_mysql(json_file, table_name):
    with open(json_file, "r", encoding="utf-8") as file:
        data = json.load(file)

    # 假設 JSON 文件是一個列表，每個項目對應一行資料
    if isinstance(data, list):
        for record in data:
            columns = ", ".join(record.keys())
            placeholders = ", ".join(["%s"] * len(record))
            sql = f"INSERT INTO {table_name} ({columns}) VALUES ({placeholders})"
            values = tuple(record.values())

            try:
                cursor.execute(sql, values)
            except mysql.connector.Error as err:
                print(f"Error: {err}")

    else:
        print("JSON 文件格式不正確，預期為列表形式。")

    connection.commit()

# 使用範例
try:
    load_json_to_mysql("data.json", "my_table")  # 替換為你的表名稱
    print("數據導入成功！")
except Exception as e:
    print(f"發生錯誤: {e}")
finally:
    cursor.close()
    connection.close()