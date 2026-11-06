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
            <asp:Table ID="tb" runat="server" CssClass="table-list table-striped text-sm" Font-Size="10px">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="100">No. PLU</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Tanggal Input</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Tanggal Approve</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Kode Karet</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Item</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Product Category</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Segmen</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Net Weight</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Parcel</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">T. Butir</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">T. Carat</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Total Harga</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>

    </div>
</asp:content>

