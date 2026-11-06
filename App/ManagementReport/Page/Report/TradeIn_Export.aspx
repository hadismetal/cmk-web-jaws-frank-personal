<%@ Page Language="C#" MasterPageFile="~/MasterPageExport.master" AutoEventWireup="true" CodeFile="TradeIn_Export.aspx.cs" Inherits="Page_Report_TradeIn_Export" %>
<asp:Content ContentPlaceHolderID="content" ID="content" runat="server">

    <div class="table-responsive" style="overflow:hidden;">
        <asp:Table ID="tb" runat="server" CssClass="table-list table-striped text-sm" Font-Size="12px">
            <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="40" />
                    <asp:TableHeaderCell Width="150">Tanggal</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">No. Referensi</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Nama Customer</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">JPJR</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="70">Qty</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="90">Weight</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="90">Butir</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="90">Carat</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Total</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>
</asp:Content>