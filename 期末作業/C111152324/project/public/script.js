document.addEventListener('DOMContentLoaded', () => {
    const dataTableBody = document.querySelector('#data-table tbody');
    const sortSelect = document.querySelector('#sort-select');
    const filterSelect = document.querySelector('#filter-select');
    const chartContainer = document.querySelector('.chart-wrapper');
    const chartTypeSelect = document.createElement('select');
    const chartButton = document.createElement('button');
    const toggleChartButton = document.createElement('button');
    const addFormContainer = document.createElement('div');
    let chartInstance;

    // 添加圖表類型選單
    chartTypeSelect.id = 'chart-type-select';
    chartTypeSelect.innerHTML = `
        <option value="">選擇圖表類型</option>
        <option value="pie">圓餅圖</option>
        <option value="bar">柱狀圖</option>
        <option value="line">折線圖</option>
    `;

    // 添加生成圖表按鈕
    chartButton.textContent = '生成圖表';
    chartButton.id = 'generate-chart-button';

    // 添加隱藏/顯示圖表按鈕
    toggleChartButton.textContent = '隱藏圖表';
    toggleChartButton.id = 'toggle-chart-button';

    // 插入到控制區域
    const controls = document.querySelector('.controls');
    controls.appendChild(chartTypeSelect);
    controls.appendChild(chartButton);
    controls.appendChild(toggleChartButton);

    // 添加新增資料表單
    addFormContainer.innerHTML = `
        <h3>新增資料</h3>
        <form id="add-form">
            <input type="text" name="case_code" placeholder="ID" required>
            <input type="text" name="market_name" placeholder="名稱" required>
            <input type="text" name="addr" placeholder="地址" required>
            <input type="text" name="business_hours" placeholder="開始時間" required>
            <input type="text" name="business_hurs_end" placeholder="結束時間" required>
            <button type="submit">新增</button>
        </form>
    `;
    controls.appendChild(addFormContainer);

    // 固定圖表位置
    chartContainer.style.display = 'none'; // 初始隱藏

    const regionOrder = {
        "北": ["台北市", "新北市", "基隆市", "桃園市", "新竹市", "新竹縣"],
        "中": ["台中市", "苗栗縣", "彰化縣", "南投縣"],
        "南": ["台南市", "高雄市", "屏東縣", "嘉義市", "嘉義縣"],
        "東": ["宜蘭縣", "花蓮縣", "台東縣"],
    };

    const generateChart = (type, data) => {
        if (chartInstance) {
            chartInstance.destroy();
        }
        const ctx = document.getElementById('chart').getContext('2d');
        chartInstance = new Chart(ctx, {
            type: type,
            data: {
                labels: Object.keys(data),
                datasets: [{
                    label: '縣市分布',
                    data: Object.values(data),
                    backgroundColor: ['#FF6384', '#36A2EB', '#FFCE56', '#4BC0C0', '#9966FF', '#FF9F40'],
                }],
            },
        });

        // 顯示圖表
        chartContainer.style.display = 'block';
        toggleChartButton.textContent = '隱藏圖表';
    };
    function openEditModal(id) {
        // 假設已經有資料從表格中讀取
        const row = document.querySelector(`[data-id="${id}"]`).closest('tr');
        const market_name = row.children[1].textContent;
        const addr = row.children[2].textContent;
        const business_hours = row.children[3].textContent.split(' - ')[0];
        const business_hurs_end = row.children[3].textContent.split(' - ')[1];
    
        // 顯示修改表單
        const editForm = `
            <form id="edit-form">
                <input type="hidden" name="case_code" value="${id}">
                <input type="text" name="market_name" value="${market_name}" required>
                <input type="text" name="addr" value="${addr}" required>
                <input type="text" name="business_hours" value="${business_hours}" required>
                <input type="text" name="business_hurs_end" value="${business_hurs_end}" required>
                <button type="submit">提交修改</button>
            </form>
        `;
        document.body.innerHTML += editForm;
    
        document.querySelector('#edit-form').addEventListener('submit', (e) => {
            e.preventDefault();
            const formData = new FormData(e.target);
            fetch('/edit', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(Object.fromEntries(formData.entries())),
            })
                .then(response => {
                    if (!response.ok) throw new Error('修改失敗');
                    return response.text();
                })
                .then(message => {
                    alert(message);
                    loadData('/data'); // 重新載入數據
                })
                .catch(err => console.error('修改資料失敗:', err));
        });
    }
    
    const loadChartData = (data) => {
        const cityCounts = {};
        data.forEach(store => {
            const city = regionOrder["北"].concat(regionOrder["中"], regionOrder["南"], regionOrder["東"]).find(city => store.addr.includes(city));
            if (city) {
                cityCounts[city] = (cityCounts[city] || 0) + 1;
            }
        });

        const selectedType = chartTypeSelect.value;
        if (selectedType) {
            generateChart(selectedType, cityCounts);
        } else {
            alert('請選擇圖表類型');
        }
    };

    const loadData = (url) => {
        fetch(url)
            .then(response => {
                if (!response.ok) throw new Error('資料加載失敗');
                return response.json();
            })
            .then(data => {
                const selectedRegion = filterSelect.value;
                if (selectedRegion) {
                    data = data.filter(store =>
                        regionOrder[selectedRegion]?.some(city => store.addr.includes(city))
                    );
                }

                const sortOption = sortSelect.value;
                if (sortOption === 'asc') {
                    data.sort((a, b) => {
                        const openingA = a.business_hours.split(':').map(Number);
                        const openingB = b.business_hours.split(':').map(Number);
                        return openingA[0] - openingB[0] || openingA[1] - openingB[1];
                    });
                } else if (sortOption === 'desc') {
                    data.sort((a, b) => {
                        const closingA = a.business_hurs_end.split(':').map(Number);
                        const closingB = b.business_hurs_end.split(':').map(Number);
                        return closingB[0] - closingA[0] || closingB[1] - closingA[1];
                    });
                }

                dataTableBody.innerHTML = '';
                if (data.length === 0) {
                    const row = document.createElement('tr');
                    row.innerHTML = '<td colspan="5" style="text-align: center;">查無資料</td>';
                    dataTableBody.appendChild(row);
                } else {
                    data.forEach(store => {
                        const row = document.createElement('tr');
                        row.innerHTML = `
                            <td>${store.case_code}</td>
                            <td>${store.market_name}</td>
                            <td>${store.addr}</td>
                            <td>${store.business_hours} - ${store.business_hurs_end}</td>
                            <td>
                                <button class="edit-button" data-id="${store.case_code}">修改</button>
                                <button class="delete-button" data-id="${store.case_code}">刪除</button>
                            </td>
                        `;
                        dataTableBody.appendChild(row);
                    });

                    document.querySelectorAll('.edit-button').forEach(button => {
                        button.addEventListener('click', (e) => {
                            const id = e.target.dataset.id;
                            openEditModal(id);
                        });
                    });

                    document.querySelectorAll('.delete-button').forEach(button => {
                        button.addEventListener('click', (e) => {
                            const id = e.target.dataset.id;
                            if (confirm('確定要刪除此資料嗎？')) {
                                fetch('/delete', {
                                    method: 'POST',
                                    headers: { 'Content-Type': 'application/json' },
                                    body: JSON.stringify({ case_code: id }),
                                })
                                    .then(response => {
                                        if (!response.ok) throw new Error('刪除資料失敗');
                                        return response.text();
                                    })
                                    .then(message => {
                                        alert(message);
                                        loadData('/data');
                                    })
                                    .catch(err => console.error('刪除資料失敗:', err));
                            }
                        });
                    });
                }
            })
            .catch(err => console.error('資料加載失敗：', err));
    };

    chartButton.addEventListener('click', () => {
        fetch('/data')
            .then(response => {
                if (!response.ok) throw new Error('生成圖表失敗');
                return response.json();
            })
            .then(data => loadChartData(data))
            .catch(err => console.error(err.message));
    });

    toggleChartButton.addEventListener('click', () => {
        if (chartContainer.style.display === 'none') {
            chartContainer.style.display = 'block';
            toggleChartButton.textContent = '隱藏圖表';
        } else {
            chartContainer.style.display = 'none';
            toggleChartButton.textContent = '顯示圖表';
        }
    });
    document.getElementById('search-button').addEventListener('click', () => {
        const keyword = document.getElementById('search-input').value;
        if (keyword.trim() === '') {
            alert('請輸入搜尋關鍵字');
            return;
        }
        fetch(`/search?keyword=${encodeURIComponent(keyword)}`)
            .then(response => {
                if (!response.ok) throw new Error('搜尋失敗');
                return response.json();
            })
            .then(data => {
                loadDataToTable(data); // 顯示搜尋結果
            })
            .catch(err => console.error('搜尋失敗:', err));
    });
    
    function loadDataToTable(data) {
        const dataTableBody = document.querySelector('#data-table tbody');
        dataTableBody.innerHTML = '';
        if (data.length === 0) {
            dataTableBody.innerHTML = '<tr><td colspan="5" style="text-align:center;">查無資料</td></tr>';
        } else {
            data.forEach(item => {
                const row = document.createElement('tr');
                row.innerHTML = `
                    <td>${item.case_code}</td>
                    <td>${item.market_name}</td>
                    <td>${item.addr}</td>
                    <td>${item.business_hours} - ${item.business_hurs_end}</td>
                    <td>
                        <button class="edit-button" data-id="${item.case_code}">修改</button>
                        <button class="delete-button" data-id="${item.case_code}">刪除</button>
                    </td>
                `;
                dataTableBody.appendChild(row);
            });
        }
    }
    
    document.querySelector('#add-form').addEventListener('submit', (e) => {
        e.preventDefault();
        const formData = new FormData(e.target);
        fetch('/add', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(Object.fromEntries(formData.entries())),
        })
            .then(response => response.text())
            .then(message => {
                alert(message);
                loadData('/data');
            })
            .catch(err => console.error('新增資料失敗:', err));
    });

    loadData('/data');

    sortSelect.addEventListener('change', () => {
        loadData('/data');
    });

    filterSelect.addEventListener('change', () => {
        loadData('/data');
    });
});