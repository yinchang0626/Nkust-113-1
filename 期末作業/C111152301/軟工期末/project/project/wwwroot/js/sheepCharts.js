document.addEventListener('DOMContentLoaded', function () {
    console.log('Chart initialization started');
    console.log('Initial chart data:', chartData);

    // 設置圖表顏色
    const colors = [
        '#0088FE', '#00C49F', '#FFBB28', '#FF8042', '#8884d8', '#82ca9d'
    ];

    try {
        // 初始化選擇器
        const products = [...new Set(chartData.priceData.map(d => d.productName))];
        const areas = [...new Set(chartData.priceData.map(d => d.area))];
        const dates = [...new Set(chartData.priceData.map(d => d.date))].sort();

        console.log('Unique products:', products);
        console.log('Unique areas:', areas);
        console.log('Unique dates:', dates);

        // 獲取DOM元素
        const trendProductSelect = document.getElementById('trendProductSelect');
        const trendAreaSelect = document.getElementById('trendAreaSelect');
        const trendConfirmBtn = document.getElementById('trendConfirmBtn');
        const productDateSelect = document.getElementById('productDateSelect');
        const productConfirmBtn = document.getElementById('productConfirmBtn');
        const areaDateSelect = document.getElementById('areaDateSelect');
        const areaConfirmBtn = document.getElementById('areaConfirmBtn');

        // 填充選擇器
        products.forEach(product => {
            const option = document.createElement('option');
            option.value = product;
            option.textContent = product;
            trendProductSelect.appendChild(option.cloneNode(true));
        });

        areas.forEach(area => {
            const option = document.createElement('option');
            option.value = area;
            option.textContent = area;
            trendAreaSelect.appendChild(option.cloneNode(true));
        });

        dates.forEach(date => {
            const option = document.createElement('option');
            option.value = date;
            option.textContent = date;
            productDateSelect.appendChild(option.cloneNode(true));
            areaDateSelect.appendChild(option.cloneNode(true));
        });

        // 更新價格趨勢圖
        function updatePriceChart() {
            console.log('Updating price chart...');
            const selectedProduct = trendProductSelect.value;
            const selectedArea = trendAreaSelect.value;

            console.log('Selected product:', selectedProduct);
            console.log('Selected area:', selectedArea);

            let filteredData = chartData.priceData;
            if (selectedProduct !== 'all') {
                filteredData = filteredData.filter(d => d.productName === selectedProduct);
            }
            if (selectedArea !== 'all') {
                filteredData = filteredData.filter(d => d.area === selectedArea);
            }

            const datasets = products
                .filter(product => selectedProduct === 'all' || product === selectedProduct)
                .map((product, index) => {
                    const productData = filteredData.filter(d => d.productName === product);
                    return {
                        label: product,
                        data: productData.map(d => ({
                            x: d.date,
                            y: d.avgPrice
                        })),
                        borderColor: colors[index % colors.length],
                        fill: false
                    };
                })
                .filter(dataset => dataset.data.length > 0);

            console.log('Prepared datasets:', datasets);

            const ctx = document.getElementById('priceChart');
            if (window.priceChart) {
                try {
                    window.priceChart.destroy();
                } catch (error) {
                    console.log(error)
                }
            }

            window.priceChart = new Chart(ctx, {
                type: 'line',
                data: {
                    datasets: datasets
                },
                options: {
                    responsive: true,
                    scales: {
                        x: {
                            type: 'category',
                            title: {
                                display: true,
                                text: '日期'
                            }
                        },
                        y: {
                            beginAtZero: true,
                            title: {
                                display: true,
                                text: '平均價格'
                            }
                        }
                    },
                    plugins: {
                        legend: {
                            position: 'bottom'
                        }
                    }
                }
            });
        }

        // 更新產品比例圖
        function updateProductChart() {
            console.log('Updating product chart...');
            const selectedDate = productDateSelect.value;
            console.log('Selected date:', selectedDate);

            let filteredData;
            if (selectedDate === 'all') {
                filteredData = chartData.productRatioData;
            } else {
                filteredData = chartData.priceData
                    .filter(d => d.date === selectedDate)
                    .reduce((acc, curr) => {
                        const existing = acc.find(item => item.name === curr.productName);
                        if (existing) {
                            existing.value += curr.num;
                        } else {
                            acc.push({ name: curr.productName, value: curr.num });
                        }
                        return acc;
                    }, []);
            }

            console.log('Filtered product data:', filteredData);

            const ctx = document.getElementById('productChart');
            if (window.productChart) {
                try {
                    window.productChart.destroy();
                } catch(error) {
                    console.log(error)
                }
                
            }

            window.productChart = new Chart(ctx, {
                type: 'pie',
                data: {
                    labels: filteredData.map(d => d.name),
                    datasets: [{
                        data: filteredData.map(d => d.value),
                        backgroundColor: colors
                    }]
                },
                options: {
                    responsive: true,
                    plugins: {
                        legend: {
                            position: 'bottom'
                        }
                    }
                }
            });
        }

        // 更新地區比例圖
        function updateAreaChart() {
            console.log('Updating area chart...');
            const selectedDate = areaDateSelect.value;
            console.log('Selected date:', selectedDate);

            let filteredData;
            if (selectedDate === 'all') {
                filteredData = chartData.areaRatioData;
            } else {
                filteredData = chartData.priceData
                    .filter(d => d.date === selectedDate)
                    .reduce((acc, curr) => {
                        const existing = acc.find(item => item.area === curr.area);
                        if (existing) {
                            existing.value += curr.num;
                        } else {
                            acc.push({ area: curr.area, value: curr.num });
                        }
                        return acc;
                    }, []);
            }

            console.log('Filtered area data:', filteredData);

            const ctx = document.getElementById('areaChart');
            if (window.areaChart) {
                try {
                    window.areaChart.destroy();
                } catch (error) {
                    console.log(error)
                }
            }

            window.areaChart = new Chart(ctx, {
                type: 'pie',
                data: {
                    labels: filteredData.map(d => d.area),
                    datasets: [{
                        data: filteredData.map(d => d.value),
                        backgroundColor: colors
                    }]
                },
                options: {
                    responsive: true,
                    plugins: {
                        legend: {
                            position: 'bottom'
                        }
                    }
                }
            });
        }

        // 綁定確認按鈕事件
        trendConfirmBtn.addEventListener('click', updatePriceChart);
        productConfirmBtn.addEventListener('click', updateProductChart);
        areaConfirmBtn.addEventListener('click', updateAreaChart);

        console.log('Chart initialization completed');
    } catch (error) {
        console.error('Error initializing charts:', error);
    }
});