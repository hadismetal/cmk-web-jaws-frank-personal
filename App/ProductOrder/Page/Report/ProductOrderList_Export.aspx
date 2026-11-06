<%@ Page Language="C#" masterpagefile="~/MasterPageExport.master" AutoEventWireup="true" CodeFile="ProductOrderList_Export.aspx.cs" Inherits="Page_Summary" %>

<asp:content contentplaceholderid="content" runat="server">
    <div id="form" runat="server">
        <div class="table-responsive">
            <asp:Table ID="tb" runat="server">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="50">No.</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">No. Referensi</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">Operator</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">Designer</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120">Item</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120">Category</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120">Kelas Harga 1</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120">Kelas Harga 2</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>
    </div>
</asp:content>
