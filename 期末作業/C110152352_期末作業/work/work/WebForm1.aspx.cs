using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace work
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // 頁面首次加載時，不處理查詢
            if (!IsPostBack)
            {
                SqlDataSource2.SelectParameters["供應商"].DefaultValue = string.Empty;
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            // 更新查詢 GridView 的數據
            SqlDataSource2.SelectParameters["供應商"].DefaultValue = txtSearch.Text.Trim();
            GridView2.DataBind(); // 重新綁定查詢結果的資料
        }
    }
}