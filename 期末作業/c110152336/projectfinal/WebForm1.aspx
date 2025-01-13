<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="projectfinal.WebForm1" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>資金管理系統</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:SqlDataSource 
                ID="SqlDataSource1" 
                runat="server" 
                ConnectionString="<%$ ConnectionStrings:finalConnectionString %>"
                SelectCommand="SELECT * FROM cash"
                UpdateCommand="UPDATE cash 
                                SET 資金運用餘額（金額） = @Balance, 
                                    基金收益（金額） = @Income, 
                                    Yield = @Yield 
                                WHERE year = @Year"
                DeleteCommand="DELETE FROM cash WHERE year = @Year"
                InsertCommand="INSERT INTO cash (year, 資金運用餘額（金額）, 基金收益（金額）, Yield) 
                                VALUES (@Year, @Balance, @Income, @Yield)">
                <UpdateParameters>
                    <asp:Parameter Name="Year" Type="Int32" />
                    <asp:Parameter Name="Balance" Type="Double" />
                    <asp:Parameter Name="Income" Type="Double" />
                    <asp:Parameter Name="Yield" Type="Double" />
                </UpdateParameters>
                <DeleteParameters>
                    <asp:Parameter Name="Year" Type="Int32" />
                </DeleteParameters>
                <InsertParameters>
                    <asp:Parameter Name="Year" Type="Int32" />
                    <asp:Parameter Name="Balance" Type="Double" />
                    <asp:Parameter Name="Income" Type="Double" />
                    <asp:Parameter Name="Yield" Type="Double" />
                </InsertParameters>
            </asp:SqlDataSource>

            <asp:GridView 
                ID="GridView1" 
                runat="server" 
                DataSourceID="SqlDataSource1" 
                AutoGenerateColumns="False" 
                AllowPaging="True" 
                AllowSorting="True" 
                DataKeyNames="year">
                <Columns>
                    <asp:BoundField DataField="year" HeaderText="Year" ReadOnly="True" />
                    <asp:BoundField DataField="Fund utilization balance (amount)" HeaderText="Balance" />
                    <asp:BoundField DataField="Fund income (amount)" HeaderText="Income" />
                    <asp:BoundField DataField="Yield" HeaderText="Yield" />
                    <asp:CommandField ShowEditButton="True" ShowDeleteButton="True" />
                </Columns>
            </asp:GridView>
        </div>
    </form>
</body>
</html>
