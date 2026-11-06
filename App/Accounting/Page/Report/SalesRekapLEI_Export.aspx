<%@ Page Language="C#" MasterPageFile="~/MasterPageExport.master" AutoEventWireup="true" CodeFile="SalesRekapLEI_Export.aspx.cs" Inherits="Page_Summary" %>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <h2 style="font-family:Consolas">Laporan Penjualan LEI</h2>
    <asp:Table ID="tb" runat="server" CssClass="table table-bordered table-responsive">
        <asp:TableHeaderRow Style="background-color: lightblue">
            <asp:TableHeaderCell Style="font-family: consolas">No.</asp:TableHeaderCell>
            <asp:TableHeaderCell Style="font-family: consolas">Tanggal</asp:TableHeaderCell>
            <asp:TableHeaderCell Style="font-family: consolas">No. Sales Order</asp:TableHeaderCell>
            <asp:TableHeaderCell Style="font-family: consolas">No. Referensi</asp:TableHeaderCell>
            <asp:TableHeaderCell Style="font-family: consolas">Lokasi</asp:TableHeaderCell>
            <asp:TableHeaderCell Style="font-family: consolas">Tipe</asp:TableHeaderCell>
            <asp:TableHeaderCell Style="font-family: consolas">PLU</asp:TableHeaderCell>
            <asp:TableHeaderCell Style="font-family: consolas">Item</asp:TableHeaderCell>
			<asp:TableHeaderCell style="font-family:consolas">Item Description</asp:TableHeaderCell>
            <asp:TableHeaderCell Style="font-family: consolas">Kode Karet</asp:TableHeaderCell>
            <asp:TableHeaderCell Style="font-family: consolas">Gold Level</asp:TableHeaderCell>
            <asp:TableHeaderCell Style="font-family: consolas">Gold Model</asp:TableHeaderCell>
            <asp:TableHeaderCell Style="font-family: consolas">Nama Customer</asp:TableHeaderCell>
            <asp:TableHeaderCell Style="font-family: consolas">Sales</asp:TableHeaderCell>
            <asp:TableHeaderCell Style="font-family: consolas">Qty</asp:TableHeaderCell>
            <asp:TableHeaderCell Style="font-family: consolas">M</asp:TableHeaderCell>
            <asp:TableHeaderCell Style="font-family: consolas">Harga</asp:TableHeaderCell>
            <asp:TableHeaderCell Style="font-family: consolas">Discount</asp:TableHeaderCell>
            <asp:TableHeaderCell Style="font-family: consolas">Discount Program</asp:TableHeaderCell>
            <asp:TableHeaderCell Style="font-family: consolas">Discount Voucher</asp:TableHeaderCell>
            <asp:TableHeaderCell Style="font-family: consolas">Discount Nominal</asp:TableHeaderCell>
            <asp:TableHeaderCell Style="font-family: consolas">Discount Promo</asp:TableHeaderCell>
            <asp:TableHeaderCell Style="font-family: consolas">Total Bayar</asp:TableHeaderCell>
            <asp:TableHeaderCell Style="font-family: consolas">Kadar Tukaran Beli</asp:TableHeaderCell>
            <asp:TableHeaderCell Style="font-family: consolas">Nett Weight</asp:TableHeaderCell>
            <asp:TableHeaderCell Style="font-family: consolas">LM Weight</asp:TableHeaderCell>
            <asp:TableHeaderCell Style="font-family: consolas">Rate LM</asp:TableHeaderCell>
            <asp:TableHeaderCell Style="font-family: consolas">Nominal Laku Emas</asp:TableHeaderCell>
			<asp:TableHeaderCell style="font-family: consolas">Price + PPN</asp:TableHeaderCell>
            <asp:TableHeaderCell Style="font-family: consolas">Price Exclude VAT</asp:TableHeaderCell>
            <asp:TableHeaderCell Style="font-family: consolas">PPN</asp:TableHeaderCell>
			<asp:TableHeaderCell Style="font-family: consolas">TGP Jual</asp:TableHeaderCell>
			<asp:TableHeaderCell Style="font-family: consolas">TGP Beli</asp:TableHeaderCell>
			<asp:TableHeaderCell style="font-family:consolas">Nominal Laku Emas (TGP Jual)</asp:TableHeaderCell>
			<asp:TableHeaderCell style="font-family:consolas">Price + PPN (TGP Jual)</asp:TableHeaderCell>
			<asp:TableHeaderCell style="font-family:consolas">Price Exclude VAT (TGP Jual)</asp:TableHeaderCell>
			<asp:TableHeaderCell style="font-family:consolas">PPN (TGP Jual)</asp:TableHeaderCell>
            <asp:TableHeaderCell Style="font-family: consolas">Jenis Transaksi</asp:TableHeaderCell>
            <asp:TableHeaderCell Style="font-family: consolas">EDC</asp:TableHeaderCell>
        </asp:TableHeaderRow>
    </asp:Table>
</asp:Content>
