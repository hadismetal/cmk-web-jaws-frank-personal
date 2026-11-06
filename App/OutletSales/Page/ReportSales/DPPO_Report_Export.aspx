<%@ Page Language="C#" MasterPageFile="~/MasterPageExport.master" AutoEventWireup="true" CodeFile="DPPO_Report_Export.aspx.cs" Inherits="Page_Summary" %>

<asp:Content ContentPlaceHolderID="content" runat="server">
        <div class="table-responsive">
            <asp:Table ID="tb" runat="server" CssClass="table-list text-sm">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="150">No. Dokumen</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Tanggal</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Lokasi</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Sales/JR</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Customer</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Kode Customer</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Sales Order</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Total DP</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Estimasi Harga</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Persentase</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="200">Keterangan</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Cash</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Kartu Kredit</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Transfer</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Debit</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Bridestory</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Bank</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>

        <div class="table-responsive">
            <asp:Table ID="tb2" runat="server" CssClass="table-list text-sm">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="150">No. Dokumen</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Tanggal</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Lokasi</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Sales/JR</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Customer</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Kode Customer</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Sales Order</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Total DP</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Estimasi Harga</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Persentase</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="200">Keterangan</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Cash</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Kartu Kredit</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Transfer</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Debit</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Bridestory</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Bank</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>
   
</asp:Content>

