<%@ page language="C#" masterpagefile="~/MasterPagePrint.master" autoeventwireup="true" codefile="DJ_Rekap_Print_20140705.aspx.cs" inherits="Page_DJ" %>

<asp:content contentplaceholderid="content" runat="server">
    <div id="form_area" runat="server" style="max-width:1070px">
        <div>
            <h2>Stock Receiving List</h2>
        </div>
        <div>
            <div style="float: left;">
                <table class="table-file">
                    <tr>
                        <td class="text-label">Cabang</td>
                        <td>:</td>
                        <td id="cabang" runat="server" class="text-bold" />
                    </tr>
                    <tr>
                        <td class="text-label">Periode</td>
                        <td>:</td>
                        <td id="periode" runat="server" class="text-bold" />
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
            <asp:Table ID="tb" runat="server" CssClass="table-list" Font-Size="12px">
                <asp:TableRow>
                    <asp:TableHeaderCell Width="140">Keterangan</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="70">Status</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="50">Stock</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Net Weight</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80">Gross Weight</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="60">Butir</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="50">Carat</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80">M</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80">R</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Harga</asp:TableHeaderCell>
                </asp:TableRow>
            </asp:Table>

            <asp:Table ID="tb2" runat="server" CssClass="table-list">
                <asp:TableRow>
                    <asp:TableHeaderCell Width="140">Keterangan</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="70">Status</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="50">Stock</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Net Weight</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80">Gross Weight</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="60">Butir</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="50">Carat</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80">M</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80">R</asp:TableHeaderCell>
                </asp:TableRow>
            </asp:Table>
        </div>

    </div>
</asp:content>

