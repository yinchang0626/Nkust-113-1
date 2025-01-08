document.addEventListener('DOMContentLoaded', function () {
    if (!chartData || !chartData.priceData) {
        console.error('Chart data not properly loaded');
        return;
    }

    const colors = [
        '#0088FE', '#00C49F', '#FFBB28', '#FF8042', '#8884d8', '#82ca9d'
    ];

    let priceChart = null;
    let ratioChart = null;

    const fontSettings = {
        size: 16,
        family: "'Helvetica Neue', 'Helvetica', 'Arial', sans-serif"
    };

    try {
        const products = Array.from(new Set(chartData.priceData.map(d => d.productName)));
        const areas = Array.from(new Set(chartData.priceData.map(d => d.area)));
        const dates = Array.from(new Set(chartData.priceData.map(d => d.date))).sort();

        const elements = {
            trendProductSelect: document.getElementById('trendProductSelect'),
            trendAreaSelect: document.getElementById('trendAreaSelect'),
            trendConfirmBtn: document.getElementById('trendConfirmBtn'),
            ratioAreaSelect: document.getElementById('ratioAreaSelect'),
            ratioDateSelect: document.getElementById('ratioDateSelect'),
            ratioConfirmBtn: document.getElementById('ratioConfirmBtn'),
            priceChart: document.getElementById('priceChart'),
            ratioChart: document.getElementById('ratioChart')
        };

        for (const [key, element] of Object.entries(elements)) {
            if (!element) {
                console.error(`Required element ${key} not found`);
                return;
            }
        }

        function populateSelect(selectElement, options, defaultOption = 'all') {
            selectElement.innerHTML = `<option value="${defaultOption}">${defaultOption === 'all' ? '所有' : defaultOption}</option>`;
            options.forEach(option => {
                const optionElement = new Option(option, option);
                selectElement.appendChild(optionElement);
            });
        }

        function displayNoDataMessage(canvas, message) {
            const ctx = canvas.getContext('2d');
            ctx.clearRect(0, 0, canvas.width, canvas.height);

            ctx.fillStyle = '#666666';
            ctx.font = `${fontSettings.size}px ${fontSettings.family}`;
            ctx.textAlign = 'center';
            ctx.textBaseline = 'middle';

            ctx.fillText(message, canvas.width / 2, canvas.height / 2);
        }

        populateSelect(elements.trendProductSelect, products);
        populateSelect(elements.trendAreaSelect, areas);
        populateSelect(elements.ratioAreaSelect, areas);
        populateSelect(elements.ratioDateSelect, dates);

        function updatePriceChart() {
            const selectedProduct = elements.trendProductSelect.value;
            const selectedArea = elements.trendAreaSelect.value;

            let filteredData = chartData.priceData;

            if (selectedProduct !== 'all') {
                filteredData = filteredData.filter(d => d.productName === selectedProduct);
            }
            if (selectedArea !== 'all') {
                filteredData = filteredData.filter(d => d.area === selectedArea);
            }

            const datasets = products
                .filter(product => selectedProduct === 'all' || product === selectedProduct)
                .map((product, index) => ({
                    label: product,
                    data: filteredData
                        .filter(d => d.productName === product)
                        .map(d => ({
                            x: d.date,
                            y: d.avgPrice
                        })),
                    borderColor: colors[index % colors.length],
                    fill: false
                }))
                .filter(dataset => dataset.data.length > 0);

            if (priceChart) {
                priceChart.destroy();
            }

            if (datasets.length === 0) {
                displayNoDataMessage(elements.priceChart, '查無相關資料');
                return;
            }

            priceChart = new Chart(elements.priceChart, {
                type: 'line',
                data: { datasets },
                options: {
                    responsive: true,
                    scales: {
                        x: {
                            type: 'category',
                            title: {
                                display: true,
                                text: '日期',
                                font: fontSettings
                            },
                            ticks: {
                                font: fontSettings
                            }
                        },
                        y: {
                            beginAtZero: true,
                            title: {
                                display: true,
                                text: '平均價格',
                                font: fontSettings
                            },
                            ticks: {
                                font: fontSettings
                            }
                        }
                    },
                    plugins: {
                        legend: {
                            position: 'bottom',
                            labels: {
                                font: fontSettings,
                                padding: 20
                            }
                        },
                        title: {
                            display: true,
                            text: '價格趨勢圖',
                            font: {
                                size: fontSettings.size + 4,
                                family: fontSettings.family
                            },
                            padding: 20
                        }
                    }
                }
            });
        }

        function updateRatioChart() {
            const selectedArea = elements.ratioAreaSelect.value;
            const selectedDate = elements.ratioDateSelect.value;

            let filteredData = chartData.priceData;

            if (selectedDate !== 'all') {
                filteredData = filteredData.filter(d => d.date === selectedDate);
            }
            if (selectedArea !== 'all') {
                filteredData = filteredData.filter(d => d.area === selectedArea);
            }

            const aggregatedData = products.map(product => ({
                name: product,
                value: filteredData
                    .filter(d => d.productName === product)
                    .reduce((sum, d) => sum + d.num, 0)
            })).filter(d => d.value > 0);

            if (ratioChart) {
                ratioChart.destroy();
            }

            if (aggregatedData.length === 0) {
                displayNoDataMessage(elements.ratioChart, '查無相關資料');
                return;
            }

            ratioChart = new Chart(elements.ratioChart, {
                type: 'pie',
                data: {
                    labels: aggregatedData.map(d => d.name),
                    datasets: [{
                        data: aggregatedData.map(d => d.value),
                        backgroundColor: colors.slice(0, aggregatedData.length)
                    }]
                },
                options: {
                    responsive: true,
                    maintainAspectRatio: true, 
                    aspectRatio: 1.5, 
                    plugins: {
                        legend: {
                            position: 'bottom',
                            labels: {
                                font: fontSettings,
                                padding: 20
                            }
                        },
                        title: {
                            display: true,
                            text: `${selectedArea === 'all' ? '全部地區' : selectedArea} - ${selectedDate === 'all' ? '全部日期' : selectedDate} 羊隻銷售比例`,
                            font: {
                                size: fontSettings.size + 4,
                                family: fontSettings.family
                            },
                            padding: {
                                top: 10,
                                bottom: 20
                            }
                        }
                    }
                }
            });
        }

        elements.trendConfirmBtn.addEventListener('click', updatePriceChart);
        elements.ratioConfirmBtn.addEventListener('click', updateRatioChart);

        updatePriceChart();
        updateRatioChart();

    } catch (error) {
        console.error('Error in chart initialization:', error);
    }
});