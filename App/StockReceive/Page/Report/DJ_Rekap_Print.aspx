<%@ Page Language="C#" MasterPageFile="~/MasterPagePrint.master" AutoEventWireup="true" CodeFile="DJ_Rekap_Print.aspx.cs" Inherits="Page_Summary" %>

<asp:Content ContentPlaceHolderID="content" runat="server">

    <div id="form_area" runat="server">
        <div>
            <h2>Diamond Jewellery Rekap</h2>
        </div>
        <div>
            <div style="float: left;">
                <table class="table-file">
                    <tr>
                        <td class="text-label">Report DateTime </td>
                        <td>:</td>
                        <td id="report_datetime" runat="server" class="text-bold"></td>
                    </tr>
                    <tr>
                        <td class="text-label">Cabang</td>
                        <td>:</td>
                        <td id="cabang" runat="server" class="text-bold" />
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
                    <tr>
                        <td class="text-label">Supplier</td>
                        <td>:</td>
                        <td id="supplier" runat="server" class="text-bold" />
                    </tr>
                </table>
            </div>
            <div style="clear: both"></div>
        </div>
        <div class="table-responsive">
            <asp:Table ID="tb" runat="server" CssClass="table-list text-xs">
                <asp:TableRow>
                    <asp:TableHeaderCell Width="100">Keterangan</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="50">Stock</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="70">Net Weight</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="70">Gross Weight</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="70">Butir</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="70">Carat</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="90">M</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="90">R</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="90">H. Jual</asp:TableHeaderCell>
                </asp:TableRow>
            </asp:Table>


        </div>

    </div>
</asp:Content>

