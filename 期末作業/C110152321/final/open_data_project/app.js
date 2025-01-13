// backend/app.js

const express = require('express');
const mysql = require('mysql2');
const cors = require('cors');
const jwt = require('jsonwebtoken');
const bcrypt = require('bcrypt');

const app = express();

// 中介軟體 (Middleware)
app.use(express.json()); // 解析 JSON 格式的請求體
app.use(cors()); // 啟用跨域資源共享 (CORS)

const SECRET_KEY = 'your_secret_key';
// MySQL 資料庫連接
const db = mysql.createConnection({
  host: 'localhost',
  user: 'root',
  password: 'shirleen0107', // 請更換為您的密碼
  database: 'open_data_project' // 資料庫名稱
});

// 連接到資料庫
db.connect((err) => {
  if (err) {
    console.error('資料庫連接錯誤:', err);
    return;
  }
  console.log('成功連接到 MySQL 資料庫。');
});

// 路由 (Routes)

// 取得所有課程資料
app.get('/courses', (req, res) => {
  const sql = 'SELECT * FROM Courses';
  db.query(sql, (err, results) => {
    if (err) {
      console.error('取得課程資料錯誤:', err);
      res.status(500).send('內部伺服器錯誤');
    } else {
      res.json(results); // 回傳資料
    }
  });
});

// 新增課程資料
app.post('/courses', (req, res) => {
  const { course_name, sessions, participants } = req.body;
  const sql = 'INSERT INTO Courses (course_name, sessions, participants) VALUES (?, ?, ?)';
  db.query(sql, [course_name, sessions, participants], (err, result) => {
    if (err) {
      console.error('新增課程資料錯誤:', err);
      res.status(500).send('內部伺服器錯誤');
    } else {
      res.status(201).send('課程新增成功。');
    }
  });
});

// 更新課程資料
app.put('/courses/:course_name', (req, res) => {
  const { course_name } = req.params;
  const { sessions, participants } = req.body;

  console.log('收到更新請求:');
  console.log('課程名稱:', course_name);
  console.log('更新場次:', sessions, '更新人次:', participants);

  const sql = 'UPDATE Courses SET sessions = ?, participants = ? WHERE course_name = ?';
  db.query(sql, [sessions, participants, course_name], (err, result) => {
    if (err) {
      console.error('更新課程資料錯誤:', err);
      res.status(500).send('內部伺服器錯誤');
    } else if (result.affectedRows > 0) {
      res.send('課程更新成功。');
    } else {
      res.status(404).send('未找到該課程，無法更新。');
    }
  });
});

// 刪除課程資料
app.delete('/courses/:course_name', (req, res) => {
  const { course_name } = req.params;
  const sql = 'DELETE FROM Courses WHERE course_name = ?';
  db.query(sql, [course_name], (err, result) => {
    if (err) {
      console.error('刪除課程資料錯誤:', err);
      res.status(500).send('內部伺服器錯誤');
    } else {
      res.send('課程刪除成功。');
    }
  });
});

// 用戶註冊
app.post('/register', async (req, res) => {
  const { username, password } = req.body;
  const hashedPassword = await bcrypt.hash(password, 10);
  const sql = 'INSERT INTO Users (username, password) VALUES (?, ?)';
  db.query(sql, [username, hashedPassword], err => {
    if (err) {
      res.status(500).send('註冊用戶時發生錯誤。');
    } else {
      res.status(201).send('用戶註冊成功。');
    }
  });
});

// 用戶登入
app.post('/login', (req, res) => {
  const { username, password } = req.body;
  const sql = 'SELECT * FROM Users WHERE username = ?';
  db.query(sql, [username], async (err, results) => {
    if (err || results.length === 0) {
      return res.status(401).send('用戶名或密碼無效。');
    }
    const user = results[0];
    const match = await bcrypt.compare(password, user.password);
    if (!match) {
      return res.status(401).send('用戶名或密碼無效。');
    }
    const token = jwt.sign({ username: user.username }, SECRET_KEY, { expiresIn: '1h' });
    res.json({ token });
  });
});

// 中介軟體，驗證用戶是否已登入

function authenticateToken(req, res, next) {
  const authHeader = req.headers['authorization'];
  const token = authHeader && authHeader.split(' ')[1];
  if (!token) return res.sendStatus(401); // 無 token 返回 401

  jwt.verify(token, SECRET_KEY, (err, user) => {
    if (err) {
      return res.status(403).send('Forbidden: Token invalid or expired'); // token 驗證錯誤，返回 403
    }
    req.user = user;
    next();
  });
}

// 留言路由
app.get('/messages', (req, res) => {
  const sql = 'SELECT * FROM Messages';
  db.query(sql, (err, results) => {
    if (err) {
      res.status(500).send('取得留言資料錯誤。');
    } else {
      res.json(results);
    }
  });
});

app.post('/messages', authenticateToken, (req, res) => {
  const { message } = req.body;
  const username = req.user.username;
  const sql = 'INSERT INTO Messages (username, message) VALUES (?, ?)';
  db.query(sql, [username, message], err => {
    if (err) {
      res.status(500).send('發送留言時發生錯誤。');
    } else {
      res.status(201).send('留言發送成功。');
    }
  });
});

// 獲取用戶資料的路由
app.get('/user', authenticateToken, (req, res) => {
  const username = req.user.username; // JWT 中的用戶名

  // 查詢資料庫中的用戶資料
  const sql = 'SELECT id, username FROM Users WHERE username = ?';
  db.query(sql, [username], (err, results) => {
    if (err) {
      console.error('獲取用戶資料錯誤:', err);
      return res.status(500).send('伺服器錯誤');
    }

    if (results.length > 0) {
      // 回傳用戶資料
      res.json({
        id: results[0].id,
        username: results[0].username
      });
    } else {
      res.status(404).send('用戶未找到');
    }
  });
});

// 例：如果使用 Cookie 儲存 token
app.post('/logout', (req, res) => {
  res.clearCookie('authToken');  // 假設你是使用 Cookie 儲存 token
  res.send({ message: '已註銷成功' });
});

// 刪除用戶帳號
app.delete('/users/del', async (req, res) => {
  const token = req.headers['authorization']?.split(' ')[1];
  if (!token) {
    return res.status(401).send('未授權');
  }

  try {
    const decoded = jwt.verify(token, 'your-secret-key'); // 解碼 token
    const userId = decoded.id; // 取出用戶 ID

    // 假設您有一個 User 模型，刪除用戶
    const result = await User.findByIdAndDelete(userId);
    if (result) {
      res.status(200).send('帳號已刪除');
    } else {
      res.status(404).send('找不到用戶');
    }
  } catch (error) {
    res.status(500).send('伺服器錯誤');
  }
});

// 啟動伺服器
const PORT = 3000;
app.listen(PORT, () => {
  console.log(`伺服器運行在 http://localhost:${PORT}`);
});
