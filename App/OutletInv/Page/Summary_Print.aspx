<%@ Page Language="C#" MasterPageFile="~/MasterPagePrint.master" AutoEventWireup="true" CodeFile="Summary_Print.aspx.cs" Inherits="Page_Summary" %>

<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="server">
    <div id="form_area" runat="server">
        <div class="table-responsive">
            Outlet : <asp:Literal ID="outlet" runat="server" /><br />
            Tanggal Cetak <asp:Literal ID="printtime" runat="server" />
        </div>
        <div class="table-responsive">
            <asp:Table ID="tb" runat="server" CssClass="table-list" Visible="false">
                <asp:TableRow>
                    <asp:TableHeaderCell Width="120">Tipe Lokasi</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="200">Lokasi</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="90">Stock</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Net Weight</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80">Gross Weight</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80">Butir</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80">Carat</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">M</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">R</asp:TableHeaderCell>
                </asp:TableRow>
            </asp:Table>
            <asp:Table ID="tb2" runat="server" CssClass="table-list" Visible="false">
                <asp:TableRow>
                    <asp:TableHeaderCell Width="140">Keterangan</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120">Status</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="90">Stock</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Net Weight</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80">Gross Weight</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80">Butir</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80">Carat</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">M</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">R</asp:TableHeaderCell>
                </asp:TableRow>
            </asp:Table>
            <asp:Table ID="tb3" runat="server" CssClass="table-list">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="180" CssClass="text-center">Keterangan</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120" CssClass="text-center">Qty</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="200" CssClass="text-center">T. Harga Jual</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140" CssClass="text-center">Gross Weight</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140" CssClass="text-center">Net Weight</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140" CssClass="text-center">Total Carat</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140" CssClass="text-center">Total Butir</asp:TableHeaderCell>

                </asp:TableHeaderRow>
            </asp:Table>
            <asp:Table ID="tb4" runat="server" CssClass="table-list text-sm" Visible="false">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="150" CssClass="text-center">Keterangan</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80" CssClass="text-right">Jual</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80" CssClass="text-right">Resell</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80" CssClass="text-right">Trade In</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80" CssClass="text-right">Titipan Customer</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80" CssClass="text-right">Reparasi</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80" CssClass="text-right">Lebur</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80" CssClass="text-right">Hadiah</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80" CssClass="text-right">Ongoing Lebur</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80" CssClass="text-right">Broken</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80" CssClass="text-right">Outgoing - New / Package</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80" CssClass="text-right">Outgoing - Approved / On The Way</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80" CssClass="text-right">Lost / Trouble</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80" CssClass="text-right">Total</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>

    </div>
</asp:Content>

