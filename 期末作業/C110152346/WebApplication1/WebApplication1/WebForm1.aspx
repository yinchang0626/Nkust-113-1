<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="WebApplication1.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>員工管理</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <!-- 查詢功能區 -->
            <asp:Label ID="LabelSearch" runat="server" Text="查詢姓名：" />
            <asp:TextBox ID="TextBoxSearch" runat="server"></asp:TextBox>
            <asp:Button ID="ButtonSearch" runat="server" Text="查詢" OnClick="ButtonSearch_Click" />
        </div>
        
        <!-- 原始 GridView -->
        <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"
            BackColor="White" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" CellPadding="3"
            DataKeyNames="員工編號" DataSourceID="SqlDataSource1" GridLines="Horizontal">
            <AlternatingRowStyle BackColor="#F7F7F7" />
            <Columns>
                <asp:CommandField ButtonType="Button" ShowDeleteButton="True" ShowEditButton="True" />
                <asp:BoundField DataField="員工編號" HeaderText="員工編號" ReadOnly="True" SortExpression="員工編號" />
                <asp:BoundField DataField="姓名" HeaderText="姓名" SortExpression="姓名" />
                <asp:BoundField DataField="性別" HeaderText="性別" SortExpression="性別" />
                <asp:CheckBoxField DataField="是否已婚" HeaderText="是否已婚" SortExpression="是否已婚" />
                <asp:BoundField DataField="部門編號" HeaderText="部門編號" SortExpression="部門編號" />
                <asp:BoundField DataField="照片" HeaderText="照片" SortExpression="照片" />
            </Columns>
            <FooterStyle BackColor="#B5C7DE" ForeColor="#4A3C8C" />
            <HeaderStyle BackColor="#4A3C8C" Font-Bold="True" ForeColor="#F7F7F7" />
            <PagerStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" HorizontalAlign="Right" />
            <RowStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" />
            <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="#F7F7F7" />
            <SortedAscendingCellStyle BackColor="#F4F4FD" />
            <SortedAscendingHeaderStyle BackColor="#5A4C9D" />
            <SortedDescendingCellStyle BackColor="#D8D8F0" />
            <SortedDescendingHeaderStyle BackColor="#3E3277" />
        </asp:GridView>
        
        <!-- 查詢結果的 GridView -->
        <asp:GridView ID="GridViewQueryResults" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"
            BackColor="White" BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" CellPadding="3"
            DataKeyNames="員工編號" DataSourceID="SqlDataSourceQuery" GridLines="Horizontal" Visible="False">
            <AlternatingRowStyle BackColor="#F7F7F7" />
            <Columns>
                <asp:BoundField DataField="員工編號" HeaderText="員工編號" ReadOnly="True" SortExpression="員工編號" />
                <asp:BoundField DataField="姓名" HeaderText="姓名" SortExpression="姓名" />
                <asp:BoundField DataField="性別" HeaderText="性別" SortExpression="性別" />
                <asp:CheckBoxField DataField="是否已婚" HeaderText="是否已婚" SortExpression="是否已婚" />
                <asp:BoundField DataField="部門編號" HeaderText="部門編號" SortExpression="部門編號" />
                <asp:BoundField DataField="照片" HeaderText="照片" SortExpression="照片" />
            </Columns>
            <FooterStyle BackColor="#B5C7DE" ForeColor="#4A3C8C" />
            <HeaderStyle BackColor="#4A3C8C" Font-Bold="True" ForeColor="#F7F7F7" />
            <PagerStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" HorizontalAlign="Right" />
            <RowStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" />
            <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="#F7F7F7" />
            <SortedAscendingCellStyle BackColor="#F4F4FD" />
            <SortedAscendingHeaderStyle BackColor="#5A4C9D" />
            <SortedDescendingCellStyle BackColor="#D8D8F0" />
            <SortedDescendingHeaderStyle BackColor="#3E3277" />
        </asp:GridView>
        
        <!-- SqlDataSource -->
        <asp:SqlDataSource ID="SqlDataSource1" runat="server"
            ConnectionString="Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\tt266\Desktop\WebApplication1\WebApplication1\chap13.mdf;Integrated Security=True;Connect Timeout=30"
            ProviderName="<%$ ConnectionStrings:chap13ConnectionString.ProviderName %>"
            SelectCommand="SELECT * FROM [員工]"
            DeleteCommand="DELETE FROM [員工] WHERE [員工編號] = @員工編號"
            InsertCommand="INSERT INTO [員工] ([員工編號], [姓名], [性別], [是否已婚], [部門編號], [照片]) VALUES (@員工編號, @姓名, @性別, @是否已婚, @部門編號, @照片)"
            UpdateCommand="UPDATE [員工] SET [姓名] = @姓名, [性別] = @性別, [是否已婚] = @是否已婚, [部門編號] = @部門編號, [照片] = @照片 WHERE [員工編號] = @員工編號">
            <DeleteParameters>
                <asp:Parameter Name="員工編號" Type="String" />
            </DeleteParameters>
            <InsertParameters>
                <asp:Parameter Name="員工編號" Type="String" />
                <asp:Parameter Name="姓名" Type="String" />
                <asp:Parameter Name="性別" Type="String" />
                <asp:Parameter Name="是否已婚" Type="Boolean" />
                <asp:Parameter Name="部門編號" Type="Int32" />
                <asp:Parameter Name="照片" Type="String" />
            </InsertParameters>
            <UpdateParameters>
                <asp:Parameter Name="姓名" Type="String" />
                <asp:Parameter Name="性別" Type="String" />
                <asp:Parameter Name="是否已婚" Type="Boolean" />
                <asp:Parameter Name="部門編號" Type="Int32" />
                <asp:Parameter Name="照片" Type="String" />
                <asp:Parameter Name="員工編號" Type="String" />
            </UpdateParameters>
        </asp:SqlDataSource>

        <asp:SqlDataSource ID="SqlDataSourceQuery" runat="server"
            ConnectionString="<%$ ConnectionStrings:chap13ConnectionString %>"
            ProviderName="<%$ ConnectionStrings:chap13ConnectionString.ProviderName %>"
            SelectCommand="SELECT * FROM [員工] WHERE ([姓名] LIKE '%' + @姓名 + '%')">
            <SelectParameters>
                <asp:ControlParameter Name="姓名" ControlID="TextBoxSearch" PropertyName="Text" Type="String" />
            </SelectParameters>
        </asp:SqlDataSource>
    </form>
</body>
</html>
