<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PaymentDP_DetailCustomer.aspx.cs" Inherits="Page_Summary" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Payment
</asp:Content>

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
                    <asp:TableHeaderCell Width="130">Referensi</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="180">Nominal</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">Customer</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">EDC</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">Bank Issuer</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">CC Number</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">CC Name</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">CC</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">Program</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>

    </div>
</asp:Content>

