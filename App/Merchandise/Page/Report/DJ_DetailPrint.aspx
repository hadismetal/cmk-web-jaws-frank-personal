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
            <asp:Table ID="tb" runat="server" CssClass="table-list table-striped text-xs">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="80">No. PLU</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80">Tanggal Approve</asp:TableHeaderCell>
					<asp:TableHeaderCell Width="110">Stock Keeper</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="110">Lokasi</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="110">Kode Karet</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="110">Supplier Nomor</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Item</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Product Category</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="90">Segmen</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="60">Qty</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="90">Net Weight</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120">Parcel</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="60">T. Butir</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="60">T. Carat</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">H. Jual <span class="text-muted">Rp</span></asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>

    </div>
</asp:content>

