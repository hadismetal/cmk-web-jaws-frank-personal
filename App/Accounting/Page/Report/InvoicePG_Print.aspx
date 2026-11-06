<%@ Page Language="C#" masterpagefile="~/MasterPagePrint.master" CodeFile="InvoicePG_Print.aspx.cs" Inherits="Page_Report_InvoicePG_Print" %>

<asp:content ID="Content1" contentplaceholderid="content" runat="server">
    <div id="form_area" runat="server">
         <div>
            <h2>Sales Order Plain Gold</h2>
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
                        <td class="text-label">Dari</td>
                        <td>:</td>
                        <td id="dari" runat="server" class="text-bold" />
                    </tr>
                     <tr>
                        <td class="text-label">Sampai</td>
                        <td>:</td>
                        <td id="sampai" runat="server" class="text-bold" />
                    </tr>
                </table>
            </div>
            <div style="float: left; padding: 0 0 0 50px;">
                <table class="table-file">
                    <tr>
                        <td class="text-label">Tipe Lokasi</td>
                        <td>:</td>
                        <td id="tipelokasi" runat="server" class="text-bold" />
                    </tr>
                    <tr>
                        <td class="text-label">Lokasi</td>
                        <td>:</td>
                        <td id="lokasi" runat="server" class="text-bold" />
                    </tr>
                    <tr>
                        <td class="text-label">Status</td>
                        <td>:</td>
                        <td id="status" runat="server" class="text-bold" />
                    </tr>

                </table>
            </div>
            <div style="clear: both"></div>
        </div>
        <div class="table-responsive">
            <asp:Table ID="tb" runat="server" CssClass="table-list text-sm">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="40" />
                    <asp:TableHeaderCell Width="150">Tanggal</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">No. Referensi</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">PLU</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">Item</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Nama Customer</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Sales</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="40">Qty</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Total Harga</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Discount</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Discount Program</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Discount Voucher</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Discount Nominal</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Net Weight</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Kode Supplier</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>

    </div>
</asp:content>
