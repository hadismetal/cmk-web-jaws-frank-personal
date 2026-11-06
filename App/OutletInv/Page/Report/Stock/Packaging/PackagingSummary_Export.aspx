<%@ Page Language="C#" MasterPageFile="~/MasterPagePrint.master" AutoEventWireup="true" CodeFile="PackagingSummary_Export.aspx.cs" Inherits="Page_Summary" %>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <div class="table-responsive">
        Outlet : <asp:Literal ID="outlet" runat="server" /><br />
        Tanggal Cetak <asp:Literal ID="printtime" runat="server" />
    </div>
    <div class="table-responsive">
        <asp:Table ID="tb" runat="server" CssClass="table-list" Visible="false" BorderWidth="1" BorderColor="Gray">
            <asp:TableRow>
                <asp:TableHeaderCell runat="server" Width="250" BorderWidth="1" BorderColor="Gray">Packaging</asp:TableHeaderCell>
                <asp:TableHeaderCell runat="server" Width="150" BorderWidth="1" BorderColor="Gray">Kode</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="100" BorderWidth="1" BorderColor="Gray">Stock Awal</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="100" BorderWidth="1" BorderColor="Gray">Receiving</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="100" BorderWidth="1" BorderColor="Gray">Incoming</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="100" BorderWidth="1" BorderColor="Gray">Outgoing</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="100" BorderWidth="1" BorderColor="Gray">Sales</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="100" BorderWidth="1" BorderColor="Gray">Ongoing</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="100" BorderWidth="1" BorderColor="Gray">Summary</asp:TableHeaderCell>
            </asp:TableRow>
        </asp:Table>
    </div>
</asp:Content>

