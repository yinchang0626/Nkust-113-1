using System;

namespace WebApplication1
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // 頁面初次載入時顯示所有資料
                SqlDataSource2.SelectCommand = "SELECT * FROM [tourism]";
                GridView2.DataBind();
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            // 根據使用者輸入更新資料來源參數
            string searchKeyword = txtSearch.Text.Trim();
            SqlDataSource2.SelectCommand = "SELECT * FROM [tourism] WHERE 月份 LIKE '%" + searchKeyword + "%'";
            GridView2.DataBind();
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            // 重置搜尋條件，恢復顯示所有資料
            txtSearch.Text = string.Empty;
            SqlDataSource2.SelectCommand = "SELECT * FROM [tourism]";
            GridView2.DataBind();
        }
    }
}
