<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PaymentDP_Detail_bak.aspx.cs" Inherits="Page_Summary" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Down Payment PO Detail
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
                    <asp:TableHeaderCell Width="130">Nomor</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="110">Payment Type</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="110">Card Type</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="110">EDC</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="110">Bank Issuer</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="110">Card No.</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="110">Program</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Total Bayar</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="110">MDR</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>

    </div>
</asp:Content>

