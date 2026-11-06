<%@ Page Language="C#" MasterPageFile="~/MasterPagePrint.master" AutoEventWireup="true" CodeFile="ResellButikEmasRekap_Export.aspx.cs" Inherits="ResellButikEmasRekap_Export" %>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <div id="form_area" runat="server">
        <div class="table-responsive">
            <asp:Table ID="tb" runat="server" CssClass="table-list text-sm">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="40" />
                    <asp:TableHeaderCell Width="100">Tanggal Beli</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Lokasi Beli</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">No. Invoice</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Harga Awal</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Keterangan Beli</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Tanggal Resell</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120">Lokasi Resell</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">No. Resell</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">Customer</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="90">PLU</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120">Group</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120">Item</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120">Product Category</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="40">Qty</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Harga Resell</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>

    </div>
</asp:Content>
