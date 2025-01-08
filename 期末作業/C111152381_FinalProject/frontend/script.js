const tableBody = document.getElementById('tableBody');
const ctx = document.getElementById('financialChart').getContext('2d');
const applyFiltersBtn = document.getElementById('applyFilters');
// 綁定篩選按鈕
applyFiltersBtn.addEventListener('click', fetchData);

// 繪製圖表
let financialChart;


const renderChart = (data) =>{
    if (financialChart) {
        financialChart.destroy();
    }
    financialChart = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: data.map(item => item.Year),
            datasets: [
                {
                    label: 'Operational Income',
                    data: data.map(item => item.revenue),
                    backgroundColor: 'green'
                },
                {
                    label: 'Non-operational Income',
                    data: data.map(item => item.extraJobIncome),
                    backgroundColor: 'blue'
                }
            ]
        }
    });
}

// 渲染表格

const renderTable = (data) => {
    tableBody.innerHTML = '';
    data.forEach(item => {
        tableBody.innerHTML += `
            <tr>
                <td>${item.Year}</td> 
                <td>${item.revenue}</td> 
                <td>${item.extraJobIncome}</td> 
                <td>${item.expenses}</td> 
                <td>${item.NonOperatingExpenses}</td> 
            </tr>
        `;
    });
}

// 加載數據
async function fetchData() {
    const startYear = document.getElementById('startYear').value;
    const endYear = document.getElementById('endYear').value;

    let url = 'http://localhost:3000/api/financial/data';
    if (startYear && endYear) {
        url += `?startYear=${startYear}&endYear=${endYear}`;
    }

    try {
        const response = await fetch(url);
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
        const data = await response.json();
        console.log('Fetched data:', data); // 日誌輸出獲取的數據
        renderTable(data); // 渲染表格
        renderChart(data); // 渲染圖表
    } catch (error) {
        console.error('Error fetching data:', error);
    }
}



// 初始加載
fetchData();
