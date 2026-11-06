<%@ Page Language="C#" MasterPageFile="~/MasterPagePrint.master" AutoEventWireup="true" CodeFile="TradeInRekap_Export.aspx.cs" Inherits="Page_Summary" %>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <div id="zero" runat="server" visible="false">
        <i class="icon-ok icon-5x pull-left text-success"></i>
        <h4>Proses Selesai</h4>
        <p class="text-muted">
            Tidak ada transaksi penjualan
        </p>
        <div class="clearfix"></div>
        <hr />
    </div>
    <div id="form_area" runat="server">
        <div class="table-responsive">
            <asp:Table ID="tb" runat="server" CssClass="table-list text-sm">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="40" />
                    
                    <asp:TableHeaderCell Width="100">Tanggal Beli</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Lokasi Beli</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">No. Invoice</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Harga Awal</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Keterangan Beli</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Tanggal Trade In</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120">Lokasi Trade In</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">No. Sales Order</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">Customer</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="90">PLU</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120">Group</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120">Item</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120">Product Category</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="40">Qty</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Harga Trade In</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>

    </div>
</asp:Content>

