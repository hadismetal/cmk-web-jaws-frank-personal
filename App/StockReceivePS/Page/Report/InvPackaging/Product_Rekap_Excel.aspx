<%@ Page Language="C#" MasterPageFile="~/MasterPageExport.master" AutoEventWireup="true" CodeFile="Product_Rekap_Excel.aspx.cs" Inherits="Page_Summary" %>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <div id="form_area" runat="server">
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

    </div>
</asp:Content>

