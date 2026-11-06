<%@ Page Language="C#" MasterPageFile="~/MasterPagePrint.master" AutoEventWireup="true" CodeFile="SalesRekap_Print.aspx.cs" Inherits="Page_Summary" %>

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
                    <asp:TableHeaderCell Width="140">Kode Karet</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">Product Category</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Nama Customer</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Sales</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="40">Qty</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">M</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Harga</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Discount</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Discount Program</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Discount Voucher</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Discount Nominal</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Harga Net</asp:TableHeaderCell>
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
                    <asp:TableHeaderCell Width="130">MDR</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Bank</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>

    </div>
</asp:Content>

