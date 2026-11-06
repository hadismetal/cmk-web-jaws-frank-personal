<%@ Page Language="C#" MasterPageFile="~/MasterPagePrint.master" AutoEventWireup="true" CodeFile="Print_Product_Rekap.aspx.cs" Inherits="Page_Summary" %>


<asp:Content ContentPlaceHolderID="content" runat="server">
    <div class="table-responsive">
        Outlet : <asp:Literal ID="outlet" runat="server" /><br />
        Tanggal Cetak <asp:Literal ID="printtime" runat="server" />
    </div>
    <div class="table-responsive">
        <asp:Table ID="tb" runat="server" CssClass="table-list">
            <asp:TableRow>
                <asp:TableHeaderCell Width="40">No.</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="200">Nama</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="100">Kode</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="120">Lokasi</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="80">Satuan</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="100">Stock</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="100">Dalam Pengiriman</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="100">Rusak</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="100">Hilang</asp:TableHeaderCell>
            </asp:TableRow>
        </asp:Table>
    </div>
</asp:Content>

