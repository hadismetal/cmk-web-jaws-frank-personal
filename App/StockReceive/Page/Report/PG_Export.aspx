<%@ Page Language="C#" masterpagefile="~/MasterPageExport.master" AutoEventWireup="true" CodeFile="PG_Export.aspx.cs" Inherits="Page_Report_PG_Export" %>

<asp:content contentplaceholderid="content" runat="server">

    <div id="form_area" runat="server">
        
        <div class="table-responsive">
            <asp:Table ID="tb" runat="server" CssClass="table-list text-sm">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="80">No. TTB</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="90">Tanggal</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80">Product Level</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="70">Model</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80">Kadar Logam</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="70">Frame Color</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Supplier</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="30">Qty</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="70">Total Weight</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="70">Ongkos</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="70">Harga</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="90">Total Berat (24K)</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="70">Total Harga</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>

    </div>
</asp:content>
