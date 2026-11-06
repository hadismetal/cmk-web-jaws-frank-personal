<%@ Page Language="C#" MasterPageFile="~/MasterPagePrint.master" AutoEventWireup="true" CodeFile="Mounting_ListPrint.aspx.cs" Inherits="Page_Mounting" Debug="true"%>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <div id="form_area" runat="server">
        <div>
            <h2>Stock Receiving Detail</h2>
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
            <asp:Table ID="tb" runat="server" CssClass="table-list table-striped text-xs">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="90">No. PLU</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="90">Tanggal Approve</asp:TableHeaderCell>
					<asp:TableHeaderCell Width="120">Stock Keeper</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">Kode Karet</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">Kode Barang</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">No. SPK</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">Stone Dist.</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120">Item</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Product Category</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Segmen</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="90">Qty</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Net Weight</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Gross Weight</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Frame Price M</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Frame Price R</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80">NO M</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80">NO M Lot</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80">NO R</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80">RO M</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80">RO R</asp:TableHeaderCell>
					<asp:TableHeaderCell Width="120">Finishing M</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120">Finishing R</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120">Ongkos Lain M</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120">Ongkos Lain R</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120">T. Harga M <span class="text-muted">USD</span></asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120">T. Harga R <span class="text-muted">USD</span></asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120">Mark Up <span class="text-muted">%</span></asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120">H. Jual <span class="text-muted">Rp</span></asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120">Supplier</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>
    </div>
</asp:Content>

