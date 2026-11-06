<%@ Page Language="C#" MasterPageFile="~/MasterPageExport.master" AutoEventWireup="true" CodeFile="SalesRekap_Export.aspx.cs" Inherits="Page_Summary" %>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <div id="form_area" runat="server">
        <div class="table-responsive">
            <asp:Table ID="tb" runat="server" CssClass="table-list text-sm">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="40" />
                    <asp:TableHeaderCell Width="130">Tanggal</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">No. Sales Order</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">No. Referensi</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Lokasi</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">Tipe</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">PLU</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">Item</asp:TableHeaderCell>
					<asp:TableHeaderCell Width="140">Level</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">Kode Karet</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">Product Category</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">Segmen</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Nama Customer</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Given To</asp:TableHeaderCell>
					<asp:TableHeaderCell Width="150">Membership</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Sales</asp:TableHeaderCell>
					<asp:TableHeaderCell Width="300">Keterangan</asp:TableHeaderCell>
					<asp:TableHeaderCell Width="130">Tgl Void</asp:TableHeaderCell>
					<asp:TableHeaderCell Width="300">Keterangan Void</asp:TableHeaderCell>
					<asp:TableHeaderCell Width="40">Qty</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">M</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Harga</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Discount Normal Percentage</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Discount Membership Percentage</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Discount Normal Nominal</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Discount Promo Nominal</asp:TableHeaderCell>
					<asp:TableHeaderCell Width="150">Discount Membership Nominal</asp:TableHeaderCell>
					<asp:TableHeaderCell Width="150">Discount Voucher Nominal</asp:TableHeaderCell>
					<asp:TableHeaderCell Width="150">Discount Bank Nominal</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Discount Other Nominal</asp:TableHeaderCell>
					<asp:TableHeaderCell Width="150">Discount Round Nominal</asp:TableHeaderCell>
					<asp:TableHeaderCell Width="150">Extra Ring Size</asp:TableHeaderCell>
					<asp:TableHeaderCell Width="150">Fee Extra Ring Size Per Size</asp:TableHeaderCell>
					<asp:TableHeaderCell Width="150">Total Fee Extra Ring Size</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Harga Net</asp:TableHeaderCell>
					<asp:TableHeaderCell Width="100">Point Received</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Kadar Tukaran beli</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Price Exclude VAT</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">PPN</asp:TableHeaderCell>                    
                    <asp:TableHeaderCell Width="130">PPN Pembagi</asp:TableHeaderCell>                    
                    <asp:TableHeaderCell Width="100">Total Butir</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Total Carat</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Net Weight</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Cash / Tunai</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Kartu Kredit</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Transfer</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Debit</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Voucher</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Exchange</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Poin</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">DP PO</asp:TableHeaderCell>
					<asp:TableHeaderCell Width="100">Bridestory Pay</asp:TableHeaderCell>
					<asp:TableHeaderCell Width="100">QRIS</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">MDR</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">T. Terima</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Bank</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>

    </div>
</asp:Content>

