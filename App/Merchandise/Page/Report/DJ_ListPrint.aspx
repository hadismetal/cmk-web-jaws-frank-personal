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
                    <asp:TableHeaderCell Width="100">No. PLU</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Kode Karet</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="70">Item</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80">Product Category</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="40">Qty</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="50">Status</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="90">Lokasi</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="70">Harga</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">H. Jual</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="60">Net Weight</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="60">Gross Weight</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="30">Butir</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="50">Carat</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="60">Segmen</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Stone Distribution</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="70">Umur(Hari)</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>

    </div>
</asp:content>

