<%@ Page Language="C#" MasterPageFile="~/MasterPagePrint.master" AutoEventWireup="true" CodeFile="InvoicePG_Rekap_Print.aspx.cs" Inherits="Page_Report_InvoicePG_Rekap_Print" %>

<asp:Content ID="Content1"  ContentPlaceHolderID ="content" runat="server">
    <div id="form_area" runat="server">
        <div>
            <h2>Sales Order Plain Gold Group by</h2>
        </div>
    </div>
    <div>
    <div style="float: left;">
        <table class="table-file">
            <tr>
                <td class="text-label">Periode</td>
                <td>:</td>
                <td>
                    <asp:Label id="lbl_from" runat="server" CssClass="text-bold" />
                    &nbsp; s/d &nbsp; 
                    <asp:Label id="lbl_to" runat="server" CssClass="text-bold" />
                </td>
                <td class="text-label">Group By</td>
                <td>:</td>
                <td>
                    <asp:Label ID="lbl_groupby" runat="server" CssClass="text-bold" />
                </td>
            </tr>
            <tr>
                <td class="text-label">Tipe Lokasi</td>
                <td>:</td>
                <td>
                    <asp:Label ID="lbl_type_location" runat="server" CssClass="text-bold" />
                </td>
                <td class="text-label">Report DateTime </td>
                 <td>:</td>
                 <td id="report_datetime" runat="server" class="text-bold"></td>
                
            </tr>
            <tr>
                <td class="text-label">Lokasi</td>
                <td>:</td>
                <td>
                    <asp:Label ID="lbl_location" runat="server" CssClass ="text-bold" />
                </td>
            </tr>
        </table>
    </div>
    </div>
     <div style="clear: both"></div>
     <div class="table-responsive" style="overflow:hidden;">
        <asp:Table ID="tb" runat="server" CssClass="table-list text-sm">
            <asp:TableHeaderRow>
                <asp:TableHeaderCell Width="40" />
                    <asp:TableHeaderCell Width="140">Keterangan</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="40">Qty</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Total Harga</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Discount</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Discount Program</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Discount Voucher</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Discount Nominal</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Weight</asp:TableHeaderCell>
            </asp:TableHeaderRow>
        </asp:Table>
    </div>
</asp:Content>