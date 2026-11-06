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
                    <asp:TableHeaderCell Width="100">Tanggal Beli Asal</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Lokasi Beli Asal</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">No. Invoice Asal</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Harga Asal</asp:TableHeaderCell>
					<asp:TableHeaderCell Width="130">Harga DPP Asal</asp:TableHeaderCell>
					<asp:TableHeaderCell Width="100">PPN Asal</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Keterangan Beli Asal</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Tanggal Trade In</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120">Lokasi Trade In</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">No. Trade In</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">No. Sales Order</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">Customer</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="90">PLU</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120">Tipe Product</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120">Item</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120">Product Category</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="40">Qty</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80">Butir</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80">Carat</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80">Gross Weight</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80">Net Weight</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Harga Trade In</asp:TableHeaderCell>
					<asp:TableHeaderCell Width="130">Harga Sales Order Baru</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Kadar Tukaran Beli</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">M</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">R</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>

    </div>
</asp:Content>

