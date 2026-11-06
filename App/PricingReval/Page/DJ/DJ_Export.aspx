<%@ Page Language="C#" MasterPageFile="~/MasterPageExport.master" AutoEventWireup="true" CodeFile="DJ_Export.aspx.cs" Inherits="Page_DJ" %>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <style>
        td
        {
            vertical-align: top;
        }
    </style>
    <div id="form_area" runat="server">
        <form id="Form1" runat="server" class="form-inline">
            <div class="table-responsive">
                <asp:Table ID="tb" runat="server" CssClass="table-list text-sm">
                    <asp:TableHeaderRow>
                        <asp:TableHeaderCell Width="90">No. PLU</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="140">Kode Karet</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="80">Item</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="130">Harga</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="130">Harga Baru</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="130">Mutasi</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="130">M</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="130">M Baru</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="130">R</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="130">R Baru</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="130">Net Weight</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="150">Gross Weight</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="150">Butir</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="150">Carat</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="150">Segmen</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="150">Stone Distribution</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="150">Category</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="150">Product Level</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="150">Harga Promo</asp:TableHeaderCell>
                    </asp:TableHeaderRow>
                </asp:Table>
            </div>
        </form>
    </div>
</asp:Content>

