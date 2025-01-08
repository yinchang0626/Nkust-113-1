<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="WebApplication1.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>111年7月澎湖縣觀光人數統計總表</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <!-- 查詢功能 -->
            <asp:Label ID="lblSearch" runat="server" Text="搜尋月份：" Font-Bold="True"></asp:Label>
            <asp:TextBox ID="txtSearch" runat="server"></asp:TextBox>
            <asp:Button ID="btnSearch" runat="server" Text="查詢" OnClick="btnSearch_Click" />
            <asp:Button ID="btnReset" runat="server" Text="重置" OnClick="btnReset_Click" />
        </div>
        <br />
        <!-- SqlDataSource2 資料來源 -->
        <asp:SqlDataSource ID="SqlDataSource2" runat="server" 
            ConnectionString="<%$ ConnectionStrings:nightmarketConnectionString %>" 
            SelectCommand="SELECT * FROM [tourism]">
        </asp:SqlDataSource>

        <!-- GridView2 表格 -->
        <asp:GridView ID="GridView2" runat="server" AllowPaging="True" AllowSorting="True" 
            AutoGenerateColumns="False" DataSourceID="SqlDataSource2" CellPadding="4" ForeColor="#333333" GridLines="None">
            <AlternatingRowStyle BackColor="White" />
            <Columns>
                <asp:BoundField DataField="月份" HeaderText="月份" SortExpression="月份" />
                <asp:BoundField DataField="年分-110年：航空人次" HeaderText="年分-110年：航空人次" SortExpression="年分-110年：航空人次" />
                <asp:BoundField DataField="年分-110年：輪船人次" HeaderText="年分-110年：輪船人次" SortExpression="年分-110年：輪船人次" />
                <asp:BoundField DataField="年分-110年：觀光客人次" HeaderText="年分-110年：觀光客人次" SortExpression="年分-110年：觀光客人次" />
                <asp:BoundField DataField="年分-111年：航空人次" HeaderText="年分-111年：航空人次" SortExpression="年分-111年：航空人次" />
                <asp:BoundField DataField="年分-111年：輪船人次" HeaderText="年分-111年：輪船人次" SortExpression="年分-111年：輪船人次" />
                <asp:BoundField DataField="年分-111年：觀光客人次" HeaderText="年分-111年：觀光客人次" SortExpression="年分-111年：觀光客人次" />
                <asp:BoundField DataField="年分-111年：觀光客增減人數" HeaderText="年分-111年：觀光客增減人數" SortExpression="年分-111年：觀光客增減人數" />
            </Columns>
            <EditRowStyle BackColor="#7C6F57" />
            <FooterStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#1C5E55" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#666666" ForeColor="White" HorizontalAlign="Center" />
            <RowStyle BackColor="#E3EAEB" />
            <SelectedRowStyle BackColor="#C5BBAF" Font-Bold="True" ForeColor="#333333" />
            <SortedAscendingCellStyle BackColor="#F8FAFA" />
            <SortedAscendingHeaderStyle BackColor="#246B61" />
            <SortedDescendingCellStyle BackColor="#D4DFE1" />
            <SortedDescendingHeaderStyle BackColor="#15524A" />
        </asp:GridView>
    </form>
</body>
</html>
