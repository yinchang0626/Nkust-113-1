// main.js

// 取得前端畫面元素
const searchBtn = document.getElementById('searchBtn');
const countySelect = document.getElementById('countySelect');
const resultBody = document.getElementById('resultBody');

// Chart 相關變數 (若需多種圖表，可再宣告其他變數)
let myChart = null;

// 綁定查詢按鈕事件
searchBtn.addEventListener('click', async () => {
  // 取得下拉選單的值
  const county = countySelect.value.trim();

  try {
    // 若有選擇縣市，帶參數 ?county=xxx；若沒有就不帶參數
    let url = '/api/airQuality';
    if (county) {
      url += `?county=${encodeURIComponent(county)}`;
    }

    const res = await fetch(url);
    const result = await res.json();

    if (result.success) {
      // 渲染表格
      renderTable(result.data);
      
      // 渲染圖表 (將空氣品質資料傳給 renderChart())
      renderChart(result.data);
    } else {
      alert('查詢失敗，請稍後再試');
    }
  } catch (error) {
    console.error(error);
    alert('發生錯誤');
  }
});

// 負責渲染表格
function renderTable(data) {
  // 清空舊資料
  resultBody.innerHTML = '';

  data.forEach(row => {
    const tr = document.createElement('tr');

    const siteNameTd = document.createElement('td');
    siteNameTd.textContent = row.site_name;

    const countyTd = document.createElement('td');
    countyTd.textContent = row.county;

    const aqiTd = document.createElement('td');
    aqiTd.textContent = row.aqi;

    const statusTd = document.createElement('td');
    statusTd.textContent = row.status;

    const pollutantTd = document.createElement('td');
    pollutantTd.textContent = row.pollutant;

    const pm25Td = document.createElement('td');
    pm25Td.textContent = row.pm25;

    const publishTimeTd = document.createElement('td');
    publishTimeTd.textContent = row.publish_time;

    tr.appendChild(siteNameTd);
    tr.appendChild(countyTd);
    tr.appendChild(aqiTd);
    tr.appendChild(statusTd);
    tr.appendChild(pollutantTd);
    tr.appendChild(pm25Td);
    tr.appendChild(publishTimeTd);
    resultBody.appendChild(tr);
  });
}

// 負責渲染圖表
function renderChart(data) {
  // 範例：取出「測站名稱」與「AQI」作為 x 軸與 y 軸資料
  // 你可依實際需求自行調整

  // 1. 整理資料：labels (x 軸) 以及對應的 AQI 數值 (y 軸)
  const labels = data.map(item => item.site_name);   // 測站
  const aqiData = data.map(item => Number(item.aqi) || 0); // AQI

  // 2. 如果先前已經建立過 myChart，就先銷毀以免重複疊加
  if (myChart) {
    myChart.destroy();
  }

  // 3. 取得畫布元素
  const ctx = document.getElementById('myChart').getContext('2d');

  // 4. 建立新的 Chart
  myChart = new Chart(ctx, {
    type: 'bar', // 可以改成 'line', 'pie' 等其他類型
    data: {
      labels: labels,
      datasets: [{
        label: '空氣品質指標 (AQI)',
        data: aqiData,
        backgroundColor: 'rgba(54, 162, 235, 0.2)', // 長條圖背景色
        borderColor: 'rgba(54, 162, 235, 1)',       // 邊框顏色
        borderWidth: 1
      }]
    },
    options: {
      responsive: true, // 響應式
      scales: {
        y: {
          beginAtZero: true
        }
      }
    }
  });
}
