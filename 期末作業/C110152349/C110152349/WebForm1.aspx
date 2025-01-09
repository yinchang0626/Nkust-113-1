<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="C110152349.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:GridView ID="GridView1" runat="server" AllowSorting="True" AutoGenerateColumns="False" BackColor="White" BorderColor="#3366CC" BorderStyle="None" BorderWidth="1px" CellPadding="4" DataKeyNames="編號" DataSourceID="SqlDataSource1">
                <Columns>
                    <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" />
                    <asp:BoundField DataField="編號" HeaderText="編號" InsertVisible="False" ReadOnly="True" SortExpression="編號" />
                    <asp:BoundField DataField="店名" HeaderText="店名" SortExpression="店名" />
                    <asp:BoundField DataField="聯絡人" HeaderText="聯絡人" SortExpression="聯絡人" />
                    <asp:BoundField DataField="連絡電話" HeaderText="連絡電話" SortExpression="連絡電話" />
                    <asp:BoundField DataField="地址" HeaderText="地址" SortExpression="地址" />
                </Columns>
                <FooterStyle BackColor="#99CCCC" ForeColor="#003399" />
                <HeaderStyle BackColor="#003399" Font-Bold="True" ForeColor="#CCCCFF" />
                <PagerStyle BackColor="#99CCCC" ForeColor="#003399" HorizontalAlign="Left" />
                <RowStyle BackColor="White" ForeColor="#003399" />
                <SelectedRowStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" />
                <SortedAscendingCellStyle BackColor="#EDF6F6" />
                <SortedAscendingHeaderStyle BackColor="#0D4AC4" />
                <SortedDescendingCellStyle BackColor="#D6DFDF" />
                <SortedDescendingHeaderStyle BackColor="#002876" />
            </asp:GridView>
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" DeleteCommand="DELETE FROM [桃園市客家美食認證優質餐廳0502] WHERE [編號] = ?" InsertCommand="INSERT INTO [桃園市客家美食認證優質餐廳0502] ([編號], [店名], [聯絡人], [連絡電話], [地址]) VALUES (?, ?, ?, ?, ?)" ProviderName="<%$ ConnectionStrings:ConnectionString.ProviderName %>" SelectCommand="SELECT * FROM [桃園市客家美食認證優質餐廳0502]" UpdateCommand="UPDATE [桃園市客家美食認證優質餐廳0502] SET [店名] = ?, [聯絡人] = ?, [連絡電話] = ?, [地址] = ? WHERE [編號] = ?">
                <DeleteParameters>
                    <asp:Parameter Name="編號" Type="Int32" />
                </DeleteParameters>
                <InsertParameters>
                    <asp:Parameter Name="編號" Type="Int32" />
                    <asp:Parameter Name="店名" Type="String" />
                    <asp:Parameter Name="聯絡人" Type="String" />
                    <asp:Parameter Name="連絡電話" Type="String" />
                    <asp:Parameter Name="地址" Type="String" />
                </InsertParameters>
                <UpdateParameters>
                    <asp:Parameter Name="店名" Type="String" />
                    <asp:Parameter Name="聯絡人" Type="String" />
                    <asp:Parameter Name="連絡電話" Type="String" />
                    <asp:Parameter Name="地址" Type="String" />
                    <asp:Parameter Name="編號" Type="Int32" />
                </UpdateParameters>
            </asp:SqlDataSource>
        </div>
    </form>
</body>
</html>
