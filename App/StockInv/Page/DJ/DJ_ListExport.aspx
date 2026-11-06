<%@ page language="C#" masterpagefile="~/MasterPagePrint.master" autoeventwireup="true" codefile="DJ_ListExport.aspx.cs" inherits="Page_DJ" %>

<asp:content contentplaceholderid="content" runat="server">
    <div id="form_area" runat="server">
        
        <div class="table-responsive">
            <asp:Table ID="tb" runat="server" CssClass="table-list text-sm">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="90">No. PLU</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">Kode Karet</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80">Item</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80">Stock</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80">Exchange</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80">Resell</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Harga</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">M</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">R</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Net Weight</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Gross Weight</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Butir</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Carat</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Segmen</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Stone Distribution</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>

    </div>
</asp:content>

