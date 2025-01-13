from flask import Flask, request, jsonify
from flask_cors import CORS
import MySQLdb

# Initialize the Flask app
app = Flask(__name__)
CORS(app)

# Configure MySQL connection
db_config = {
    'host': 'localhost',
    'user': 'root',
    'passwd': 'a148970721',
    'db': 'tutoring_data'
}

def get_db_connection():
    return MySQLdb.connect(**db_config)

@app.route('/data', methods=['GET'])
def get_data():
    conn = get_db_connection()
    cursor = conn.cursor(MySQLdb.cursors.DictCursor)

    # 提取參數
    district = request.args.get('district')
    range_min = request.args.get('rangeMin', type=int)
    range_max = request.args.get('rangeMax', type=int)

    # SQL 查詢
    query = """
        SELECT 
            `行政區別`, 
            `總計`, 
            `文理類合計`, 
            `技藝類合計`, 
            `文理類[法政]`, 
            `文理類[文理]`, 
            `文理類[外語]`, 
            `技藝類[音樂_舞蹈]`, 
            `技藝類[電機_汽車修護_建築_工藝_製圖類]`, 
            `技藝類[速讀]`, 
            `技藝類[資訊]`, 
            `技藝類[美術_書法_攝影_美工設計_圍棋]`, 
            `技藝類[美容_美髮_理髮]`, 
            `技藝類[縫紉]`, 
            `技藝類[瑜珈]`, 
            `技藝類[心算_珠算_會計]`, 
            `技藝類[家政_插花烹飪]`, 
            `技藝類[其他]`
        FROM data
        WHERE 1=1
    """
    params = []

    if district:
        query += " AND `行政區別` LIKE %s"
        params.append(f"%{district}%")

    if range_min is not None:
        query += " AND `總計` >= %s"
        params.append(range_min)

    if range_max is not None:
        query += " AND `總計` <= %s"
        params.append(range_max)

    cursor.execute(query, params)
    data = cursor.fetchall()

    cursor.close()
    conn.close()

    return jsonify(data)

@app.route('/data', methods=['POST'])
def add_data():
    new_data = request.json
    conn = get_db_connection()
    cursor = conn.cursor()

    # 計算合計
    文理類合計 = sum(new_data.get(key, 0) for key in ['文理類[法政]', '文理類[文理]', '文理類[外語]'])
    技藝類合計 = sum(new_data.get(key, 0) for key in [
        '技藝類[音樂_舞蹈]', '技藝類[電機_汽車修護_建築_工藝_製圖類]', '技藝類[速讀]',
        '技藝類[資訊]', '技藝類[美術_書法_攝影_美工設計_圍棋]', '技藝類[美容_美髮_理髮]',
        '技藝類[縫紉]', '技藝類[瑜珈]', '技藝類[心算_珠算_會計]', '技藝類[家政_插花烹飪]', '技藝類[其他]'
    ])
    總計 = 文理類合計 + 技藝類合計

    # SQL 插入語句
    query = """
        INSERT INTO data (
            `行政區別`, `總計`, `文理類合計`, `技藝類合計`,
            `文理類[法政]`, `文理類[文理]`, `文理類[外語]`, 
            `技藝類[音樂_舞蹈]`, `技藝類[電機_汽車修護_建築_工藝_製圖類]`, 
            `技藝類[速讀]`, `技藝類[資訊]`, `技藝類[美術_書法_攝影_美工設計_圍棋]`, 
            `技藝類[美容_美髮_理髮]`, `技藝類[縫紉]`, `技藝類[瑜珈]`, 
            `技藝類[心算_珠算_會計]`, `技藝類[家政_插花烹飪]`, `技藝類[其他]`
        ) VALUES (%s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s)
    """
    values = (
        new_data['district'], 總計, 文理類合計, 技藝類合計,
        new_data.get('文理類[法政]', 0), new_data.get('文理類[文理]', 0), new_data.get('文理類[外語]', 0),
        new_data.get('技藝類[音樂_舞蹈]', 0), new_data.get('技藝類[電機_汽車修護_建築_工藝_製圖類]', 0),
        new_data.get('技藝類[速讀]', 0), new_data.get('技藝類[資訊]', 0), new_data.get('技藝類[美術_書法_攝影_美工設計_圍棋]', 0),
        new_data.get('技藝類[美容_美髮_理髮]', 0), new_data.get('技藝類[縫紉]', 0), new_data.get('技藝類[瑜珈]', 0),
        new_data.get('技藝類[心算_珠算_會計]', 0), new_data.get('技藝類[家政_插花烹飪]', 0), new_data.get('技藝類[其他]', 0)
    )
    cursor.execute(query, values)

    conn.commit()
    cursor.close()
    conn.close()

    return jsonify({"message": "Data added successfully"}), 201

@app.route('/data', methods=['PUT'])
def update_bulk_data():
    updates = request.json.get('updates', [])
    if not updates:
        return jsonify({"message": "No updates provided"}), 400

    conn = get_db_connection()
    cursor = conn.cursor()

    try:
        for update in updates:
            文理類合計 = sum(update.get(key, 0) for key in ['文理類[法政]', '文理類[文理]', '文理類[外語]'])
            技藝類合計 = sum(update.get(key, 0) for key in [
                '技藝類[音樂_舞蹈]', '技藝類[電機_汽車修護_建築_工藝_製圖類]', '技藝類[速讀]',
                '技藝類[資訊]', '技藝類[美術_書法_攝影_美工設計_圍棋]', '技藝類[美容_美髮_理髮]',
                '技藝類[縫紉]', '技藝類[瑜珈]', '技藝類[心算_珠算_會計]', '技藝類[家政_插花烹飪]', '技藝類[其他]'
            ])
            總計 = 文理類合計 + 技藝類合計

            update['文理類合計'] = 文理類合計
            update['技藝類合計'] = 技藝類合計
            update['總計'] = 總計

            update_fields = []
            update_values = []

            for key, value in update.items():
                if key != 'district':
                    update_fields.append(f"`{key}` = %s")
                    update_values.append(value)

            update_values.append(update['district'])
            query = f"""
            UPDATE data SET {', '.join(update_fields)}
            WHERE `行政區別` = %s
            """
            cursor.execute(query, update_values)

        conn.commit()
    except MySQLdb.Error as e:
        conn.rollback()
        return jsonify({"message": f"Database error: {e}"}), 500
    finally:
        cursor.close()
        conn.close()

    return jsonify({"message": "Data updated successfully"})

@app.route('/data', methods=['DELETE'])
def delete_data():
    district = request.json.get('district')
    if not district:
        return jsonify({"message": "No district provided"}), 400

    conn = get_db_connection()
    cursor = conn.cursor()

    query = "DELETE FROM data WHERE `行政區別` = %s"
    cursor.execute(query, (district,))

    conn.commit()
    cursor.close()
    conn.close()

    return jsonify({"message": "Data deleted successfully"})

if __name__ == '__main__':
    app.run(debug=True)
