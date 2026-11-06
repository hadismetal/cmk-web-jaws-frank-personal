<%@ Page Language="C#" MasterPageFile="~/MasterPagePrint.master" AutoEventWireup="true" CodeFile="GJ_ListPrint.aspx.cs" Inherits="Page_GJ" %>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <div id="form_area" runat="server">
        <div>
            <h2>Stock Receiving List</h2>
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
            <asp:Table ID="tb" runat="server" CssClass="table-list text-sm" Font-Size="7px">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="50">No.</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">No. PLU</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Kode Karet</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Item</asp:TableHeaderCell>
                    
                    <asp:TableHeaderCell Width="80">Status</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Lokasi</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80">Qty</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120">Harga</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">M</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">R</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120">Net Weight</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120">Gross Weight</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Segmen</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Stone Distribution</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="200">Supplier</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>

    </div>
</asp:Content>

