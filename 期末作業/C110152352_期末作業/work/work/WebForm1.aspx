<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="work.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>供應商管理</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <!-- 查詢功能 -->
            <asp:Label ID="Label1" runat="server" Text="查詢供應商: "></asp:Label>
            <asp:TextBox ID="txtSearch" runat="server"></asp:TextBox>
            <asp:Button ID="btnSearch" runat="server" Text="查詢" OnClick="btnSearch_Click" />
        </div>
        <br />
        <!-- 原始的資料列表 -->
        <asp:GridView ID="GridView1" runat="server" 
            AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" 
            DataKeyNames="供應商編號" DataSourceID="SqlDataSource1">
            <Columns>
                <asp:CommandField ButtonType="Button" ShowDeleteButton="True" ShowEditButton="True" />
                <asp:BoundField DataField="供應商編號" HeaderText="供應商編號" InsertVisible="False" ReadOnly="True" SortExpression="供應商編號" />
                <asp:BoundField DataField="供應商" HeaderText="供應商" SortExpression="供應商" />
                <asp:BoundField DataField="連絡人" HeaderText="連絡人" SortExpression="連絡人" />
                <asp:BoundField DataField="連絡人職稱" HeaderText="連絡人職稱" SortExpression="連絡人職稱" />
                <asp:BoundField DataField="地址" HeaderText="地址" SortExpression="地址" />
                <asp:BoundField DataField="城市" HeaderText="城市" SortExpression="城市" />
                <asp:BoundField DataField="行政區" HeaderText="行政區" SortExpression="行政區" />
                <asp:BoundField DataField="郵遞區號" HeaderText="郵遞區號" SortExpression="郵遞區號" />
                <asp:BoundField DataField="國家地區" HeaderText="國家地區" SortExpression="國家地區" />
                <asp:BoundField DataField="電話" HeaderText="電話" SortExpression="電話" />
                <asp:BoundField DataField="傳真電話" HeaderText="傳真電話" SortExpression="傳真電話" />
                <asp:BoundField DataField="首頁" HeaderText="首頁" SortExpression="首頁" />
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
            ConnectionString="<%$ ConnectionStrings:NorthwindConnectionString %>" 
            SelectCommand="SELECT * FROM [供應商]">
        </asp:SqlDataSource>
        <br />

        <!-- 查詢結果的 GridView -->
        <asp:GridView ID="GridView2" runat="server" 
            AllowPaging="True" AllowSorting="True" AutoGenerateColumns="True" 
            DataSourceID="SqlDataSource2">
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSource2" runat="server" 
            ConnectionString="<%$ ConnectionStrings:NorthwindConnectionString %>" 
            SelectCommand="SELECT * FROM [供應商] WHERE ([供應商] LIKE '%' + @供應商 + '%')">
            <SelectParameters>
                <asp:Parameter Name="供應商" Type="String" />
            </SelectParameters>
        </asp:SqlDataSource>
    </form>
</body>
</html>
