<%@ Page Language="C#" MasterPageFile="~/MasterPagePrint.master" AutoEventWireup="true" CodeFile="DJ_Print.aspx.cs" Inherits="Page_DJ" %>

<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="server">
    <div id="form_area" runat="server">

        <div class="table-responsive">
            <asp:Table ID="tb" runat="server" CssClass="table-list text-sm">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="120">No. Dokumen</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="90">No. PLU</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="90">Tanggal Input</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="90">Tanggal Approve</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="140">Nama Supplier</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="140">Kode Karet</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="120">Item</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="150">Product Category</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="130">Segmen</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="90">Qty</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="100">Gross Weight</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="100">Net Weight</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="100">Frame Price</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="130">Parcel</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="70">T. Butir</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="70">T. Carat</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="100">GIA</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="100">Total Harga</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="80">NO</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="80">RO</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="100">Finishing</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="100">Stone Setting</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="120">Harga M <span class="text-muted">USD</span></asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="120">Harga Jual <span class="text-muted">Rp</span></asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="200">Keterangan</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>
    </div>
</asp:Content>

