<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="WebApplication2.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:chap13ConnectionString %>" DeleteCommand="DELETE FROM [部門] WHERE [部門編號] = @部門編號" InsertCommand="INSERT INTO [部門] ([部門名稱]) VALUES (@部門名稱)" SelectCommand="SELECT * FROM [部門]" UpdateCommand="UPDATE [部門] SET [部門名稱] = @部門名稱 WHERE [部門編號] = @部門編號">
                <DeleteParameters>
                    <asp:Parameter Name="部門編號" Type="Int32" />
                </DeleteParameters>
                <InsertParameters>
                    <asp:Parameter Name="部門名稱" Type="String" />
                </InsertParameters>
                <UpdateParameters>
                    <asp:Parameter Name="部門名稱" Type="String" />
                    <asp:Parameter Name="部門編號" Type="Int32" />
                </UpdateParameters>
            </asp:SqlDataSource>
            <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConflictDetection="CompareAllValues" ConnectionString="<%$ ConnectionStrings:chap13ConnectionString %>" DeleteCommand="DELETE FROM [員工] WHERE [員工編號] = @original_員工編號 AND (([姓名] = @original_姓名) OR ([姓名] IS NULL AND @original_姓名 IS NULL)) AND (([性別] = @original_性別) OR ([性別] IS NULL AND @original_性別 IS NULL)) AND (([是否已婚] = @original_是否已婚) OR ([是否已婚] IS NULL AND @original_是否已婚 IS NULL)) AND (([部門編號] = @original_部門編號) OR ([部門編號] IS NULL AND @original_部門編號 IS NULL)) AND (([照片] = @original_照片) OR ([照片] IS NULL AND @original_照片 IS NULL))" InsertCommand="INSERT INTO [員工] ([員工編號], [姓名], [性別], [是否已婚], [部門編號], [照片]) VALUES (@員工編號, @姓名, @性別, @是否已婚, @部門編號, @照片)" OldValuesParameterFormatString="original_{0}" SelectCommand="SELECT * FROM [員工] WHERE ([部門編號] = @部門編號)" UpdateCommand="UPDATE [員工] SET [姓名] = @姓名, [性別] = @性別, [是否已婚] = @是否已婚, [部門編號] = @部門編號, [照片] = @照片 WHERE [員工編號] = @original_員工編號 AND (([姓名] = @original_姓名) OR ([姓名] IS NULL AND @original_姓名 IS NULL)) AND (([性別] = @original_性別) OR ([性別] IS NULL AND @original_性別 IS NULL)) AND (([是否已婚] = @original_是否已婚) OR ([是否已婚] IS NULL AND @original_是否已婚 IS NULL)) AND (([部門編號] = @original_部門編號) OR ([部門編號] IS NULL AND @original_部門編號 IS NULL)) AND (([照片] = @original_照片) OR ([照片] IS NULL AND @original_照片 IS NULL))">
                <DeleteParameters>
                    <asp:Parameter Name="original_員工編號" Type="String" />
                    <asp:Parameter Name="original_姓名" Type="String" />
                    <asp:Parameter Name="original_性別" Type="String" />
                    <asp:Parameter Name="original_是否已婚" Type="Boolean" />
                    <asp:Parameter Name="original_部門編號" Type="Int32" />
                    <asp:Parameter Name="original_照片" Type="String" />
                </DeleteParameters>
                <InsertParameters>
                    <asp:Parameter Name="員工編號" Type="String" />
                    <asp:Parameter Name="姓名" Type="String" />
                    <asp:Parameter Name="性別" Type="String" />
                    <asp:Parameter Name="是否已婚" Type="Boolean" />
                    <asp:Parameter Name="部門編號" Type="Int32" />
                    <asp:Parameter Name="照片" Type="String" />
                </InsertParameters>
                <SelectParameters>
                    <asp:ControlParameter ControlID="ListBox1" DefaultValue="1" Name="部門編號" PropertyName="SelectedValue" Type="Int32" />
                </SelectParameters>
                <UpdateParameters>
                    <asp:Parameter Name="姓名" Type="String" />
                    <asp:Parameter Name="性別" Type="String" />
                    <asp:Parameter Name="是否已婚" Type="Boolean" />
                    <asp:Parameter Name="部門編號" Type="Int32" />
                    <asp:Parameter Name="照片" Type="String" />
                    <asp:Parameter Name="original_員工編號" Type="String" />
                    <asp:Parameter Name="original_姓名" Type="String" />
                    <asp:Parameter Name="original_性別" Type="String" />
                    <asp:Parameter Name="original_是否已婚" Type="Boolean" />
                    <asp:Parameter Name="original_部門編號" Type="Int32" />
                    <asp:Parameter Name="original_照片" Type="String" />
                </UpdateParameters>
            </asp:SqlDataSource>
            <asp:ListBox ID="ListBox1" runat="server" AutoPostBack="True" DataSourceID="SqlDataSource1" DataTextField="部門名稱" DataValueField="部門編號"></asp:ListBox>
            <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" BackColor="White" BorderColor="#CC9966" BorderStyle="None" BorderWidth="1px" CellPadding="4" DataKeyNames="員工編號" DataSourceID="SqlDataSource2">
                <Columns>
                    <asp:CommandField ShowDeleteButton="True" ShowEditButton="True" ShowSelectButton="True" />
                    <asp:BoundField DataField="員工編號" HeaderText="員工編號" ReadOnly="True" SortExpression="員工編號" />
                    <asp:BoundField DataField="姓名" HeaderText="姓名" SortExpression="姓名" />
                    <asp:BoundField DataField="性別" HeaderText="性別" SortExpression="性別" />
                    <asp:CheckBoxField DataField="是否已婚" HeaderText="是否已婚" SortExpression="是否已婚" />
                    <asp:BoundField DataField="部門編號" HeaderText="部門編號" SortExpression="部門編號" />
                    <asp:BoundField DataField="照片" HeaderText="照片" SortExpression="照片" />
                </Columns>
                <FooterStyle BackColor="#FFFFCC" ForeColor="#330099" />
                <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="#FFFFCC" />
                <PagerStyle BackColor="#FFFFCC" ForeColor="#330099" HorizontalAlign="Center" />
                <RowStyle BackColor="White" ForeColor="#330099" />
                <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="#663399" />
                <SortedAscendingCellStyle BackColor="#FEFCEB" />
                <SortedAscendingHeaderStyle BackColor="#AF0101" />
                <SortedDescendingCellStyle BackColor="#F6F0C0" />
                <SortedDescendingHeaderStyle BackColor="#7E0000" />
            </asp:GridView>
        </div>
    </form>
</body>
</html>
