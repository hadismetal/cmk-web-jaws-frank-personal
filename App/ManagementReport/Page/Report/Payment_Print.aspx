<%@ Page Language="C#" MasterPageFile="~/MasterPagePrint.master" AutoEventWireup="true" CodeFile="Payment_Print.aspx.cs" Inherits="Page_Report_Payment_Print" %>

<asp:Content ID="Content1"  ContentPlaceHolderID ="content" runat="server">
    <div id="form_area" runat="server">
        <div>
            <h2> Payment</h2>
        </div>
    </div>
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
                <td class="text-label">Report DateTime </td>
                <td>:</td>
                <td id="report_datetime" runat="server" class="text-bold"></td>
            </tr>
            <tr>
                <td class="text-label">Tipe Lokasi</td>
                <td>:</td>
                <td>
                    <asp:Label ID="lbl_type_location" runat="server" CssClass="text-bold" />
                </td>
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
    <div style="clear: both"></div>
    <div class="table-responsive" style="overflow:hidden;">
        <asp:Table ID="tb" runat="server" CssClass="table-list table-striped text-sm" Font-Size="12px">
            <asp:TableHeaderRow>
                <asp:TableHeaderCell Width="40" />
                <asp:TableHeaderCell Width="130">Payment Type</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="140" HorizontalAlign="Right">Total Paid</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="140">MDR</asp:TableHeaderCell>
            </asp:TableHeaderRow>
        </asp:Table>
    </div>
 </asp:Content>
