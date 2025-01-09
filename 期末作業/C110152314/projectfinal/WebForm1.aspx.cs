using System;
using System.Web.UI.WebControls;

namespace projectfinal
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // 初始化邏輯
            }
        }

        // 更新事件處理
        protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            // 已由 SqlDataSource 處理更新邏輯
        }

        // 刪除事件處理
        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            // 已由 SqlDataSource 處理刪除邏輯
        }

        // 插入新資料
        protected void InsertNewRecord(object sender, EventArgs e)
        {
            SqlDataSource1.InsertParameters["Year"].DefaultValue = "2025"; // 修改為用戶輸入值
            SqlDataSource1.InsertParameters["Balance"].DefaultValue = "100000000";
            SqlDataSource1.InsertParameters["Income"].DefaultValue = "5000000";
            SqlDataSource1.InsertParameters["Yield"].DefaultValue = "5.0";

            SqlDataSource1.Insert();
            GridView1.DataBind();
        }
    }
}
