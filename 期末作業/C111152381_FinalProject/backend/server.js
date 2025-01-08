// backend/server.js
const express = require('express');
const app = express();
const cors = require('cors');
const financialRoutes = require('../backend/financial');


const PORT = 3000;

// Middleware
app.use(cors());
app.use(express.json());

// Routes
app.use('/api/financial', financialRoutes);

// 啟動伺服器
app.listen(PORT, (err) => {
    if(err){
        console.log('Server failed to start:', err);
    }else{
        console.log(`Server is running on http://localhost:${PORT}`);
    }
});
