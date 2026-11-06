<%@ Page Language="C#" masterpagefile="~/MasterPageExport.master" AutoEventWireup="true" CodeFile="PG_Export.aspx.cs" Inherits="Page_Report_PG_List" %>

<asp:content contentplaceholderid="content" runat="server">
    
    <div id="form_area" runat="server">
        <div class="table-responsive">
            <asp:Table ID="tb" runat="server" CssClass="table-list">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="30">No.</asp:TableHeaderCell>                    
                    <asp:TableHeaderCell Width="150">No. Dokumen</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">No. PLU</asp:TableHeaderCell>
					<asp:TableHeaderCell Width="100">Kode Barang</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Tanggal</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Tanggal Approval</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">Nama Supplier</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Product Item</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="110">Product Level</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Model</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80">Kadar Logam</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80">Frame Color</asp:TableHeaderCell>
					<asp:TableHeaderCell Width="80">Ring Size</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Supplier</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="50">Qty</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Total Weight</asp:TableHeaderCell>
					<asp:TableHeaderCell Width="100">Total Resell Weight</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Ongkos</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Harga</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Total Harga</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">HPPM</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="200">Keterangan</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>

    </div>
</asp:content>
