<%@ Page Language="C#" masterpagefile="~/MasterPagePrint.master" CodeFile="Sales_Print.aspx.cs" Inherits="Page_Report_Sales_Print" %>

<asp:content contentplaceholderid="content" runat="server">
    <div id="form_area" runat="server">
         <div>
            <h2>Sales Order Summary</h2>
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
                        <td class="text-label">Group by</td>
                        <td>:</td>
                        <td id="groupby" runat="server" class="text-bold" />
                    </tr>

                </table>
            </div>
            <div style="clear: both"></div>
        </div>
        <div class="table-responsive">
            <asp:Table ID="tb" runat="server" CssClass="table-list text-sm">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="90">Keterangan</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100" CssClass="text-right">Qty</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150" CssClass="text-right">Jumlah</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150" CssClass="text-right">Discount</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150" CssClass="text-right">Discount Program</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150" CssClass="text-right">Discount Voucher</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150" CssClass="text-right">Discount Nominal</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150" CssClass="text-right">Total Harga</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130" CssClass="text-right">Total Butir</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130" CssClass="text-right">Total Carat</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130" CssClass="text-right">Net Weight (gr)</asp:TableHeaderCell>

                </asp:TableHeaderRow>
            </asp:Table>
        </div>
        <hr />
        <div class="table-responsive">
            <asp:Table ID="tb2" runat="server" CssClass="table-list text-sm">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="90">Keterangan</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="200" ColumnSpan="2" style="text-align: center;">Sales</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="200" ColumnSpan="2" style="text-align: center;">Trade In / Exchange</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="200" ColumnSpan="2" style="text-align: center;">Resell</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="200" ColumnSpan="2" style="text-align: center;">Net Omzet</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>

    </div>
</asp:content>
