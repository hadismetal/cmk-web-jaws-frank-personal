<%@ Page Language="C#" MasterPageFile="~/MasterPagePrint.master" AutoEventWireup="true" CodeFile="SalesOrderReport_Print.aspx.cs" Inherits="SalesOrderReport_Print" %>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <div>
        <div class="table-responsive">
            <asp:Table ID="tb" runat="server" CssClass="table-list text-sm">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="40" />
                    <asp:TableHeaderCell Width="130">Tanggal</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">No. Referensi</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Nama Customer</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Sales</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="40">Qty</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Total</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Total Terbayar</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Total Belum dibayar</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="180">Keterangan</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>

    </div>
</asp:Content>

