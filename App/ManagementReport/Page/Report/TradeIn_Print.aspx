<%@ Page Language="C#" MasterPageFile="~/MasterPagePrint.master" AutoEventWireup="true" CodeFile="TradeIn_Print.aspx.cs" Inherits="Page_Report_TradeIn_Print" %>
<asp:Content ContentPlaceHolderID="content" ID="content" runat="server">
<div id="form_area" runat="server">
    <div>
            <h2>Trade In</h2>
    </div>
</div>
<div style="float: left;">
    <table class="table-file">
        <tr>
            <td class="text-label">
                Search
            </td>
            <td>&nbsp;:&nbsp;</td>
            <td>
                <asp:Label ID="lbl_kw" runat="server" CssClass="text-bold"></asp:Label>
            </td>
            <td class="text-label">Lokasi</td>
            <td>&nbsp;:&nbsp;</td>
            <td>
                <asp:Label ID="lbl_location" runat="server" CssClass="text-bold"></asp:Label>
            </td>
        </tr>
        <tr>
            <td class="text-label">Periode</td>
            <td>:</td>
            <td>
                <asp:Label id="lbl_from" runat="server" CssClass="text-bold" />
                &nbsp; s/d &nbsp; 
                <asp:Label id="lbl_to" runat="server" CssClass="text-bold" />
            </td>
            <td class="text-label">Status</td>
            <td>&nbsp:&nbsp;</td>
            <td><asp:Label ID="lbl_status" runat="server" CssClass="text-bold"></asp:Label></td>
        </tr>
        <tr>
            <td class="text-label">TipeLokasi</td>
            <td>&nbsp;:&nbsp;</td>
            <td>
                <asp:Label ID="lbl_typelocation" runat="server" CssClass="text-bold"></asp:Label>   
            </td>
            <td class="text-label">Report DateTime</td>
            <td>&nbsp:&nbsp;</td>
            <td id="report_datetime" runat="server" class="text-bold"></td>
        </tr>
    </table>
</div>
<div style="clear: both"></div>
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