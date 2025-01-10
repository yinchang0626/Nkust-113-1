function initializeChart(labels, data) {
    if (ageGroupChart) {
        ageGroupChart.destroy(); // 如果圖表已存在，先銷毀
    }

    ageGroupChart = new Chart(ctx, {
        type: 'pie',
        data: {
            labels: labels,
            datasets: [{
                data: data,
                backgroundColor: [
                    '#FF6384', '#36A2EB', '#FFCE56', '#66BB6A', '#FFA726',
                    '#AB47BC', '#29B6F6', '#EF5350', '#8D6E63', '#D4E157', '#FF7043'
                ],
                borderWidth: 1
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                legend: {
                    position: 'right',
                },
                title: {
                    display: true,
                    text: '年齡區間人口分布圖'
                },
                datalabels: {
                    formatter: (value, context) => {
                        const total = context.chart.data.datasets[0].data.reduce((sum, val) => sum + val, 0);
                        const percentage = ((value / total) * 100).toFixed(2);

                        // 只顯示大於 3% 的標籤
                        return percentage >= 3 ? `${percentage}%` : '';
                    },
                    color: '#000',
                    font: {
                        size: 12,
                        weight: 'bold'
                    },
                    anchor: 'end',
                    align: 'end',
                    offset: 10,
                    clip: false,
                    // 使用引線避免重疊
                    labels: {
                        connector: {
                            length: 20,
                            length2: 10,
                            lineWidth: 1,
                            color: '#666'
                        }
                    }
                }
            }
        },
        plugins: [ChartDataLabels]
    });
}
