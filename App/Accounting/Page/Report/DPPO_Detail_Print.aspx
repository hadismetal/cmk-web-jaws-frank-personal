<%@ Page Language="C#" MasterPageFile="~/MasterPagePrint.master" AutoEventWireup="true" CodeFile="DPPO_Detail_Print.aspx.cs" Inherits="Page_Summary" %>

<asp:Content ContentPlaceHolderID="content" runat="server">
        <div class="table-responsive">
            <asp:Table ID="tb" runat="server" CssClass="table-list text-sm">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="150">No. Dokumen</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Tanggal</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Lokasi</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Customer</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Sales Order</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Total DP</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Estimasi</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Persentase</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="200">Keterangan</asp:TableHeaderCell>
					<asp:TableHeaderCell Width="200">Keterangan Void</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Cash / Tunai</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Kartu Kredit</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Transfer</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Debit</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Voucher</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Exchange</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Poin</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">DP PO</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Bank</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>
    </div>
</asp:Content>

