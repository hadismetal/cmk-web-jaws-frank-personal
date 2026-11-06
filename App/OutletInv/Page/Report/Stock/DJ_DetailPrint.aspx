<%@ page language="C#" masterpagefile="~/MasterPagePrint.master" autoeventwireup="true" codefile="DJ_DetailPrint.aspx.cs" inherits="Page_DJ" %>
<asp:content contentplaceholderid="content" runat="server">

    <div id="form_area" runat="server">
        <div>
            <h2>Diamond Jewellery Detail</h2>
        </div>
        <div>
            <div style="float: left;">
                <table class="table-file">
                    <tr>
                        <td class="text-label">Report DateTime </td>
                        <td>:</td>
                        <td id="report_datetime" runat="server" class="text-bold" />
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
            <asp:Table ID="tb" runat="server" CssClass="table-list table-striped text-sm">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="40">No.</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="90">No. PLU</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="90">Tanggal Input</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="90">Tanggal Approve</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">Kode Karet</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">Kode Barang</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120">Item</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Product Category</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Segmen</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Net Weight</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="70">T. Butir</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="70">T. Carat</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120">Harga</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120">Harga Promo</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>
        <asp:Literal ID="paging_text" runat="server" />
    </div>
</asp:content>

