<%@ Page Language="C#" masterpagefile="~/MasterPagePrint.master" AutoEventWireup="true" CodeFile="PG_ListPrint.aspx.cs" Inherits="Page_Report_PG_ListPrint" %>

<asp:content contentplaceholderid="content" runat="server">
    <div id="form_area" runat="server">
         <div>
            <h2>Plain Gold List</h2>
        </div>
        <div>
            <div style="float: left;">
                <table class="table-file">
                    <tr>
                        <td class="text-label">Report DateTime </td>
                        <td>:</td>
                        <td id="report_datetime" runat="server" class="text-bold" />
                    </tr>
                    <tr>
                        <td class="text-label">Brand</td>
                        <td>:</td>
                        <td id="brand" runat="server" class="text-bold" />
                    </tr>
                    <tr>
                        <td class="text-label">Product Item</td>
                        <td>:</td>
                        <td id="item" runat="server" class="text-bold" />
                    </tr>
                </table>
            </div>
            <div style="float: left; padding: 0 0 0 50px;">
                <table class="table-file">
                    <tr>
                        <td class="text-label">Product Level</td>
                        <td>:</td>
                        <td id="level" runat="server" class="text-bold" />
                    </tr>
                    <tr>
                        <td class="text-label">Model</td>
                        <td>:</td>
                        <td id="model" runat="server" class="text-bold" />
                    </tr>
                    <tr>
                        <td class="text-label">Frame Color</td>
                        <td>:</td>
                        <td id="frame" runat="server" class="text-bold" />
                    </tr>
                </table>
            </div>
            <div style="float: left; padding: 0 0 0 50px;">
                <table class="table-file">
                    <tr>
                        <td class="text-label">Keywords</td>
                        <td>:</td>
                        <td></td>
                    </tr>
                    <tr>
                        <td colspan="3" id="keywords" runat="server" class="text-bold" />
                    </tr>
                </table>
            </div>
            <div style="clear: both"></div>
        </div>
        <div class="table-responsive">
             <asp:Table ID="tb" runat="server" CssClass="table-list text-sm" Font-Size="9px">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="100">No. PLU</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Tgl Approval</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Status</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Lokasi</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Kode Barang</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Product Item</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80">Product Level</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="70">Model</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80">Kadar Logam</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="70">Frame Color</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="30">Qty</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="70">Total Weight</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="90">Total Berat (24K)</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="70">Total Harga Jual</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>

    </div>
</asp:content>