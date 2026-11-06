<%@ page language="C#" masterpagefile="~/MasterPagePrint.master" autoeventwireup="true" codefile="DJ_ListPrint.aspx.cs" inherits="Page_DJ" %>

<asp:content contentplaceholderid="content" runat="server">
    <div id="form_area" runat="server">
         <div>
            <h2>Diamond Jewellery List</h2>
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
                        <td class="text-label">Cabang</td>
                        <td>:</td>
                        <td id="cabang" runat="server" class="text-bold" />
                    </tr>
                    <tr>
                        <td class="text-label">Brand</td>
                        <td>:</td>
                        <td id="brand" runat="server" class="text-bold" />
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
                        <td class="text-label">Product Category</td>
                        <td>:</td>
                        <td id="category" runat="server" class="text-bold" />
                    </tr>
                    <tr>
                        <td class="text-label">Product Item</td>
                        <td>:</td>
                        <td id="item" runat="server" class="text-bold" />
                    </tr>

                </table>
            </div>
            <div style="clear: both"></div>
        </div>
        <div class="table-responsive">
             <asp:Table ID="tb" runat="server" CssClass="table-list text-xs" Font-Size="12px">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="30">No.</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="90">No. PLU</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">Kode Karet</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80">Item</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80">Product Category</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80">Qty</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80">Status</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Lokasi</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">M Awal</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">M</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">R</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">H. Jual</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Netto</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Gross</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Butir</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Carat</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Segmen</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Stone Distribution</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Umur (Hari)</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Bunga</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Harga Promo</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>

    </div>
</asp:content>

