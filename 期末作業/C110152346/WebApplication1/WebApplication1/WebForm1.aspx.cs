using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void ButtonSearch_Click(object sender, EventArgs e)
        {
            // 如果輸入框有值，顯示查詢結果 GridView
            if (!string.IsNullOrEmpty(TextBoxSearch.Text))
            {
                GridViewQueryResults.Visible = true;
                GridViewQueryResults.DataBind();
            }
            else
            {
                GridViewQueryResults.Visible = false;
            }
        }
    }
}