<%@ Page Language="C#" masterpagefile="~/MasterPageExport.master" AutoEventWireup="true" CodeFile="PG_ListExport.aspx.cs" Inherits="Page_Report_PG_ListExport" %>

<asp:content contentplaceholderid="content" runat="server">

    <div id="form_area" runat="server">
        
        <div class="table-responsive">
            <asp:Table ID="tb" runat="server" CssClass="table-list text-sm">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="100">No. PLU</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Tgl Approval</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Status</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Consignment</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Lokasi</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Product Item</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80">Product Level</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="70">Model</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80">Kadar Logam</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="70">Frame Color</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="70">Size</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="30">Qty</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="70">Total Weight</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="70">Ongkos</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="70">Harga</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="90">Total Berat (24K)</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="70">Total Harga</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="70">Total Harga Jual</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>

    </div>
</asp:content>

