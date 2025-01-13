const ctx = document.getElementById('trendChart').getContext('2d');

let trendChart;

// 初始化折線圖
function initializeTrendChart(labels, datasets) {
    if (trendChart) {
        trendChart.destroy(); // 如果圖表已存在，先銷毀
    }

    trendChart = new Chart(ctx, {
        type: 'line',
        data: {
            labels: labels,
            datasets: datasets
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                legend: {
                    position: 'top',
                },
                title: {
                    display: true,
                    text: '歷月人口趨勢'
                }
            },
            scales: {
                x: {
                    title: {
                        display: true,
                        text: '月份'
                    }
                },
                y: {
                    title: {
                        display: true,
                        text: '人口數'
                    }
                }
            }
        }
    });
}

// 請求 API 資料並初始化折線圖
function fetchTrendData(regions) {
    const params = new URLSearchParams();
    regions.forEach(region => params.append('region', region));

    fetch(`/api/population/trend?${params.toString()}`)
        .then(response => response.json())
        .then(data => {
            const labels = [];
            const datasets = [];

            Object.keys(data).forEach(region => {
                const regionData = data[region];
                const sortedMonths = Object.keys(regionData).sort(); // 按月份排序

                if (labels.length === 0) {
                    labels.push(...sortedMonths);
                }

                const populationValues = sortedMonths.map(month => regionData[month]);
                datasets.push({
                    label: region,
                    data: populationValues,
                    fill: false,
                    borderColor: getRandomColor(),
                    tension: 0.1
                });
            });

            initializeTrendChart(labels, datasets);
        })
        .catch(error => console.error('Error:', error));
}

// 隨機生成顏色
function getRandomColor() {
    const letters = '0123456789ABCDEF';
    let color = '#';
    for (let i = 0; i < 6; i++) {
        color += letters[Math.floor(Math.random() * 16)];
    }
    return color;
}

// 當選擇地區時更新圖表
document.querySelectorAll('input[name="region"]').forEach(checkbox => {
    checkbox.addEventListener('change', function () {
        const selectedRegions = Array.from(document.querySelectorAll('input[name="region"]:checked'))
            .map(checkbox => checkbox.value);
        fetchTrendData(selectedRegions);
    });
});

// 初始載入
fetchTrendData(['臺北市']);
